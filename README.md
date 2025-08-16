# BPIBankSystem.API (Backend)

Hệ thống Internet Banking backend bằng ASP.NET Core (.NET 8), cung cấp RESTful API cho các chức năng ngân hàng như chuyển tiền, quản lý tài khoản, séc, sao kê, hỗ trợ, đổi địa chỉ và chặn thanh toán.

## Features
- Chuyển tiền nội bộ/liên ngân hàng với OTP 2 bước và tham chiếu giao dịch
- Giao dịch an toàn bằng SQL Transaction (Serializable) và rollback toàn phần khi lỗi
- Quản lý tài khoản, lịch sử giao dịch, sao kê
- Yêu cầu séc, chặn thanh toán, đổi địa chỉ, hỗ trợ người dùng
- Xác thực/ủy quyền JWT, phân quyền cơ bản

## Technologies
- .NET 8, ASP.NET Core Web API
- Entity Framework Core, SQL Server
- AutoMapper, IMemoryCache, Logging (ILogger)

## Architecture
- Layered: Controllers → Services → Data (EF Core `AppDbContext`)
- DTOs/Entities tách biệt, mapping qua `Mapper/MappingProfile.cs`
- Migration trong thư mục `Migrations/`

## Project Structure (rút gọn)
- `Controllers/` API endpoints (ví dụ: `TransfersController.cs`)
- `Services/Impl/` Nghiệp vụ (ví dụ: `TransferService.cs`)
- `Data/` DbContext, `CommonResponse`
- `Entities/` Bảng dữ liệu: `Account`, `Transaction`, `TransferRequest`, ...
- `DTOs/` Request/Response models

## Local Setup
1. Yêu cầu: .NET 8 SDK, SQL Server
2. Cấu hình kết nối DB trong `appsettings.Development.json` (ConnectionStrings:DefaultConnection)
3. Áp dụng migration và tạo DB:
```bash
DOTNET_CLI_HOME=. dotnet ef database update --project BPIBankSystem.API
```
4. Chạy API:
```bash
dotnet run --project BPIBankSystem.API
```
API mặc định chạy tại `https://localhost:7065` hoặc `http://localhost:5065` (theo `Properties/launchSettings.json`).

## Env & Config
- `appsettings.json`/`appsettings.Development.json`: ConnectionStrings, JWT, email (nếu sử dụng)
- Đặt secret nhạy cảm qua User Secrets hoặc biến môi trường khi chạy thực tế

## Transfer Flow (2 bước với OTP)
- `POST /api/transfers/send-otp` gửi OTP nếu đúng mật khẩu giao dịch
- `POST /api/transfers/confirm-transfer` xác nhận OTP và thực hiện chuyển khoản

Nghiệp vụ chính trong `Services/Impl/TransferService.cs`:
- Xác thực OTP bằng `IMemoryCache`
- Thực hiện chuyển tiền trong transaction SQL:
  - Dùng `BeginTransactionAsync(IsolationLevel.Serializable)`
  - Ghi `TransferRequest`, điều chỉnh số dư `fromAccount`/`toAccount`
  - Ghi `Transaction` với `Reference`
  - `Commit` nếu thành công, `Rollback` nếu lỗi → đảm bảo tính toàn vẹn (atomicity)

## Migrations
- Tạo migration mới:
```bash
dotnet ef migrations add <Name> --project BPIBankSystem.API
```
- Cập nhật DB:
```bash
dotnet ef database update --project BPIBankSystem.API
```

## Testing (gợi ý)
- Unit/Integration tests cho: xác thực OTP, đủ/thiếu số dư, rollback khi lỗi
- Kiểm thử song song (concurrency) và idempotency cho endpoint chuyển tiền

## Notes
- Định dạng tiền: `DECIMAL(18,2)` cho `Account.Balance`, `Transaction.Amount`
- Có thể bổ sung khóa lạc quan (rowversion) cho `Account` để tăng an toàn khi cạnh tranh ghi
- Logging lỗi và audit trail để hỗ trợ vận hành/truy vết

