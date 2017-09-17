
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_InsertIdentificationConfirmation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_InsertIdentificationConfirmation]
GO

CREATE PROC [dbo].[usp_InsertIdentificationConfirmation]
(
 @P_AgentId [nvarchar] (100),
 @P_CustomerSessionId [nvarchar] (100)
 --@dateIdentified datetime
 )
AS
BEGIN
    SET NOCOUNT ON

    BEGIN TRY
     INSERT INTO tIdentificationConfirmation(AgentID, CustomerSessionID, DateIdentified)
            SELECT @P_AgentId, @P_CustomerSessionId, getdate()

            SELECT 'Success';
            END TRY

    BEGIN CATCH
        SELECT ERROR_MESSAGE();
    END CATCH
END
GO