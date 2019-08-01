dotnet ef migrations add ApplicationDbContextMigration -c ApplicationDbContext -o Migrations --project WebShop.Users.Data
dotnet ef database update --project WebShop.Users.Data