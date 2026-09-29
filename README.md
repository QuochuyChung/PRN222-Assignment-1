# asg1 – AI-powered Viva Exam System / AIVES (Hệ thống thi vấn đáp thông minh có AI)

> Ghi chú: "AIVES" là tên viết tắt của hệ thống theo đúng đề bài gốc (AI-powered Viva Exam System), không phải tên project trong code. Project/solution trong repo này được đặt tên là **`asg1`**.

## 1. Giới thiệu

Thi vấn đáp (viva/oral exam) hiện dùng trong bảo vệ đồ án, thi cuối kỳ, phỏng vấn năng lực... nhưng tốn nhiều thời gian giảng viên, khó chuẩn hóa câu hỏi/thang điểm giữa các phòng thi, khó mở rộng khi số lượng sinh viên lớn, và thiếu bằng chứng khách quan (transcript, điểm từng câu) khi có khiếu nại điểm.

AIVES hỗ trợ giảng viên: sinh câu hỏi, tổ chức buổi vấn đáp có AI đặt câu hỏi/hỏi xoáy theo câu trả lời, hỗ trợ chấm điểm dựa trên rubric, và lưu vết toàn bộ quá trình thi.

## 2. Yêu cầu đề bài — 7 nhóm chức năng

### Nhóm 1: Quản lý ngân hàng câu hỏi & rubric
- Giảng viên tạo, import hoặc dùng AI sinh câu hỏi vấn đáp theo môn học/chủ đề từ giáo trình, slide (RAG trên tài liệu môn học).
- Câu hỏi gắn nhãn mức độ nhận thức theo thang Bloom (nhớ, hiểu, vận dụng, phân tích) và gắn rubric chấm điểm (tiêu chí + thang điểm).
- Giảng viên duyệt, chỉnh sửa, loại bỏ câu hỏi AI sinh ra trước khi đưa vào ngân hàng chính thức.

### Nhóm 2: Quản lý kỳ thi & lịch thi
- Tạo phiên thi vấn đáp (gắn môn học, danh sách sinh viên, khung thời gian mỗi thí sinh).
- Chọn ngẫu nhiên/thích ứng bộ câu hỏi cho từng sinh viên để tránh trùng lặp giữa các thí sinh thi liên tiếp; đặt số câu hỏi chính + số câu hỏi đào sâu tối đa mỗi thí sinh.

### Nhóm 3: Lõi phỏng vấn AI — **bắt buộc phải có trong Group Project**
- AI đóng vai giám khảo ảo: đọc câu hỏi bằng giọng nói (TTS), sinh viên trả lời bằng giọng nói, hệ thống chuyển giọng nói sang văn bản (STT) theo thời gian gần thực.
- Dựa trên nội dung câu trả lời, AI sinh câu hỏi hỏi xoáy/làm rõ (adaptive follow-up) khi câu trả lời còn mơ hồ, thiếu ý hoặc mâu thuẫn.
- Giới hạn thời gian trả lời và số lượt hỏi xoáy tối đa mỗi câu.

### Nhóm 4: Hỗ trợ chấm điểm bằng AI
- Đối chiếu transcript câu trả lời với đáp án/rubric để đưa ra điểm gợi ý và nhận xét (điểm mạnh/yếu, ý còn thiếu).
- Giảng viên xem transcript đầy đủ + điểm AI đề xuất, có quyền điều chỉnh và quyết định điểm cuối cùng (human-in-the-loop). Có thể ghi nhận tín hiệu phụ như thời gian trả lời, độ trôi chảy.

### Nhóm 5: Giám sát & minh bạch
- Ghi âm/ghi hình (nếu online) toàn bộ buổi thi làm bằng chứng khi có khiếu nại điểm.
- Lưu log toàn bộ câu hỏi, câu trả lời, điểm AI đề xuất và điểm giảng viên chốt cho từng thí sinh.

### Nhóm 6: Phản hồi & báo cáo
- Sinh viên xem lại báo cáo sau khi thi: điểm từng câu, nhận xét của AI.
- Giảng viên xem thống kê toàn lớp (câu hỏi khó nhất, tỷ lệ trả lời tốt, phân bố điểm) và xuất bảng điểm theo mẫu trường.

### Nhóm 7: Quản trị hệ thống
- Quản lý tài khoản, phân quyền giảng viên/môn học, cấu hình ngôn ngữ (Việt/Anh) cho STT/TTS.

