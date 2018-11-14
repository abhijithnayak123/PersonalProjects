-- Author:         Nitish Biradar 
-- Description:     WU Message update - Answer Question 
-- Story Id   :  B-16729 
-- ================================================================================ 

IF EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '1005.100.8301') 
BEGIN 

    UPDATE  
        tMessageStore 
    SET 
        Content = 'Form Incomplete', 
        AddlDetails = 'Please answer the question - Is the customer sending this money on behalf of another person or entity?', 
        DTServerLastModified = GETDATE() 
    WHERE  
        MessageKey = '1005.100.8301' 
END 
GO 

IF NOT EXISTS(SELECT 1 FROM tMessageStore WHERE MessageKey LIKE  '%1003.103.23606%') 
BEGIN 
    INSERT INTO dbo.tMessageStore 
    ( 
        MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails,ContactType 
    ) 
    VALUES 
    ( 
       N'1003.103.23606', 
        1, 
        N'0', 
        N'Card needs a balance to cover fees for card re-order.', 
        GETDATE(), 
        N'Visa', 
        2, 
        N'Once card is loaded, please contact RBS -or- have customer contact Visa to order replacement card.', 
        NULL 
    ) 

END 

GO 

