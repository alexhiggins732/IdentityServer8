rmdir /S /Q Migrations

dotnet ef database drop

dotnet ef migrations add Users -o Migrations/UsersDb
dotnet ef migrations script -o Migrations/UsersDb.sql

dotnet ef database update
