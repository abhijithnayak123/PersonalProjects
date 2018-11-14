--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <12-26-2017>
-- Description:	Wildcard search.
-- ================================================================================


IF OBJECT_ID(N'usp_GetCustomerLastNameByTerm', N'P') IS NOT NULL
BEGIN
     DROP PROCEDURE  usp_GetCustomerLastNameByTerm
END
GO