### Yêu cầu phi chức năng
- Độ trễ giữa lúc sinh viên trả lời xong và AI đặt câu hỏi tiếp theo phải đủ nhanh để không phá vỡ nhịp vấn đáp.
- Độ chính xác STT tiếng Việt cho thuật ngữ chuyên ngành.
- Bảo mật/quyền riêng tư dữ liệu ghi âm sinh viên.

## 3. Yêu cầu môn học (PRN212)

- **Assignment 1**: implement **1 luồng chính** (tự chọn 1 trong 7 nhóm chức năng) chạy end-to-end, theo **MVC pattern** + **kiến trúc 3-layer**.
- **Assignment 2**: thêm **1 luồng nữa** để demo.
- **Group Project** (tổng thể): bắt buộc phải có **Nhóm chức năng 3 – Lõi phỏng vấn AI**.
- Không tách 3-layer rõ ràng = **0 điểm**.
- Connection string bắt buộc nằm trong `appsettings.json`, không hardcode trong code.

**Luồng đã chọn cho asg1: Nhóm 1 – Quản lý ngân hàng câu hỏi & rubric.**
Lý do: CRUD thuần, không phụ thuộc AI/voice phức tạp, và là dữ liệu nền mà Nhóm 2 (lịch thi) và Nhóm 3 (AI core) sẽ cần dùng lại sau này.

## 4. Kiến trúc & công nghệ

- **.NET 8**, ASP.NET Core MVC theo **kiến trúc 3 lớp**. Mỗi lớp là 1 project riêng trong solution, phụ thuộc 1 chiều `asg1 → asg1.BLL → asg1.DAL` (lớp dưới không biết gì về lớp trên):
  - **`asg1` (Presentation)**: MVC. Controller nhận request từ View, gọi Service ở BLL rồi trả kết quả cho View. `Models/` ở đây là ViewModel (dữ liệu dành riêng cho từng màn hình). Controller không truy vấn DB trực tiếp.
  - **`asg1.BLL` (Business Logic)**: Service xử lý nghiệp vụ và validate, DTO trả dữ liệu cho Controller, tích hợp AI (Gemini). Chỉ gọi xuống DAL qua interface Repository.
  - **`asg1.DAL` (Data Access)**: Entity, `AppDbContext` (EF Core), Repository, Migrations. Đây là nơi duy nhất chạm vào database.
- "Model" theo nghĩa MVC chính là `asg1.BLL` + `asg1.DAL`: nơi Controller gọi vào để lấy/ghi dữ liệu.
- Database: SQL Server (EF Core Code-First), connection string trong `asg1/appsettings.json`.

Luồng dữ liệu: `View → Controller → Service (BLL) → Repository (DAL) → SQL Server`, chiều về đi ngược lại. Dữ liệu đổi dạng theo từng lớp: `Entity (DAL) → DTO (BLL) → ViewModel (Presentation) → View`.

```
asg1.slnx
├── asg1                               (Presentation - MVC)
│   ├── Controllers/                   (Home, Questions, AiQuestions)
│   ├── Models/                        (ViewModel cho View)
│   ├── Views/
│   └── appsettings.json               (connection string + Gemini key, bị .gitignore)
├── asg1.BLL                           (Business Logic)
│   ├── Services/                      (IQuestionService, IAiQuestionService, ...)
│   ├── Dtos/
│   ├── AI/                            (tích hợp Gemini sinh câu hỏi)
│   └── Common/                        (ServiceResult, ErrorKeys)
└── asg1.DAL                           (Data Access)
    ├── Entities/                      (Subject, Question, RubricCriterion)
    ├── Enums/                         (BloomLevel, QuestionStatus, QuestionSource)
    ├── Data/                          (AppDbContext, DbSeeder)
    ├── Repositories/                  (IGenericRepository<T>, repo cụ thể)
    └── Migrations/
```

## 5. Tiến độ hiện tại

