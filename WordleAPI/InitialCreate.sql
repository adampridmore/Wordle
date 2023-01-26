IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Teams] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Teams] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Games] (
    [Id] uniqueidentifier NOT NULL,
    [TeamId] uniqueidentifier NOT NULL,
    [Word] nvarchar(5) NOT NULL,
    [State] int NOT NULL,
    [Guess1] nvarchar(5) NULL,
    [Guess2] nvarchar(5) NULL,
    [Guess3] nvarchar(5) NULL,
    [Guess4] nvarchar(5) NULL,
    [Guess5] nvarchar(5) NULL,
    [Guess6] nvarchar(5) NULL,
    CONSTRAINT [PK_Games] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Games_Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [Teams] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Games_TeamId] ON [Games] ([TeamId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230126221931_InitialCreate', N'7.0.2');
GO

COMMIT;
GO

