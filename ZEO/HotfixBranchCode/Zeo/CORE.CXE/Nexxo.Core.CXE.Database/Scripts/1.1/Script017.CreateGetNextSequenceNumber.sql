/****** Object:  StoredProcedure [dbo].[GetNextSequenceNumber]    Script Date: 05/03/2013 10:11:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
