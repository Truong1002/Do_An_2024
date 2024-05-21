INSERT INTO [dbo].[AppManufacturers]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[Country]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (newid()
           ,N'Liên Tuấn'
           ,'M1'
           ,'lientuan'
           ,null
           ,1
           ,1
           ,N'Bắc Ninh'
           ,null
           ,null
           ,getdate()
           ,null)

		   INSERT INTO [dbo].[AppManufacturers]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[Country]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (newid()
           ,N'Cổ truyền'
           ,'M2'
           ,'cotruyen'
           ,null
           ,1
           ,1
           ,N'Bắc Giang'
           ,null
           ,null
           ,getdate()
           ,null)