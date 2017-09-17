-- ============================================================
-- Author:		Adwait Ullal
-- Create date: <01/09/2015>
-- Description:	<Create a new table tChannelPartner_X9_Parameters, 
--				to persist X9 search parameters 
--				for a channel partner>
-- Rally ID:	<US1685>
-- ============================================================


/****** Object:  Table [dbo].[tChannelPartner_X9_Parameters]    Script Date: 12/17/2014 3:27:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tChannelPartner_X9_Parameters]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tChannelPartner_X9_Parameters]
(
	[X9ParameterID]				[int]					IDENTITY(1,1)	NOT NULL,
	[ChannelPartnerID]			[uniqueidentifier]						NOT NULL,
	[X9Type]					[varchar](20)							NOT NULL,
	[Pattern]					[varchar](100)							NOT NULL,
	CONSTRAINT [PK_tChannelPartner_X9_Parameters] PRIMARY KEY CLUSTERED 
	(
		[X9ParameterID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [CK_tChannelPartner_X9_Parameters_X9Type_Value]
		CHECK (X9Type = 'On Us' OR X9Type = 'Money Order'),
	CONSTRAINT [CK_tChannelPartner_X9_Parameters_ChannelPartnerID_tChannelPartner_RowGuid]
		FOREIGN KEY (ChannelPartnerID)
			REFERENCES tChannelPartners(RowGuid)

) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO