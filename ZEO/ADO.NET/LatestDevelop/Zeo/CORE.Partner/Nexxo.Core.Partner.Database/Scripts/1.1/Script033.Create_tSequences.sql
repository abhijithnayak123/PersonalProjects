
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tSequences]') AND type in (N'U'))
DROP TABLE [dbo].[tSequences]
GO

CREATE TABLE [dbo].[tSequences](
	[seqName] [nvarchar](25) NOT NULL,
	[seqNum] [bigint] NOT NULL
) ON [PRIMARY]
GO

INSERT tSequences(seqName,seqNum) values('PAN',0)
GO

IF EXISTS 
(SELECT 1 FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[GetNextSequenceNumber]') AND TYPE in (N'P'))
	DROP PROCEDURE GetNextSequenceNumber;
GO

CREATE PROCEDURE [dbo].[GetNextSequenceNumber] 
	-- Add the parameters for the stored procedure here
	@sequenceName nvarchar(25),
	@sequenceNumber bigint OUTPUT
AS
BEGIN
	UPDATE tSequences SET @sequenceNumber=seqNum=seqNum + 1 WHERE seqName = @sequenceName
END
GO