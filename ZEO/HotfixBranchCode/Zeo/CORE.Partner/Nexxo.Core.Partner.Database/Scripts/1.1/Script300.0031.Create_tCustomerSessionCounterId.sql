--===========================================================================================
-- Author:		<SwarnaLakshmi>
-- Create date: <10/27/2014>
-- Description:	<Synovus Customer session Counter ID Details>
-- Rally ID:	<US2028>
--===========================================================================================


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCustomerSessionCounterIdDetails]') AND type in (N'U'))
DROP TABLE [dbo].[tCustomerSessionCounterIdDetails]
GO

CREATE TABLE [dbo].[tCustomerSessionCounterIdDetails](
	[CustomerSessionId] [uniqueidentifier] NOT NULL,
	[CounterId] [nvarchar](50) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL
) ON [PRIMARY]

GO
