--- ===================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <11/23/2015>
-- Description:	<As Alloy, I need a minimum age for customers to transact in the system for Synovus>
-- Jira ID:		<AL-1629>
-- =====================================================================================



IF EXISTS 
        (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerProductProcessorsMapping' AND COLUMN_NAME = 'MinimumTransactAge'		  
		)
BEGIN
     
    
	DECLARE @synovusChannelPartnerPK UNIQUEIDENTIFIER
	DECLARE @processorId UNIQUEIDENTIFIER

	select @SynovusChannelPartnerPK = ChannelPartnerPK from tChannelPartners WHERE NAME = 'Synovus'		
	
	SELECT @processorId = PP.ProcessorId  FROM tProductProcessorsMapping AS PP INNER JOIN 
    tProcessors P ON P.rowguid = PP.ProcessorId WHERE P.Name = 'TSys'

	-- Update Minimum Age as '18' for Process check, Money Order, Send MOney, Bill Pay and Receive Money  

	UPDATE CP set CP.MinimumTransactAge = 18
    FROM tChannelPartnerProductProcessorsMapping AS CP
	INNER JOIN tProductProcessorsMapping AS PP
	ON CP.ProductProcessorId = PP.rowguid
	WHERE ChannelPartnerId = @SynovusChannelPartnerPK AND PP.ProcessorId != @processorId

	-- Update Minimum Age as '13 for Tsys 

	UPDATE CP set CP.MinimumTransactAge = 13
    FROM tChannelPartnerProductProcessorsMapping AS CP
	INNER JOIN tProductProcessorsMapping AS PP
	ON CP.ProductProcessorId = PP.rowguid
	WHERE ChannelPartnerId = @SynovusChannelPartnerPK AND PP.ProcessorId = @processorId

END
GO





