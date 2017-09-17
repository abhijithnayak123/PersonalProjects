--- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <07/10/2017>
-- Description: Handling additional details for the customer.
-- ================================================================================

DELETE FROM tMessageStore WHERE MessageKey IN ('1001.602.1111')

INSERT INTO dbo.tMessageStore
(
    MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails
)
VALUES
(
    N'1001.602.1111',1,N'0',N'Retail customer already exists', GETDATE(),N'RCIF',2,
	CONCAT (N'Cannot create new customer in ZEO, customer profile already exists in RCIF.',CHAR(13) + CHAR(10),
	'Confirm SSN/ITIN or DOB keyed is correct. If incorrect, search TCF Customer in ZEO with correct information.',CHAR(13) + CHAR(10),
	'If keyed correctly, search customer in RCIF by SSN/ITIN or Name.',CHAR(13) + CHAR(10),
	'Find customer record in RCIF and locate account number.',CHAR(13) + CHAR(10),
	'Search ZEO by TCF account number and continue with registration.',CHAR(13) + CHAR(10),
	'NOTE: If updates are needed to the RCIF profile, refer to the ZEO procedures on “Updating the Customer Profile”.')
)
