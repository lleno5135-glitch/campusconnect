# Campus Connect — Advanced B.Sc IT Final-Year Project

A student campus community portal built with ASP.NET Core MVC (C#), ASP.NET Core Identity and SQL Server.

## Included modules
- Student registration, login and profile
- Community posts, search, likes and owner/admin delete
- Study resource uploads and downloads (PDF/DOC/DOCX/PPT/PPTX, 10 MB max)
- Campus events and student registration
- Project team finder with skill matching
- Lost & Found reports
- Admin dashboard and announcements
- Optional local recommendation engine based on profile interests and skills (no API key required)

## Requirements
- .NET 10 SDK
- Visual Studio 2026 (ASP.NET and web development workload) or VS Code
- SQL Server LocalDB (installed with Visual Studio) or SQL Server Express

## Run on Windows (PowerShell)
1. Extract the ZIP to a folder such as `C:\CampusConnect`.
2. Open PowerShell in that folder.
3. Check the SDK:
   ```powershell
   dotnet --version
   ```
   It should display `10.0.xxx`.
4. Restore packages:
   ```powershell
   dotnet restore
   ```
5. Run:
   ```powershell
   dotnet run
   ```
6. Open the HTTPS or HTTP localhost address printed in the terminal. Accept the local development certificate if prompted.

The first run creates `CampusConnectDb` in SQL Server LocalDB automatically.

## Demo administrator
- Email: `admin@campusconnect.local`
- Password: `Campus@123`

Sign in with this account to add events and announcements. Change this password before any shared/demo deployment. The seeded account is for local demonstration only.

## Student registration
Use **Join campus** on the home page. Newly registered accounts can use the student-facing modules after signing in. The seeded Admin account is the only administrator by default.

## If LocalDB is not available
Edit `ConnectionStrings:DefaultConnection` in `appsettings.json`. Example SQL Server Express:
```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=CampusConnectDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```
Make sure SQL Server Express is installed and running.

## Useful commands
```powershell
dotnet clean
dotnet restore
dotnet build
dotnet run
```

## Important notes before deployment
- This starter uses `EnsureCreated()` for beginner-friendly first-run database creation. For a production project, replace it with EF Core migrations (`dotnet ef migrations add InitialCreate`, then `dotnet ef database update`).
- Configure a production SQL Server connection string using environment variables or secret storage. Never commit passwords or API keys.
- Use HTTPS, change the demo admin password, configure email confirmation/password recovery, validate uploaded files with malware scanning, and use cloud/object storage for production uploads.
- The recommendation engine is deliberately rule-based and local, not a generative AI model. An external AI provider can be added later behind `IRecommendationService`.
- Bootstrap and Google Fonts are loaded from CDNs; internet access is needed for those external assets. The app's core MVC pages still render without them, but styling may be reduced.