Đã làm:
- [x] Solution 3 project theo kiến trúc 3 lớp: `asg1` (Presentation), `asg1.BLL`, `asg1.DAL` (net8.0), phụ thuộc 1 chiều `asg1 → asg1.BLL → asg1.DAL`.
- [x] Entity `Subject`, `Question` (FK `SubjectId`), `RubricCriterion` (FK `QuestionId`); enum `BloomLevel`, `QuestionStatus`, `QuestionSource`.
- [x] `AppDbContext` với 3 `DbSet` và quan hệ FK (`Subject 1-N Question`, `Question 1-N RubricCriterion`); `DbSeeder` tạo sẵn vài môn học mẫu.
- [x] Repository: `IGenericRepository<T>` / `GenericRepository<T>`, `ISubjectRepository`, `IQuestionRepository` (tìm kiếm, lọc theo môn/trạng thái, kiểm tra trùng nội dung).
- [x] Migration `InitialCreate` + `AddQuestionAndRubric` đã chạy lên database của nhóm: có bảng `Subjects`, `Questions`, `RubricCriteria`.
- [x] **CRUD câu hỏi thủ công** (`/Questions`): danh sách, tìm kiếm, lọc theo môn học và trạng thái, tạo (có "Lưu & tạo thêm"), sửa, xóa, xem chi tiết. Gắn mức Bloom khi tạo/sửa. Đi qua `QuestionService` (BLL) có validate và chặn trùng nội dung trong cùng môn.
- [x] **AI sinh câu hỏi bằng Gemini** (`/AiQuestions`): tải PDF/Word/PowerPoint/TXT/Markdown (tối đa 20 MB), chọn môn học và số câu, câu hỏi sinh ra được lưu với `Source = AIGenerated`, `Status = Draft`. Cần Gemini API key (xem mục 7).
- [x] Kết nối database thật của nhóm; Docker SQL Server local làm phương án dự phòng (`docker-compose.yml`).
- [x] `appsettings.example.json` làm mẫu; `.gitignore` loại `appsettings.json` thật và thư mục `db/` ra khỏi git.
- [x] Build cả solution: 0 lỗi, 0 warning.

Chưa làm (còn lại để hoàn thành asg1):
- [ ] **Quản lý rubric**: chưa có màn hình thêm/sửa/xóa `RubricCriterion` cho từng câu hỏi (trang chi tiết mới chỉ hiển thị rubric nếu có).
- [ ] **Luồng duyệt câu hỏi**: chưa có thao tác chuyển `Draft → Approved/Rejected`. Trạng thái mới chỉ để hiển thị và lọc, câu hỏi AI sinh ra đang nằm ở `Draft`.
- [ ] Câu hỏi do AI sinh ra chưa được kiểm tra trùng nội dung như câu tạo tay, nên tải cùng một tài liệu 2 lần có thể tạo câu trùng.
- [ ] Chưa có entity `Lecturer`/`Account` (Nhóm 7) để lưu người tạo câu hỏi (`CreatedBy`), để dành cho asg2/group project.

## 6. Đề xuất chia task cho nhóm

**Lưu ý quan trọng:** giảng viên yêu cầu asg1 chỉ làm **1 nhóm chức năng duy nhất cho cả nhóm** (không phải mỗi người 1 nhóm khác nhau). Nhóm đã chọn Nhóm 1 – Ngân hàng câu hỏi & rubric, nên việc chia task dưới đây là **chẻ nhỏ chính nhóm chức năng 1 ra thành 5 phần** để 5 người làm song song trên cùng 1 luồng, KHÔNG đụng tới Nhóm 2-7 trong asg1.

| Người | Việc | Mô tả | Phụ thuộc |
|---|---|---|---|
| **P1 — Nền tảng chung** | DbContext DI, connection string, migration | Hoàn thiện `Program.cs` (đăng ký `AppDbContext`), tạo entity `Question` + `RubricCriterion`, migration đầu tiên (connection string đã có sẵn trong `appsettings.json`) | Làm trước, ưu tiên xong sớm nhất để 4 người sau có DB dùng |
| **P2 — CRUD câu hỏi thủ công** | Controller + View cho `Question` | Tạo/sửa/xóa/danh sách/chi tiết câu hỏi, lọc theo `Subject`, tìm kiếm | Cần entity `Question` từ P1 |
| **P3 — Quản lý rubric** | Controller + View cho `RubricCriterion` | Thêm/sửa/xóa tiêu chí chấm điểm gắn với từng câu hỏi, thang điểm | Cần entity `RubricCriterion` từ P1, và `Question` đã có từ P2 để gắn vào |
| **P4 — Bloom level & luồng duyệt** | UI gắn `BloomLevel`, quản lý `QuestionStatus` | Gắn nhãn Bloom (nhớ/hiểu/vận dụng/phân tích) khi tạo câu hỏi; màn hình giảng viên duyệt/sửa/loại câu hỏi (Draft → Approved/Rejected) | Cần CRUD câu hỏi từ P2 đã có sẵn để thao tác trên đó |
| **P5 — AI sinh câu hỏi** | Upload tài liệu + gọi AI sinh câu hỏi | Giảng viên upload giáo trình/slide, gọi AI (LLM) sinh câu hỏi tự động, lưu vào bảng `Question` với `Source = AIGenerated`, `Status = Draft` để P4 duyệt | Cần entity `Question` từ P1; có thể làm bản đơn giản (không cần RAG thật ngay) song song với các phần khác |

