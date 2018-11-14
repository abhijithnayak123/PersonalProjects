--- ===============================================================================
-- Author     :	 Nitish Biradar
-- Description:  Contact information displayed in messages
-- Creatd Date:  27-03-2018
-- Story Id   :  B-13192
-- ================================================================================


IF EXISTS(SELECT 1 FROM tMessageStore WHERE AddlDetails LIKE  '%1-800-TCF-DESK(823-3375)%')
BEGIN
	UPDATE 
		tMessageStore 
	SET 
		ContactType = 'ITServiceDesk'
	WHERE
		AddlDetails LIKE  '%1-800-TCF-DESK(823-3375)%'
END

IF EXISTS(SELECT 1 FROM tMessageStore WHERE AddlDetails LIKE '%1-800-325-6000%')
BEGIN
	UPDATE 
		tMessageStore 
	SET 
		ContactType = 'WU'
	WHERE
		AddlDetails LIKE '%1-800-325-6000%'
END

IF EXISTS(SELECT 1 FROM tMessageStore WHERE AddlDetails LIKE '%855-477-1135%')
BEGIN
	UPDATE 
		tMessageStore 
	SET 
		ContactType = 'VISA'
	WHERE
		AddlDetails LIKE '%855-477-1135%'
END

IF EXISTS(SELECT 1 FROM tMessageStore WHERE AddlDetails LIKE '%763-337-7881%')
BEGIN
	UPDATE 
		tMessageStore 
	SET 
		ContactType = 'BSA'
	WHERE
		AddlDetails LIKE '%763-337-7881%'
END

IF EXISTS(SELECT 1 FROM tMessageStore WHERE AddlDetails LIKE  '%1-800-TCF-DESK(823-3375)%')
BEGIN
	UPDATE 
		tMessageStore 
	SET 
		AddlDetails = REPLACE(AddlDetails, '1-800-TCF-DESK(823-3375)', '{0}')
	WHERE
		AddlDetails LIKE  '%1-800-TCF-DESK(823-3375)%'
END

IF EXISTS(SELECT 1 FROM tMessageStore WHERE AddlDetails LIKE '%1-800-325-6000%')
BEGIN
	UPDATE 
		tMessageStore 
	SET 
		AddlDetails = REPLACE(AddlDetails, '1-800-325-6000', '{0}')
	WHERE
		AddlDetails LIKE '%1-800-325-6000%'
END

IF EXISTS(SELECT 1 FROM tMessageStore WHERE AddlDetails LIKE '%855-477-1135%')
BEGIN
	UPDATE 
		tMessageStore 
	SET 
		AddlDetails = REPLACE(AddlDetails, '855-477-1135', '{0}')
	WHERE
		AddlDetails LIKE '%855-477-1135%'
END

IF EXISTS(SELECT 1 FROM tMessageStore WHERE AddlDetails LIKE '%763-337-7881%')
BEGIN
	UPDATE 
		tMessageStore 
	SET 
		AddlDetails = REPLACE(AddlDetails, '763-337-7881', '{0}')
	WHERE
		AddlDetails LIKE '%763-337-7881%'
END








