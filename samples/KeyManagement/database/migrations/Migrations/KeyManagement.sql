IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [DataProtectionKeys] (
    [Id] int NOT NULL IDENTITY,
    [Created] datetime2 NOT NULL,
    [Name] nvarchar(200) NULL,
    [Value] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_DataProtectionKeys] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [SigningKeys] (
    [Id] int NOT NULL IDENTITY,
    [Created] datetime2 NOT NULL,
    [Name] nvarchar(200) NULL,
    [Value] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_SigningKeys] PRIMARY KEY ([Id])
);

GO

CREATE UNIQUE INDEX [IX_DataProtectionKeys_Name] ON [DataProtectionKeys] ([Name]) WHERE [Name] IS NOT NULL;

GO

CREATE UNIQUE INDEX [IX_SigningKeys_Name] ON [SigningKeys] ([Name]) WHERE [Name] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200327143521_KeyManagement', N'3.1.3');

GO

