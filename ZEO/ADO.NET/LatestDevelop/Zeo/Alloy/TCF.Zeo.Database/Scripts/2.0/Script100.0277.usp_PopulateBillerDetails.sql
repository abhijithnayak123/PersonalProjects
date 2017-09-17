--- ===============================================================================
-- Author:		 Kaushik Sakala
-- Description:	 Script to populate the WU Billers
-- Jira ID:		<AL-8998>
-- ================================================================================

IF OBJECT_ID(N'usp_PopulateBillerDetails', N'P') IS NOT NULL
	DROP PROC usp_PopulateBillerDetails
GO

CREATE PROCEDURE usp_PopulateBillerDetails		
@Billers xml
AS
BEGIN
 BEGIN TRY
	BEGIN TRANSACTION;	  
	SET NOCOUNT ON;
		
	IF OBJECT_ID('#TempBillers') IS NOT NULL
		drop table #TempBillers	
		
	SELECT
		[Table].[Column].value('CompanyName[1]', 'nvarchar(max)') as 'CompanyName',
		[Table].[Column].value('Country[1]', 'nvarchar(max)') as 'Country',
		[Table].[Column].value('CurrencyCode[1]', 'nvarchar(50)') as 'CurrencyCode',
		[Table].[Column].value('ISOCountryCode[1]', 'nvarchar(max)') as 'ISOCountryCode',
		[Table].[Column].value('ChannelPartnerId[1]', 'int') as 'ChannelPartnerId',
		[Table].[Column].value('DTServerCreate[1]', 'DATETIME') as 'DTServerCreate'
	into #TempBillers
	FROM @Billers.nodes('/DocumentElement/Billers') as [Table]([Column])

	----- Update all the billers in tWUnion_Catalog to false
	UPDATE tWUnion_Catalog SET IsActive = 0 
		
	----- Update only the existing billers to true 
	UPDATE tWUnion_Catalog
		SET DTServerLastModified = GETDATE() , IsActive = 1 
		FROM tWUnion_Catalog CTL  
		INNER JOIN #TempBillers Temp ON 
		Temp.CompanyName = CTL.CompanyName 
			
	--- insert new billers to the table. 
	INSERT INTO tWUnion_Catalog (CompanyName,ISOCountryCode,Country, CurrencyCode,IsActive,ChannelPartnerId,DTServerCreate)
		SELECT A.CompanyName, A.ISOCountryCode,A.Country,A.CurrencyCode, 1 AS ISACTIVE,A.ChannelPartnerId,A.DTServerCreate
		FROM 
		(SELECT DISTINCT 
			Temp.CompanyName, Temp.ISOCountryCode,Temp.Country,Temp.CurrencyCode,Temp.ChannelPartnerId,Temp.DTServerCreate 
			FROM 
			#TempBillers Temp 
			WHERE 
			NOT EXISTS (SELECT CompanyName FROM tWUnion_Catalog  CTL WHERE Temp.CompanyName = CTL.CompanyName)				   
		)A
				  					
	  COMMIT TRANSACTION;	          
  END TRY
  BEGIN CATCH

    IF @@TRANCOUNT > 0 
    BEGIN   	  
        ROLLBACK TRANSACTION 		
    END

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

  END CATCH
END
GO
