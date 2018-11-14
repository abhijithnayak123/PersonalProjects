--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <23-10-2017>
-- Description: Update decline messages in 'tMessageStore' table.
-- ================================================================================


UPDATE 
    tMessageStore
SET 
    DisplayMessage = Content  
WHERE
	Messagekey LIKE '%1002.200.%' 
	AND Messagekey
	NOT IN('1002.200.5003','1002.200.5002','1002.200.5001','1002.200.5000') 
	AND ChannelPartnerId IN (34,1)

GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'Cannot be resubmitted. Do not cash.' 
WHERE
	Messagekey IN (	
				   '1002.200.1',
				   '1002.200.5',
				   '1002.200.18',
				   '1002.200.19',
				   '1002.200.24',
				   '1002.200.25',
				   '1002.200.30',
				   '1002.200.31',
				   '1002.200.33',
				   '1002.200.34',
	               '1002.200.53',
				   '1002.200.54',
				   '1002.200.56',
				   '1002.200.57',
				   '1002.200.58',
				   '1002.200.59',
				   '1002.200.60'
				   )
	
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'Resubmit if you have more info. Additional info needed from customer'  
WHERE
	Messagekey IN ('1002.200.40')

GO



UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'Return check to customer- All payees are not present'
WHERE
	Messagekey IN ('1002.200.42')

GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'LATESHIFT - cannot approve until normal business hours when we can contact maker'
WHERE
	Messagekey IN ('1002.200.49')

GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'Return check to customer - maker temporarily unavailable. Try again in one hour.'
WHERE
	Messagekey IN ('1002.200.45')

GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'Cannot be resubmitted. The check is from out of the country'
WHERE
	Messagekey IN ('1002.200.35')
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'Cannot be resubmitted. This is a credit card check'
WHERE
	Messagekey IN ('1002.200.29')
GO




UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'Cannnot be resubmitted. This is a starter check'
WHERE
	Messagekey IN ('1002.200.20')

GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'Return check to customer- Postdated check cannot be cashed today'
WHERE
	Messagekey IN ('1002.200.10')

GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'Resubmit if you have more info-Duplicate Check. Call 770 587 0001'
WHERE
	Messagekey IN ('1002.200.7')

GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'Cannot be resubmitted. Check is too old to cash'
WHERE
	Messagekey IN ('1002.200.6')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'Return check to customer - per teller the customer withdrew the transaction'
WHERE
	Messagekey IN ('1002.200.3')

GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = N'Resubmit if you have more info- Unable to contact maker'
WHERE
	Messagekey IN ('1002.200.2')

GO


