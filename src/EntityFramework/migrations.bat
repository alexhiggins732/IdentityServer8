cd host
rmdir /S /Q Migrations

dotnet ef migrations add Grants -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add Config -c ConfigurationDbContext -o Migrations/IdentityServer/ConfigurationDb
dotnet ef migrations script -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb.sql
dotnet ef migrations script -c ConfigurationDbContext -o Migrations/IdentityServer/ConfigurationDb.sql

cd ..
