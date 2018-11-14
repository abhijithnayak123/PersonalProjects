--- ===============================================================================
-- Author:		 M.Purna Pushkal
-- Description: To Upload the receipt template
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE NAME = 'usp_UploadReceiptTemplate')
BEGIN
	 DROP PROCEDURE usp_UploadReceiptTemplate
END
GO

CREATE PROCEDURE usp_UploadReceiptTemplate
(
	 @templateName NVARCHAR(100),
	 @receiptData VARBINARY(MAX),
	 @dtServerDate DATETIME
)
AS
BEGIN

IF EXISTS(SELECT 1 FROM tReceipts WHERE TemplateName = @templateName)
BEGIN
	 UPDATE tReceipts 
	 SET 
		  ReceiptData = @receiptData,
		  DtserverLastModified = @dtServerDate
	 WHERE 
		  TemplateName = @templateName
END
ELSE
BEGIN
	 INSERT INTO tReceipts
	 (
		  [TemplateName],
		  [ReceiptData],
		  [DtServerCreate]
	 )
	 VALUES
	 (
		  @templateName,
		  @receiptData,
		  @dtServerDate
	 )
END

END