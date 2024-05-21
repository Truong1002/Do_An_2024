BEGIN TRANSACTION;
GO

EXEC sp_rename N'[AppProducts].[Visiblity]', N'Visibility', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240422171814_UpdateVisibility', N'8.0.0');
GO

COMMIT;
GO

