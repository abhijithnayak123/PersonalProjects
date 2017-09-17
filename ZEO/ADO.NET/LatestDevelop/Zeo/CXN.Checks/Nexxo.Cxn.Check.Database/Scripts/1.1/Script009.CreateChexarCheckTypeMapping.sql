﻿CREATE TABLE [dbo].[tChxr_CheckTypeMapping](
	[ChexarType] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CheckType] [int] NULL,
 CONSTRAINT [PK_tChxr_CheckTypeMapping] PRIMARY KEY CLUSTERED 
(
	[ChexarType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

insert tChxr_CheckTypeMapping(ChexarType, Name, CheckType)
values (18, 'Check - Cashier''s/Official', 1),
(19, 'Check - Two Party', 10),
(21, 'Check - Payroll Handwritten', 6),
(22, 'Check - Money Order', 5),
(23, 'Check - Payroll Printed', 7),
(24, 'Check - Insurance/Attorney', 12),
(25, 'Check - Tax Refund -   U.S. Treasury', 8),
(26, 'Check - Govt -  U.S. Treasury', 2),
(27, 'Check - Govt State - Out of State', 4),
(41, 'Check - Free / Gratis', 0),
(42, 'Check - WU Fee', 0),
(108, 'Check - Promo Printed Payroll', 13),
(390, 'Check - Misc Check', 10),
(420, 'Check - Govt -  U.S. Treasury Recurring', 2),
(421, 'Check - Loan / RAL', 14),
(423, 'Check - Two Party Business', 11),
(424, 'Check - Govt - All Other', 3),
(425, 'Check - Govt - All Other  Recurring', 3),
(426, 'Check - Tax Refund - All Other', 9),
(444, 'Check - On Us', 0),
(452, 'Check - Loan', 15),
(455, 'Check - Unknown Check Type', 16),
(460, 'Check - Loan', 15),
(465, 'Check - RAC', 17),
(30001, 'Check - Woodforest', 18)
go
