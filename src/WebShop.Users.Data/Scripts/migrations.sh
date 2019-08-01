dotnet ef migrations add ApplicationDbContextMigration -c ApplicationDbContext -o Migrations/DbContext
dotnet ef database update