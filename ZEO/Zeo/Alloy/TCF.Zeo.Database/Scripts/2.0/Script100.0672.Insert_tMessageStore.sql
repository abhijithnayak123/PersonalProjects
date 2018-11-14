--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <05-28-2018>
-- Description: Teller Inquiry Error Handling.
-- ================================================================================


DELETE FROM tMessageStore WHERE MessageKey IN ('1002.100.2024','1002.100.2025','1002.100.2026','1002.100.2027','1002.100.2028',
	'1002.100.2029','1002.100.2030','1002.100.2031','1002.100.2032','1002.100.2033')

INSERT INTO dbo.tMessageStore
(
    MessageKey,ChannelPartnerId,Language,Content,DTServerCreate,Processor,Type,AddlDetails
)
VALUES
(
   N'1002.100.2024',
	1,
	N'0',
    N'Certificate Not Found for Teller Inquiry Call.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information'
),
(
   N'1002.100.2025',
	1,
	N'0',
    N'Teller Inquiry Call Failed.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information'
),
(
   N'1002.100.2026',
	1,
	N'0',
    N'Teller Inquiry Failed - Non Sufficient Fund to Process Teller Inquiry.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information'
),
(
   N'1002.100.2027',
	1,
	N'0',
    N'Teller Inquiry Failed - Account has Stops.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information'
),
(
   N'1002.100.2028',
	1,
	N'0',
    N'Teller Inquiry Failed - Account has Cautions.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information'
),
(
   N'1002.100.2029',
	1,
	N'0',
    N'Teller Inquiry Failed - Flagged for No Posting.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information'
),
(
   N'1002.100.2030',
	1,
	N'0',
    N'Teller Inquiry Failed - Flagged for No Debits.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information'
),
(
   N'1002.100.2031',
	1,
	N'0',
    N'Teller Inquiry Failed - No Account.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information'
),
(
   N'1002.100.2032',
	1,
	N'0',
    N'Teller Inquiry Failed - Account is Closed.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information'
),
(
   N'1002.100.2033',
	1,
	N'0',
    N'Teller Inquiry Failed - Account is Dormant.',
	GETDATE(),
	N'ZEO',
	2,
	N'Transaction could not be completed. Please contact your technical support team for more information'
)
GO