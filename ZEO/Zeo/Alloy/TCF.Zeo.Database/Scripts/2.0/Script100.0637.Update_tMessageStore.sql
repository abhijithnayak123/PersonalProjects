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
    DisplayMessage = 'Cannot be resubmitted.  Do not cash.'  
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
	               '1002.200.44',
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
    DisplayMessage = 'Resubmit if you have more info-Need info from customer'  
WHERE
	Messagekey IN ('1002.200.40')

GO



UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer- All payees are not present'  
WHERE
	Messagekey IN ('1002.200.42')

GO



UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'LATESHIFT - cannot approve until normal business hours when we can contact maker.'  
WHERE
	Messagekey IN ('1002.200.49')

GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer - maker temporarily unavailable.  Try again in one hour.'  
WHERE
	Messagekey IN ('1002.200.45')

GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Cannot be resubmitted. The check is from out of the country'  
WHERE
	Messagekey IN ('1002.200.35')
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Cannot be resubmited. This is a credit card check'  
WHERE
	Messagekey IN ('1002.200.29')
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Cannot be resubmitted. This is a starter check'  
WHERE
	Messagekey IN ('1002.200.20')

GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer- Postdated check cannot be cashed today'  
WHERE
	Messagekey IN ('1002.200.10')

GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Resubmit if you have more info-Duplicate Check. Call 770 587 0001'  
WHERE
	Messagekey IN ('1002.200.7')

GO



UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer - per teller the customer withdrew the transaction'  
WHERE
	Messagekey IN ('1002.200.3')

GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Resubmit if you have more info- Unable to contact maker'  
WHERE
	Messagekey IN ('1002.200.2')

GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Cannot be resubmitted. Check is too old to cash'  
WHERE
	Messagekey IN ('1002.200.6')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'This is an InGo error code but effectively an "instant decline"'  
WHERE
	Messagekey IN ('1002.200.-1')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'This is an InGo error code but effectively an "instant decline"'  
WHERE
	Messagekey IN ('1002.200.-2')
	
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'This is an InGo error code but effectively an "instant decline"'  
WHERE
	Messagekey IN ('1002.200.-3')
	
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Please resubmit-Unable to use check image - missing or smeared'  
WHERE
	Messagekey IN ('1002.200.4')
	
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer-Check not signed by maker/purchaser'  
WHERE
	Messagekey IN ('1002.200.11')
	
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer- Check endorsed incorrectly'  
WHERE
	Messagekey IN ('1002.200.12')
	
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer -$amount and written amount do not match'  
WHERE
	Messagekey IN ('1002.200.13')
	
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer- Date field missing or incorrect'  
WHERE
	Messagekey IN ('1002.200.14')
	
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer - Check can only be cashed by intended payee'  
WHERE
	Messagekey IN ('1002.200.22')
	
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer- Maker and Payee have same address'  
WHERE
	Messagekey IN ('1002.200.23')
	
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer - check was submitted after hours'  
WHERE
	Messagekey IN ('1002.200.26')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer-Maker of check told us not to cash. Have customer contact maker'  
WHERE
	Messagekey IN ('1002.200.27')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer- Money Order cannot be cashed for 48 hours'  
WHERE
	Messagekey IN ('1002.200.32')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer- Maker and payee are same person'  
WHERE
	Messagekey IN ('1002.200.36')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Resubmit if you have more info - Duplicate check.  Call 229-276-3800 if you disagree'  
WHERE
	Messagekey IN ('1002.200.37')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Please resubmit - teller used the wrong membership'  
WHERE
	Messagekey IN ('1002.200.38')
	
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer - Bank needs to cancel their endorsement'  
WHERE
	Messagekey IN ('1002.200.41')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer- Maker will not verify check'  
WHERE
	Messagekey IN ('1002.200.43')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Resubmit if you have more info - Check is not filled out'  
WHERE
	Messagekey IN ('1002.200.46')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Resubmit if you have more info- We cannot verify makers business or identity'  
WHERE
	Messagekey IN ('1002.200.47')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer - per teller $$ not available in store to cash'  
WHERE
	Messagekey IN ('1002.200.48')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Return check to customer-  Check cashing limit exceeded for today'  
WHERE
	Messagekey IN ('1002.200.50')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Please resubmit - your check will be approved after customer endorses and you rescan'  
WHERE
	Messagekey IN ('1002.200.51')
	
GO


UPDATE 
    tMessageStore
SET 
    DisplayMessage = 'Resubmit if you have more info - Customers ID is not acceptable for check cashing'  
WHERE
	Messagekey IN ('1002.200.52')
	
GO

UPDATE 
    tMessageStore
SET 
    DisplayMessage = ''  
WHERE
	Messagekey IN ('1002.200.55')

GO

DELETE FROM 
  tMessageStore
WHERE 
  MessageKey 
	IN 
	( '1002.200.02',
	  '1002.200.03',
	  '1002.200.04',
	  '1002.200.05',
	  '1002.200.07')

GO