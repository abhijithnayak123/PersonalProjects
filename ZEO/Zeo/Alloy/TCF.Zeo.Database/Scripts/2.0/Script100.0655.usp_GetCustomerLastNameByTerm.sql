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

CREATE PROCEDURE usp_GetCustomerLastNameByTerm
(
	 @term NVARCHAR(500)
)
AS
BEGIN
       BEGIN TRY
           SELECT DISTINCT(LastName) FROM tCustomers 
                 WHERE
                  LastName LIKE @term + '%' AND IsRCIFSuccess = 1 and ProfileStatus = 1
              
       END TRY
       BEGIN CATCH           
              -- Execute error retrieval routine.  
              EXECUTE usp_CreateErrorInfo;  
       END CATCH
END
GO


