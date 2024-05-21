BEGIN TRANSACTION;
GO

ALTER TABLE [AppProducts] ADD [CategoryName] nvarchar(250) NULL;
GO

ALTER TABLE [AppProducts] ADD [CategorySlug] nvarchar(250) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240423063504_AddCategoryFieldInProduct', N'8.0.0');
GO

COMMIT;
GO