**Thứ tự chạy thực tế:** P1 đẩy entity + migration lên sớm nhất (không cần chờ 100% xong mới cho người khác bắt đầu — P2/P3/P4/P5 có thể viết Controller/View song song ngay khi P1 push entity lên nhánh chung). P3 và P4 phụ thuộc nhẹ vào P2 (cần có sẵn `Question` để gắn rubric/duyệt), nên P2 nên ưu tiên hoàn thành phần `Create`/`Index` trước để không chặn người khác.

**Nhóm 2-7 còn lại** (quản lý kỳ thi, AI interview core, chấm điểm AI, giám sát, báo cáo, quản trị hệ thống) để dành cho **asg2** (thêm 1 luồng nữa) và **group project cuối kỳ** (bắt buộc phải có Nhóm 3 – Lõi phỏng vấn AI), chưa làm trong asg1.

## 7. Hướng dẫn chạy dự án

### Yêu cầu môi trường
- .NET 8 SDK
- `dotnet-ef` tool: `dotnet tool install --global dotnet-ef` (nếu chưa có)
- Quyền truy cập database (xin connection string từ leader nhóm, hoặc tự chạy SQL Server bằng Docker ở máy mình)

### Bước 1 — Cấu hình `appsettings.json`
File `asg1/appsettings.json` chứa connection string thật nên bị `.gitignore`, **không có sẵn khi clone repo về**. Cần tự tạo:
```bash
cp asg1/appsettings.example.json asg1/appsettings.json
```
Sau đó mở `asg1/appsettings.json`, điền connection string vào `ConnectionStrings.DefaultConnection`.

**Cách A — Dùng chung SQL Server của nhóm (khuyến khích, khỏi cài gì thêm):**
```
Server=<địa-chỉ-server>,1433;Database=Asignment1;User Id=<user>;Password=<password>;TrustServerCertificate=True;
```
Xin địa chỉ server, user và password từ leader nhóm. **Không ghi thông tin thật vào README hay commit lên git** (repo này để public).

**Cách B — Tự chạy SQL Server local bằng Docker (nếu server chung sập hoặc muốn test riêng):**
```bash
docker compose up -d
```
Container chạy ở `localhost:1433`, user `sa`, password xem trong `docker-compose.yml`. Sau đó có thể chạy `db/create-app-user.sql` để tạo user riêng giới hạn quyền cho đúng chuẩn bảo mật.

### Bước 2 — Tạo bảng trong database (migration)
```bash
dotnet ef database update --project asg1.DAL --startup-project asg1
```
Migration nằm trong `asg1.DAL` nên `--project` phải trỏ vào `asg1.DAL` (còn `--startup-project` là `asg1`). Nếu trỏ `--project asg1` sẽ báo lỗi "target project doesn't match your migrations assembly".

User dùng để connect cần có quyền `db_ddladmin` trở lên để tạo bảng lúc migrate. Nếu gặp lỗi `CREATE TABLE permission denied`, nhờ người quản lý DB chạy giúp:
```sql
USE Asignment1;
GO
ALTER ROLE db_ddladmin ADD MEMBER asg_1;
GO
```

### Bước 3 — Chạy ứng dụng
```bash
dotnet run --project asg1
```
Mặc định chạy ở `http://localhost:5163` (xem `asg1/Properties/launchSettings.json` nếu muốn đổi port).

### Cấu hình Gemini (chỉ cần cho tính năng AI sinh câu hỏi)
Trang `/AiQuestions` cần API key của Google Gemini (lấy từ leader nhóm, hoặc tự tạo ở Google AI Studio). Điền vào `asg1/appsettings.json` (file đã bị `.gitignore` nên key không bị đẩy lên git):
```json
"Gemini": {
  "ApiKey": "<gemini-api-key>",
  "Model": "gemini-3.1-flash-lite",
  "BaseUrl": "https://generativelanguage.googleapis.com/v1beta"
}
```
Hoặc đặt biến môi trường `Gemini__ApiKey` thay cho việc lưu key trong file. Nếu chưa có key, trang này sẽ báo "Chưa cấu hình Gemini API key", các chức năng khác vẫn chạy bình thường.

### Khi thêm entity mới (Question, RubricCriterion, ...)
Sau khi thêm entity + khai báo `DbSet` mới trong `AppDbContext`, tạo migration mới rồi update lại DB:
```bash
dotnet ef migrations add <TenMigration> --project asg1.DAL --startup-project asg1
dotnet ef database update --project asg1.DAL --startup-project asg1
```
