/****** Script for SelectTopNRows command from SSMS  ******/

-- ================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <11/20/2015>
-- Description:	<Update CardExpiryPeriod as 48 months for TCF Visa>
-- Jira ID:		<AL-1637>
-- ================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerProductProcessorsMapping'
		AND COLUMN_NAME = 'CardExpiryPeriod'
		)
BEGIN

    DECLARE @CardExpiryPeriod INT
	DECLARE @productProcessorId UNIQUEIDENTIFIER
	DECLARE @tcfChannelPartnerPk UNIQUEIDENTIFIER

	SET @CardExpiryPeriod = 48
	SELECT @productProcessorId = PP.rowguid  FROM tProductProcessorsMapping AS PP INNER JOIN 
    tProcessors P ON P.rowguid = PP.ProcessorId WHERE P.Name = 'VISA'

	SELECT @tcfChannelPartnerPk = ChannelPartnerPK FROM tChannelPartners WHERE Name = 'TCF'

	UPDATE tChannelPartnerProductProcessorsMapping 
	SET CardExpiryPeriod = @CardExpiryPeriod 
	WHERE ChannelPartnerId = @tcfChannelPartnerPk and ProductProcessorId = @productProcessorId

END
GO