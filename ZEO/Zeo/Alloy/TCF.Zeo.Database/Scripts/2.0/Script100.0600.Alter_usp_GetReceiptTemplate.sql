--- ===============================================================================
-- Author:		 M.Purna Pushkal
-- Description: To get the receipt template
-- Create date: <19-08-2017>
-- ================================================================================

IF OBJECT_ID(N'usp_GetReceiptTemplate', N'P') IS NOT NULL
	DROP PROC usp_GetReceiptTemplate
GO

CREATE PROCEDURE usp_GetReceiptTemplate	
	@receipt XML
AS
BEGIN
	BEGIN TRY

	DECLARE @xmlTable TABLE
	(
		  Name NVARCHAR(255),
		  [Order] INT
	)

	INSERT 
	INTO 	@xmlTable (Name,[Order])
	(
		SELECT 
		  'Name' = T.C.value('Name[1]', 'NVARCHAR(255)'),           
        'Order' = T.C.value('Order[1]', 'INT')              
      FROM 
		  @receipt.nodes('/DocumentElement/Receipt') AS T(C)
	)	

		SELECT TOP 1 
		  tr.TemplateName,
		  tr.ReceiptData
		FROM 
		  @xmlTable tt
		INNER JOIN tReceipts tr WITH (NOLOCK)
		  ON tr.TemplateName = tt.Name
		ORDER BY 
		  tt.[Order] ASC
			
	END TRY

	BEGIN CATCH
		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	END CATCH
END
GO
