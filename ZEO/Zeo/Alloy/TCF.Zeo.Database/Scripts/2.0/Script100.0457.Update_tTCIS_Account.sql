-- ================================================================================
-- Author:		<Abhijith>
-- Create date: <04/03/2017>
-- Description:	Updating the CustomerId in tTCISAccount table.
-- ================================================================================

UPDATE t 
SET t.CustomerID = pc.CXEId, t.CustomerSessionID = cs.CustomerSessionID
FROM
	tTCIS_Account t
		INNER JOIN tAccounts a on t.TCISAccountID = a.CXNId AND a.ProviderId = 602
		INNER JOIN tPartnerCustomers pc on pc.CustomerPK = a.CustomerPK 
		LEFT JOIN tCustomerSessions cs on pc.CustomerPK = cs.CustomerPK 
			AND cs.CustomerSessionPK =
				(
					SELECT  TOP 1 cts.CustomerSessionPK
					FROM    tCustomerSessions cts
					WHERE   cts.CustomerPK  = cs.CustomerPK
					ORDER BY cts.DtServerCreate DESC
				)
GO


UPDATE t
SET t.CustomerRevisionNo = isnull(cr.revisionno,1)
FROM 
	tTCIS_Account t
		INNER JOIN 
		   (    
			 SELECT 
			   CustomerId,
			   max(RevisionNo) RevisionNo
			 FROM
			   tCustomers_Aud 
			 GROUP BY
			   CustomerId	
		   ) AS cr 
		   ON 
			cr.CustomerId = t.CustomerId 
GO

