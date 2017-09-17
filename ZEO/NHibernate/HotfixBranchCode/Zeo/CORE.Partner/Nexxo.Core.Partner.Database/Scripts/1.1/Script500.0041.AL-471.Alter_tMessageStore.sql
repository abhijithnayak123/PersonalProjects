-- Create date: 25/06/2015
-- Description:	<As a product owner,  I need to allow WU to determine which fields are required >
-- Rally ID:	<AL-471>
--===========================================================================================
--
IF NOT EXISTS( SELECT 1 from TMessageStore where MessageKey = '1005.6008' and PartnerPK = 1)
BEGIN
INSERT INTO [tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], 
			[Language], [Content], [DTCreate], [DTLastMod], [AddlDetails], [Processor])
 VALUES    (NEWID(), '1005.6008', 1, 0, 'This transaction requires SSN/ITIN. Add SSN/ITIN to the customer''s profile and then resend transaction', 
           GETDATE(), Null, 'Please contact the System Administrator', '')
END
GO

IF NOT EXISTS( SELECT 1 from TMessageStore where MessageKey = '1004.6008' and PartnerPK = 1)
BEGIN   
INSERT INTO [tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], 
			[Language], [Content], [DTCreate], [DTLastMod], [AddlDetails], [Processor])
 VALUES    (NEWID(), '1004.6008', 1, 0, 'This transaction requires SSN/ITIN. Add SSN/ITIN to the customer''s profile and then resend transaction', 
           GETDATE(), Null, 'Please contact the System Administrator', '')

END
GO

IF NOT EXISTS( SELECT 1 from TMessageStore where MessageKey = '1005.5050' and PartnerPK = 1)
BEGIN   
INSERT INTO [tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], 
			[Language], [Content], [DTCreate], [DTLastMod], [AddlDetails], [Processor])
 VALUES    (NEWID(), '1005.5050', 1, 0, 'This transaction requires SSN/ITIN. Add SSN/ITIN to the customer''s profile and then resend transaction', 
           GETDATE(), Null, 'Please contact the System Administrator', '')

END
GO

IF NOT EXISTS( SELECT 1 from TMessageStore where MessageKey = '1004.5050' and PartnerPK = 1)
BEGIN   
INSERT INTO [tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], 
			[Language], [Content], [DTCreate], [DTLastMod], [AddlDetails], [Processor])
 VALUES    (NEWID(), '1004.5050', 1, 0, 'This transaction requires SSN/ITIN. Add SSN/ITIN to the customer''s profile and then resend transaction', 
           GETDATE(), Null, 'Please contact the System Administrator', '')

END
GO

IF NOT EXISTS( SELECT 1 from TMessageStore where MessageKey = '1005.7490' and PartnerPK = 1)
BEGIN   
INSERT INTO [tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], 
			[Language], [Content], [DTCreate], [DTLastMod], [AddlDetails], [Processor])
 VALUES    (NEWID(), '1005.7490', 1, 0, 'This transaction requires SSN/ITIN. Add SSN/ITIN to the customer''s profile and then resend transaction', 
           GETDATE(), Null, 'Please contact the System Administrator', '')

END
GO

IF NOT EXISTS( SELECT 1 from TMessageStore where MessageKey = '1004.7490' and PartnerPK = 1)
BEGIN   
INSERT INTO [tMessageStore]([MessageStorePK], [MessageKey], [PartnerPK], 
			[Language], [Content], [DTCreate], [DTLastMod], [AddlDetails], [Processor])
 VALUES    (NEWID(), '1004.7490', 1, 0, 'This transaction requires SSN/ITIN. Add SSN/ITIN to the customer''s profile and then resend transaction', 
           GETDATE(), Null, 'Please contact the System Administrator', '')

END
GO
--===========================================================================================
