-- Author: Swarnalakshmi Subramani
-- Date Created: Jan 08 2015
-- Description: Adding new table to persists customer Fee Adjustments
-- User Story ID: US1800 Task ID: 
--======================================================================

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tCustomerFeeAdjustments_tChannelPartnerFeeAdjustments]') AND parent_object_id = OBJECT_ID(N'[dbo].[tCustomerFeeAdjustments]'))
ALTER TABLE [dbo].[tCustomerFeeAdjustments] DROP CONSTRAINT [FK_tCustomerFeeAdjustments_tChannelPartnerFeeAdjustments]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCustomerFeeAdjustments]') AND type in (N'U'))
DROP TABLE [dbo].[tCustomerFeeAdjustments]
GO

CREATE TABLE [dbo].[tCustomerFeeAdjustments](
	[rowguid] [uniqueidentifier] NOT NULL,
	[ID] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[FeeAdjustmentPK] [uniqueidentifier] NOT NULL,
	[CustomerID] [bigint] NOT NULL,
	[IsAvailed] [bit] NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tCustomerFeeAdjustments] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tCustomerFeeAdjustments]  WITH CHECK ADD  CONSTRAINT [FK_tCustomerFeeAdjustments_tChannelPartnerFeeAdjustments] FOREIGN KEY([FeeAdjustmentPK])
REFERENCES [dbo].[tChannelPartnerFeeAdjustments] ([rowguid])
GO

ALTER TABLE [dbo].[tCustomerFeeAdjustments] CHECK CONSTRAINT [FK_tCustomerFeeAdjustments_tChannelPartnerFeeAdjustments]
GO




