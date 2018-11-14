--- ===============================================================================
-- Author:		 M.Purna Pushkal
-- Description: To get the receipt template
-- ================================================================================

IF OBJECT_ID(N'usp_GetReceiptTemplate', N'P') IS NOT NULL
	DROP PROC usp_GetReceiptTemplate
GO

CREATE PROCEDURE usp_GetReceiptTemplate	
	@receipt XML
AS
BEGIN
	BEGIN TRY

		SELECT 
			[Table].[Column].value('Name[1]', 'NVARCHAR(255)') AS 'Name',
			[Table].[Column].value('Order[1]', 'INT') AS 'Order'
		INTO 
			#TempReceipts
		FROM 
			@receipt.nodes('/DocumentElement/Receipt') AS [Table]([Column])

		SELECT TOP 1 
		  tr.TemplateName,
		  tr.ReceiptData
		FROM 
		  #TempReceipts tt
		INNER JOIN tReceipts tr 
		  ON tr.TemplateName = tt.Name
		ORDER BY 
		  tt.[Order] ASC

		IF OBJECT_ID('#TempReceipts') IS NOT NULL
		BEGIN
			DROP TABLE TempReceipts
		END		
	END TRY

	BEGIN CATCH
		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	END CATCH
END
GO
