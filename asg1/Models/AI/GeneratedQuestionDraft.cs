using asg1.Models.Enums;

namespace asg1.Models.AI;

// This is the AI result shown for lecturer review, not a database entity.
// It can be mapped to the team's Question entity after P1 is merged.
public sealed class GeneratedQuestionDraft
{
    public string Content { get; init; } = string.Empty;
    public string SuggestedAnswer { get; init; } = string.Empty;
    public BloomLevel BloomLevel { get; init; }
    public QuestionSource Source => QuestionSource.AIGenerated;
    public QuestionStatus Status => QuestionStatus.Draft;
}
