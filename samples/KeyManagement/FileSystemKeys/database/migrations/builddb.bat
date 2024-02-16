dotnet ef database drop -c KeyManagementDbContext

rmdir /S /Q Migrations

dotnet ef migrations add KeyManagement -c KeyManagementDbContext -o Migrations/KeyManagement
dotnet ef migrations script -c KeyManagementDbContext -o Migrations/KeyManagement.sql
dotnet ef database update -c KeyManagementDbContext
