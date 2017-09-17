--===========================================================================================
-- Auther:			Raviraja
-- Date Created:	1/3/2013
-- Description:		Script for Update Carver ChannelPartner to exclude YubiKey Authentication 
--===========================================================================================
UPDATE [dbo].[tChannelPartners]
SET [TIM]=2  WHERE [id]=28
GO
--===========================================================================================
