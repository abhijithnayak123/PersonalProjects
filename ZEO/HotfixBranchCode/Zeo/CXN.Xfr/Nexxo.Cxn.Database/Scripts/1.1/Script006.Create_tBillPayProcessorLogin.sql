/****** Object:  Table [dbo].[tBillPayTransactionMapping]    Script Date: 05/14/2013 17:31:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS(SELECT * FROM sys.objects where object_id = OBJECT_ID('tBillPayTransactionMapping') and TYPE in ('U'))
BEGIN
CREATE TABLE [dbo].[tBillPayTransactionMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BillPayId] [bigint] NOT NULL,
	[ProcessorReferenceId] [varchar](50) NOT NULL,
	[processorId] [int] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
 CONSTRAINT [PK_tBillPayTransactionMapping_ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF
END


