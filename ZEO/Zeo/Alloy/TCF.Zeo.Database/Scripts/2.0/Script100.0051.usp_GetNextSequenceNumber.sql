-- ================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <07/27/2015>
-- Description:	<As an engineer, I want to implement ADO.Net for Customer module>
-- Jira ID:		<AL-7630>
-- ================================================================================

IF  EXISTS (
	SELECT * 
	FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[dbo].[GetNextSequenceNumber]') AND type in (N'P', N'PC')
)
DROP PROCEDURE [dbo].[GetNextSequenceNumber]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetNextSequenceNumber] 
	@sequenceName nvarchar(25),
	@sequenceNumber bigint
AS
BEGIN
    BEGIN TRY

	UPDATE tSequences SET @sequenceNumber=seqNum=seqNum + 1 WHERE seqName = @sequenceName
	select @sequenceNumber as SequenceNumber

	END TRY
	BEGIN CATCH
		-- Execute error retrieval routine.  
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
GO


