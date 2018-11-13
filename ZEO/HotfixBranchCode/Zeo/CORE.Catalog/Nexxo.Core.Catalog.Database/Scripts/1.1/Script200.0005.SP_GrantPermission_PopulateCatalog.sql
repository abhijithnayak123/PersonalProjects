IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PopulateCatalog]') AND type in (N'P', N'PC'))
	GRANT EXECUTE ON [dbo].[PopulateCatalog] TO [DMSWebSvc]
GO