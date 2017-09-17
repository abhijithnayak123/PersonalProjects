IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCheckTypes]') AND type in (N'U'))
DROP TABLE [dbo].[tCheckTypes]
GO

CREATE TABLE [dbo].[tCheckTypes](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tCheckTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

insert tCheckTypes (id, Name)
values (1, 'Cashier'),
(2, 'Govt - US Treasury'),
(3, 'Govt - US Other'),
(4, 'Govt - State'),
(5, 'Money Order'),
(6, 'Payroll Handwritten'),
(7, 'Payroll Printed'),
(8, 'Tax Refund - US Treasury'),
(9, 'Tax Refund - Other'),
(10, 'Two Party'),
(11, 'Two Party Business'),
(12, 'Insurance/Attorney'),
(13, 'Promo Printed Payroll'),
(14, 'Loan / RAL'),
(15, 'Loan'),
(16, 'Unknown Check Type'),
(17, 'HRB RAC'),
(18, 'Woodforest')
