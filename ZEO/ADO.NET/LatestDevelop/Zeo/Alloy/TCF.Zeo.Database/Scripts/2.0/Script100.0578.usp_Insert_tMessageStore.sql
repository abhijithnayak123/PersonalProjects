--- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <07/07/2017>
-- Description: Handling additional details for the customer.
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey IN ('1001.602.1111')

INSERT INTO dbo.tMessageStore
(
    MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,AddlDetails,Processor,Type
)
VALUES
(
    N'1001.602.1111',1,N'0',N'Retail customer already exists', GETDATE(),N'Contact RCIF for registering this customer',N'RCIF',2 
)
