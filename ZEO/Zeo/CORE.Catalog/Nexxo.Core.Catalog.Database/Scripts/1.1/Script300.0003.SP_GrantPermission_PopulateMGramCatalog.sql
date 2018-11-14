IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_PopulateMGramCatalog]') AND type in (N'P', N'PC'))
	GRANT EXECUTE ON [dbo].[USP_PopulateMGramCatalog] TO [DMSWebSvc]
GO