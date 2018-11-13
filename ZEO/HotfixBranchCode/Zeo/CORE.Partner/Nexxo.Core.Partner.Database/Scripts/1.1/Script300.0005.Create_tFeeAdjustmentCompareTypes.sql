CREATE TABLE [dbo].[tFeeAdjustmentCompareTypes](
	[Id] [tinyint] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_tFeeAdjustmentCompareTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


insert tFeeAdjustmentCompareTypes
values (1, 'equal'),
(2, 'not equal'),
(3, 'in'),
(4, 'not in'),
(5, 'greater than'),
(6, 'less than'),
(7, 'greater than / equal'),
(8, 'less than / equal')
GO