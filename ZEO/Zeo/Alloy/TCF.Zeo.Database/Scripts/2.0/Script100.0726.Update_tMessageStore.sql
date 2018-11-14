--- ===============================================================================
-- Author     :	 Abhijith
-- Description:  Changing the Contact Number of the TCF to point it to IT Service Desk.
-- Creatd Date:  02-Apr-2018
-- Story Id   :  B-13192
-- ================================================================================

IF EXISTS(SELECT 1 FROM tMessageStore WHERE AddlDetails LIKE  '%1-866-TCF-DESK (1-866-823-3375)%')
BEGIN
	UPDATE 
		tMessageStore 
	SET 
		ContactType = 'ITServiceDesk', AddlDetails = REPLACE(AddlDetails, '1-866-TCF-DESK (1-866-823-3375)', '{0}')
	WHERE
		AddlDetails LIKE  '%1-866-TCF-DESK (1-866-823-3375)%'
END
GO





