--- ================================================================================
-- Author:		<Nitish Biradar>
-- Create date: <06/30/2016>
-- Description:	Add Error/Exception handling Framework in Transaction History
-- Jira ID:		<AL-7331>
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey like '1000.100.49%' or MessageKey like '1000.100.39%'
GO


INSERT INTO [dbo].[tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], [Language], [Content], [AddlDetails], [DTServerCreate], [Processor], [Type])
VALUES

(NEWID(),  '1000.100.4900', 1 , 0, 'Error occurred while getting customer transaction history', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(),'MGiAlloy', 2 ),
(NEWID(),  '1000.100.4901', 1 , 0, 'Error occurred while getting agent transaction history', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(),'MGiAlloy', 2 ),
(NEWID(),  '1000.100.4902', 1 , 0, 'Error occurred while getting transaction history', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(),'MGiAlloy', 2 ),
(NEWID(),  '1000.100.4903', 1 , 0, 'Error occurred while getting past transaction history', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(),'MGiAlloy', 2 ),
(NEWID(),  '1000.100.4904', 1 , 0, 'Error occurred while getting cash transaction', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(),'MGiAlloy', 2 ),
(NEWID(),  '1000.100.4905', 1 , 0, 'Error occurred while getting fund transaction ', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(),'MGiAlloy', 2 ),
(NEWID(),  '1000.100.4906', 1 , 0, 'Error occurred while getting check processing transaction ', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(),'MGiAlloy', 2 ),
(NEWID(),  '1000.100.4907', 1 , 0, 'Error occurred while getting moneytransfer transaction ', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(),'MGiAlloy', 2 ),
(NEWID(),  '1000.100.4908', 1 , 0, 'Error occurred while getting money order transaction', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(),'MGiAlloy', 2 ),
(NEWID(),  '1000.100.4909', 1 , 0, 'Error occurred while getting billpay transaction', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(),'MGiAlloy', 2 ),
(NEWID(),  '1000.100.3902', 1 , 0, 'Error occurred while getting transaction history', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(),'MGiAlloy', 2 ),
(NEWID(),  '1000.100.3903', 1 , 0, 'Error occurred while getting past transaction history', 'This action failed. Please retry and if problem persist contact your technical support team with this message', GETDATE(),'MGiAlloy', 2 )


GO