CREATE TABLE [dbo].[tFeeAdjustmentConditionTypes](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tFeeAdjustmentConditionTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

insert tFeeAdjustmentConditionTypes (Id, Name)
values 
(1,'Group'),
(2, 'Transaction Location'),
(3, 'Transaction Amount'),
(4, '# Transactions'),
(5, 'Check Type'),
(6, 'Registration Date'),
(7, '# Days Since Registration')
go

