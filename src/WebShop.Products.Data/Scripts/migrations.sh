dotnet ef migrations add ProductsDbContextMigration -c ProductsDbContext -o Migrations --project WebShop.Products.Data
dotnet ef database update --project WebShop.Products.Data