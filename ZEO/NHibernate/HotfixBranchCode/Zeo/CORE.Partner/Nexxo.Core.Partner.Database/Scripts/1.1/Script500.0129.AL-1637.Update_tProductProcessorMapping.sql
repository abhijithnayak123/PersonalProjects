/****** Script for SelectTopNRows command from SSMS  ******/

-- ================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <11/20/2015>
-- Description:	<Update CardExpiryPeriod as 48 months for Visa>
-- Jira ID:		<AL-16377>
-- ================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProductProcessorsMapping'
		AND COLUMN_NAME = 'CardExpiryPeriod'
		)
BEGIN

    DECLARE @CardExpiryPeriod INT
	DECLARE @processorId UNIQUEIDENTIFIER

	SET @CardExpiryPeriod = 48
	SELECT @processorId = PP.ProcessorId  FROM tProductProcessorsMapping AS PP INNER JOIN 
    tProcessors P ON P.rowguid = PP.ProcessorId WHERE P.Name = 'VISA'

	UPDATE tProductProcessorsMapping 
	SET CardExpiryPeriod = @CardExpiryPeriod 
	WHERE ProcessorId = @processorId

END
GO