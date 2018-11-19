SET NOCOUNT ON	

Delete from dbo.TBL_AccountLookup
Go

SET IDENTITY_INSERT TBL_AccountLookup ON 
INSERT dbo.TBL_AccountLookup (ManagerCode,ClientID,AllianceNumber,ProgramID,CustomerAccountNumber,AccountID)
SELECT 
                   C.BriefName AS ManagerCode
                  ,C.ClientID
                  
                  ,SUBSTRING(P.BriefName,1,15) AS AllianceNumber
                  ,P.ProgramID
                  
                  ,IT.CustomerAccountNumber 
                  ,DA.AccountID
FROM        $(InnotrustDB).dbo.AccountMaster IT

INNER JOIN $(ExcelsiorDB).dbo.DEFERREDGIFTACCOUNT DA
ON          IT.CustomerAccountNumber = DA.AdventID

INNER JOIN $(ExcelsiorDB).dbo.Program P
ON          DA.ProgramID = P.ProgramID

INNER JOIN $(ExcelsiorDB).dbo.Client C
ON          P.ClientID = C.ClientID

SET IDENTITY_INSERT TBL_AccountLookup OFF 
