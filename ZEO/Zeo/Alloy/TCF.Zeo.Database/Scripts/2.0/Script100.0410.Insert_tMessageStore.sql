-- =================================================
-- Author:		Kaushik Sakala		
-- Create date: 09/03/2017	
-- Description:	Script to insert into tMessageStore
-- =================================================

INSERT dbo.tMessageStore
(
    MessageKey,
    ChannelPartnerId,
    Language,
    Content,
    DTServerCreate,
    AddlDetails,
    Processor,
    Type
)
VALUES
(
    N'1003.100.8101', 
    1, 
    N'0',
    N'Zeo Prepaid Visa Debit Card cannot be issued.', 
    '2017-03-10 17:34:24', 
    N'Zeo Prepaid Visa Debit Card cannot be associated with this customer profile. This customer''s previous card was closed on account of fraud.', 
    N'Zeo', 
    2 
)