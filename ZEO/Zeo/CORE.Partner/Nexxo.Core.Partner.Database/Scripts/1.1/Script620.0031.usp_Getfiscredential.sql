-- =============================================
-- Author:	Karun
-- Create date: 06-Jun-2016
-- Description:	SP to get fis credential from database based on channel partnerid and bank id
-- =============================================

IF EXISTS (
	SELECT  1 
	FROM sys.objects	
	WHERE NAME = 'usp_getfiscredential'
)
BEGIN
	DROP PROCEDURE usp_getfiscredential
END
GO

CREATE PROCEDURE [dbo].[usp_getfiscredential](@ChannelPartnerId INT,@BankId varchar(10))
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;    
	SELECT 
		UserName,
		Password,
		ApplicationKey,
		ChannelKey,
		ChannelPartnerId,
		BankId,
		MetBankNumber 
	FROM 
		tFIS_Credential WITH (NOLOCK) 
	WHERE 
		ChannelPartnerId = @ChannelPartnerId 
		AND BankId = @BankId
END
GO