BEGIN TRANSACTION;
GO

ALTER TABLE [Games] ADD [DateStarted] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230127221911_DateStarted', N'7.0.2');
GO

COMMIT;
GO

