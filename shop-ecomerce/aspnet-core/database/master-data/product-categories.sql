INSERT INTO [dbo].[AppProductCategories]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[SortOrder]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[ParentId]
           ,[SeoMetaDescription]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (newid()
           ,N'Nem truyền thống'
           ,'C1'
           ,'nem-truyen-thong'
           ,1
           ,null
           ,1
           ,1
           ,null
           ,N'Danh mục nem truyền thống'
           ,null
           ,null
           ,getdate()
           ,null)


		   