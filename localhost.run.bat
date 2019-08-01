start /d "." dotnet run -p .\src\WebShop.Auth.Api\WebShop.Auth.Api.csproj --server.urls https://0.0.0.0:5001
start /d "." dotnet run -p .\src\WebShop.Users.Api\WebShop.Users.Api.csproj --server.urls https://0.0.0.0:5002
start /d "." dotnet run -p .\src\WebShop.Web.Mvc\WebShop.Web.Mvc.csproj --server.urls https://0.0.0.0:5004
start /d "." dotnet run -p .\src\WebShop.Email.Api\WebShop.Email.Api.csproj --server.urls https://0.0.0.0:5010