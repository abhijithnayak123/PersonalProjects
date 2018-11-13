-- =============================================================================================
-- Author:		<Mohammed Khaja Firoz Molla>
-- Create date: <07/03/2015>
-- Description:	<Script for set up Certegy as check processor>
-- Jira ID:	<AL-438>
-- ==============================================================================================

IF NOT EXISTS (SELECT [Name] FROM [dbo].[tProcessors] WHERE [Name] = 'Certegy')
BEGIN
	INSERT INTO tProcessors(rowguid, Name, DTServerCreate) VALUES (NewID(), 'Certegy', GETDATE())
END
GO

DECLARE @CertegyPK UNIQUEIDENTIFIER
SELECT @CertegyPK = rowguid from [tProcessors] Where Name = 'Certegy'

DECLARE @CheckProductPK UNIQUEIDENTIFIER
SELECT @CheckProductPK = rowguid from [tProducts] Where Name = 'ProcessCheck'

IF NOT EXISTS (SELECT 1 FROM [dbo].tProductProcessorsMapping WHERE [ProductId] = @CheckProductPK AND [ProcessorId] = @CertegyPK)
BEGIN
	INSERT INTO tProductProcessorsMapping (rowguid, ProductId, ProcessorId, Code, DTServerCreate, IsSSNRequired, IsSWBRequired, CanParkReceiveMoney, ReceiptCopies, ReceiptReprintCopies) 
		VALUES(NEWID(), @CheckProductPK, @CertegyPK, 201, GETDATE(), 1, 0, 0, 1, 1)
END
GO

