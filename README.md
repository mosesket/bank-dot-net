#README.md

dotnet aspnet-codegenerator --version
dotnet tool uninstall -g dotnet-aspnet-codegenerator

dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet restore
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet aspnet-codegenerator controller -name WalletController -api -outDir Controllers

dotnet new webapi -n UserService
dotnet new webapi -n WalletService

dotnet tool install --global dotnet-ef

dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Pomelo.EntityFrameworkCore.MySql
<!-- dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL -->
dotnet add package Microsoft.EntityFrameworkCore.Design

--version 8.0.2
 --version 9.0.0


dotnet ef migrations add InitialCreate
dotnet ef database update
