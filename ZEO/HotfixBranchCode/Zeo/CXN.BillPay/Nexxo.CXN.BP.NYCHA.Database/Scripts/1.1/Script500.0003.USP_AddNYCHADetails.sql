--===========================================================================================
-- Author:		<Nitika Saini>
-- Created date: <Apr 21st 2015>
-- Description:	<Script to alter stored procedure USP_AddNYCHADetails>           
-- Jira ID:	<AL-244>
--===========================================================================================

/****** Object:  StoredProcedure [dbo].[USP_AddNYCHADetails]    Script Date: 4/7/2015 3:14:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--create [USP_AddNYCHADetails] to import Tenent details from CSV file 
ALTER PROCEDURE [dbo].[USP_AddNYCHADetails]
		@recdts xml,
		@result int out
	AS
	BEGIN
		BEGIN TRANSACTION;
	  
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
		
		IF EXISTS (select * from sys.objects where name = '#TempNYCHAFileTable' and type = 'u')
		drop table #TempNYCHAFileTable	
		
		Select NYCHAFile.Items.value('data(TenantId[1])','nvarchar(20)') as Tenantid,
			   NYCHAFile.Items.value('data(AccountNumber[1])','nvarchar(max)') as AccountNumber,	
			   NYCHAFile.Items.value('data(NychaFileid[1])','int') as 	NychaFileID,
			   NYCHAFile.Items.value('data(NychaFilename[1])','nvarchar(500)') as NychaFilename		      
			into #TempNYCHAFileTable
			from @recdts.nodes('/ArrayOfNYCHAFile/NYCHAFile') AS NYCHAFile(Items)
		
		Update NyT Set NyT.DTLastMod = GETDATE()
		From  dbo.tNYCHATenant NyT
		Inner join #TempNYCHAFileTable TempNycha 
		on NyT.TenantID = TempNycha.TenantID and 
		NyT.AccountNumber = TempNycha.AccountNumber
		
		DELETE TempNycha
		FROM #TempNYCHAFileTable TempNycha
		INNER JOIN dbo.tNYCHATenant NyT on NyT.TenantID = TempNycha.TenantID and 
		NyT.AccountNumber = TempNycha.AccountNumber	
		
		Declare @NychaFilename nvarchar(500)
		Declare @NychaFileID int
		set @NychaFilename = (select distinct NychaFilename from #TempNYCHAFileTable)
		set @NychaFileID = (select distinct NychaFileid from #TempNYCHAFileTable)
		
		INSERT into tNYCHATenant(TenantID,AccountNumber,NYCHAFilePK,Active,DTCreate)
		SELECT  Tenantid,AccountNumber,NychaFileID,1,GETDATE() from #TempNYCHAFileTable	
			
		Update tNYCHAFiles set Status = 1 where NYCHAFileName = @NychaFilename and NYCHAFilePK=@NychaFileID

		IF @@ERROR > 0
		Begin
			ROLLBACK TRANSACTION;
			set @result = 0
		End 
		ELSE 
		Begin 
		 If @@TRANCOUNT > 0
			BEGIN
			COMMIT TRANSACTION;
			set @result = 1
			END
		END 
	END
GO


