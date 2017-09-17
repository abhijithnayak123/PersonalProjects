-- ================================================================================
-- Author:          Abhijith
-- Create date:		05/24/2017
-- Description:		Get Sender Code from the WU response.
-- ================================================================================

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_GetSenderStateCode')
BEGIN
       DROP PROCEDURE usp_GetSenderStateCode
END
GO

CREATE PROCEDURE usp_GetSenderStateCode
(
       @wuTrxId               BIGINT      
       ,@senderStateListCode  VARCHAR(20)
)
AS
BEGIN
       BEGIN TRY
              
			DECLARE @originatingCountry VARCHAR(20) = 'UNITED STATES'
			DECLARE @originatingCountryCode VARCHAR(20) 
			DECLARE @senderState VARCHAR(100)
			DECLARE @senderStateOrCountryName VARCHAR(20) = 'UNITED STATES'
			
			SELECT 
			       @originatingCountryCode = OriginatingCountryCode 
			FROM 
				tWunion_Trx
			WHERE 
				WUTrxID = @wuTrxId
			
			IF @originatingCountryCode = 'US' OR @originatingCountryCode = 'USA'
			BEGIN
			       
				IF @senderStateListCode IS NOT NULL
				BEGIN
					SET @senderState =
					(
						SELECT LEFT(SUBSTRING(@senderStateListCode, (LEN(@senderStateListCode) - 2), 3),2)
					)
					
					SELECT 
						@senderStateOrCountryName = s.Name 
					FROM   
						tStates s
						INNER JOIN dbo.tMasterCountries mc ON s.MasterCountriesID = mc.MasterCountriesID
					WHERE 
					    mc.Name = @originatingCountry AND Abbr = @senderState
					
				END
			END
			ELSE 
			BEGIN
			       
				SELECT 
					@senderStateOrCountryName = Name 
				FROM 
					dbo.tMasterCountries tmc
				WHERE 
					Abbr2 = @originatingCountryCode
			END
			
			
			SELECT @senderStateOrCountryName AS 'SenderStateCode'           

       END TRY
       BEGIN CATCH

              EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

       END CATCH
END
GO
