BEGIN TRANSACTION;
GO

CREATE TABLE [AppIdentitySettings] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [Prefix] nvarchar(max) NOT NULL,
    [CurrentNumber] int NOT NULL,
    [StepNumber] int NOT NULL,
    CONSTRAINT [PK_AppIdentitySettings] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240427031310_AddIdentitySettingTable', N'8.0.0');
GO

COMMIT;
GO

