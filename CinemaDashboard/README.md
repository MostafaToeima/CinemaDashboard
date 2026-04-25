# CinemaDashboard

ASP.NET Core MVC Cinema Dashboard + Customer Area.

## Run steps

1. Open `CinemaDashboard/CinemaDashboard.csproj` in Visual Studio.
2. Make sure SQL Server is running.
3. Open Package Manager Console and run:

```powershell
Add-Migration init
Update-Database
```

4. Run the project.

Default route: `/Admin/Home/Index`  
Customer route: `/Customer/Home/Index`

## Notes

- Uses direct `ApplicationDbContext` in controllers.
- No Identity/Auth.
- No Repository pattern.
- No async/await.
- Admin layout was adapted from the uploaded Falcon/e-commerce template into a clean MVC Razor layout.
