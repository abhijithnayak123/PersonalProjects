--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <09/21/2018>
-- Description:	 Create a new table for Master Features.
-- ================================================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tFeatures]') AND type in (N'U'))
BEGIN
	DROP TABLE [dbo].[tFeatures]
END 

CREATE TABLE [dbo].[tFeatures]
(
	[FeatureID] [int] NOT NULL,
	[Name][varchar](100) NULL,
	[IsEnable] BIT DEFAULT 1,
	[DTServerCreate] [datetime] NOT NULL,
	[DTServerLastModified] [datetime] NULL,
	[DTTerminalCreate] [datetime] NOT NULL,
	[DTTerminalLastModified] [datetime] NULL
CONSTRAINT [PK_tFeatures] PRIMARY KEY CLUSTERED 
(
    [FeatureID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF NOT EXISTS(SELECT 1 FROM tFeatures)
BEGIN
	INSERT INTO dbo.tFeatures
	(
		FeatureID,Name,DTServerCreate,DTServerLastModified,DTTerminalCreate,DTTerminalLastModified
	)
	VALUES
	(
		1,
	   N'Card Purchase',
		GETDATE(),
		NULL,
		GETDATE(),
		NULL
	),
	(
		2,
	   N'Card Load/Unload',
		GETDATE(),
		NULL,
		GETDATE(),
		NULL
	),
	(
		3,
	   N'Western Union',
		GETDATE(),
		NULL,
		GETDATE(),
		NULL
	)

END
GO