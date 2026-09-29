using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using asg1.BLL.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace asg1.BLL.AI
{
    public class GeminiQuestionGenerator : IAiQuestionGenerator
    {
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() }
        };

        private readonly HttpClient _httpClient;
        private readonly GeminiOptions _options;
        private readonly ILogger<GeminiQuestionGenerator> _logger;

        public GeminiQuestionGenerator(
            HttpClient httpClient,
            IOptions<GeminiOptions> options,
            ILogger<GeminiQuestionGenerator> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<IReadOnlyList<GeneratedQuestionDto>> GenerateAsync(
            Stream document,
            string fileName,
            string contentType,
            int questionCount,
            CancellationToken cancellationToken = default)
        {
            ValidateConfiguration();

            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await document.CopyToAsync(memoryStream, cancellationToken);
                fileBytes = memoryStream.ToArray();
            }

            var safeFileName = Path.GetFileName(fileName);
            var normalizedContentType = NormalizeContentType(contentType, safeFileName);
            var requestBody = new
            {
                model = _options.Model,
                store = false,
                input = new object[]
                {
                    new
                    {
                        type = "document",
                        data = Convert.ToBase64String(fileBytes),
                        mime_type = normalizedContentType
                    },
                    new
                    {
                        type = "text",
                        text = $"""
                            Bạn là giảng viên đang xây dựng ngân hàng câu hỏi vấn đáp.
                            Tên tài liệu: {safeFileName}.
                            Chỉ dựa trên tài liệu đính kèm, hãy tạo đúng {questionCount} câu hỏi bằng tiếng Việt.
                            Câu hỏi phải rõ ràng, không trùng ý, có đáp án gợi ý ngắn gọn và phân loại theo Bloom:
                            Remember, Understand, Apply hoặc Analyze.
                            Không đưa thông tin không có căn cứ trong tài liệu.
                            """
                    }
                },
                response_format = new
                {
                    type = "text",
                    mime_type = "application/json",
                    schema = BuildResponseSchema(questionCount)
                },
                generation_config = new
                {
                    max_output_tokens = 4000
                }
            };

            HttpResponseMessage response;
            try
            {
                response = await SendWithRetryAsync(requestBody, cancellationToken);
            }
            catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(exception, "Could not call the Gemini question generation API.");
                throw new AiQuestionGenerationException(
                    "Không thể kết nối tới Gemini. Vui lòng kiểm tra mạng và cấu hình API.",
                    exception);
            }

            using (response)
            {
                var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Gemini API returned HTTP {StatusCode}. Response: {Response}",
                        (int)response.StatusCode,
                        responseJson);

                    throw new AiQuestionGenerationException(
                        $"Gemini trả về lỗi HTTP {(int)response.StatusCode}. Hãy kiểm tra API key, model và hạn mức free tier.");
                }

                try
                {
                    var outputText = ExtractOutputText(responseJson);
                    var envelope = JsonSerializer.Deserialize<GeneratedQuestionsEnvelope>(outputText, SerializerOptions);

                    if (envelope?.Questions is null || envelope.Questions.Count == 0)
                    {
                        throw new AiQuestionGenerationException("Gemini không trả về câu hỏi hợp lệ.");
                    }

                    return envelope.Questions;
                }
                catch (AiQuestionGenerationException)
                {
                    throw;
                }
                catch (Exception exception) when (exception is JsonException or InvalidOperationException)
                {
                    _logger.LogError(exception, "Could not parse the Gemini response.");
                    throw new AiQuestionGenerationException("Không đọc được kết quả do Gemini trả về.", exception);
                }
            }
        }

        private async Task<HttpResponseMessage> SendWithRetryAsync(
            object requestBody,
            CancellationToken cancellationToken)
        {
            const int maxAttempts = 3;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"{_options.BaseUrl.TrimEnd('/')}/interactions");
                request.Headers.TryAddWithoutValidation("x-goog-api-key", _options.ApiKey);
                request.Content = JsonContent.Create(requestBody, options: SerializerOptions);

                var response = await _httpClient.SendAsync(request, cancellationToken);
                var shouldRetry = response.StatusCode is HttpStatusCode.TooManyRequests
                    or HttpStatusCode.ServiceUnavailable;

                if (!shouldRetry || attempt == maxAttempts)
                {
                    return response;
                }

                _logger.LogWarning(
                    "Gemini returned HTTP {StatusCode}. Retrying request ({Attempt}/{MaxAttempts}).",
                    (int)response.StatusCode,
                    attempt + 1,
                    maxAttempts);

                response.Dispose();
                await Task.Delay(TimeSpan.FromSeconds(attempt), cancellationToken);
            }

            throw new InvalidOperationException("Gemini retry loop ended unexpectedly.");
        }

        private void ValidateConfiguration()
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                throw new AiQuestionGenerationException(
                    "Chưa cấu hình Gemini API key. Hãy đặt Gemini:ApiKey trong appsettings.json hoặc biến môi trường Gemini__ApiKey.");
            }

            if (string.IsNullOrWhiteSpace(_options.Model))
            {
                throw new AiQuestionGenerationException("Chưa cấu hình model trong Gemini:Model.");
            }

            if (!Uri.TryCreate(_options.BaseUrl, UriKind.Absolute, out _))
            {
                throw new AiQuestionGenerationException("Gemini:BaseUrl không hợp lệ.");
            }
        }

        private static object BuildResponseSchema(int questionCount) => new
        {
            type = "object",
            properties = new
            {
                questions = new
                {
                    type = "array",
                    minItems = questionCount,
                    maxItems = questionCount,
                    items = new
                    {
                        type = "object",
                        properties = new
                        {
                            content = new
                            {
                                type = "string",
                                description = "Nội dung câu hỏi vấn đáp bằng tiếng Việt"
                            },
                            suggestedAnswer = new
                            {
                                type = "string",
                                description = "Đáp án gợi ý ngắn gọn, dựa trên tài liệu"
                            },
                            bloomLevel = new
                            {
                                type = "string",
                                @enum = new[] { "Remember", "Understand", "Apply", "Analyze" }
                            }
                        },
                        required = new[] { "content", "suggestedAnswer", "bloomLevel" },
                        additionalProperties = false
                    }
                }
            },
            required = new[] { "questions" },
            additionalProperties = false
        };

        private static string ExtractOutputText(string responseJson)
        {
            using var document = JsonDocument.Parse(responseJson);

            if (document.RootElement.TryGetProperty("status", out var status) &&
                status.GetString() is not "completed")
            {
                throw new AiQuestionGenerationException(
                    $"Gemini chưa hoàn thành yêu cầu (trạng thái: {status.GetString() ?? "không xác định"}).");
            }

            if (!document.RootElement.TryGetProperty("steps", out var steps))
            {
                throw new InvalidOperationException("The Gemini response has no steps array.");
            }

            foreach (var step in steps.EnumerateArray().Reverse())
            {
                if (!step.TryGetProperty("type", out var stepType) ||
                    stepType.GetString() != "model_output" ||
                    !step.TryGetProperty("content", out var content))
                {
                    continue;
                }

                foreach (var part in content.EnumerateArray())
                {
                    if (part.TryGetProperty("type", out var type) &&
                        type.GetString() == "text" &&
                        part.TryGetProperty("text", out var text))
                    {
                        return text.GetString()
                            ?? throw new InvalidOperationException("The Gemini output text is empty.");
                    }
                }
            }

            throw new InvalidOperationException("The Gemini response has no output text.");
        }

        private static string NormalizeContentType(string contentType, string fileName)
        {
            if (!string.IsNullOrWhiteSpace(contentType) && contentType != "application/octet-stream")
            {
                return contentType;
            }

            return Path.GetExtension(fileName).ToLowerInvariant() switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                ".md" => "text/markdown",
                _ => "text/plain"
            };
        }

        private sealed class GeneratedQuestionsEnvelope
        {
            public List<GeneratedQuestionDto> Questions { get; init; } = [];
        }
    }
}
