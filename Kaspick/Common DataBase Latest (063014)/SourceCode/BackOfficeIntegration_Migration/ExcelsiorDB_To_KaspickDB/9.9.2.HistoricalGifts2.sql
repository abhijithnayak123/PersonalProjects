DECLARE @SecurityTypeName VARCHAR(20), @FundingTypeIDName VARCHAR(20), @IsRentRoyalAgreementsName VARCHAR(40)
		,@FundingTypeListTypeID INT, @SecurityTypeListTypeID INT, @IsRentRoyalAgreementsListTypeID INT
SELECT @SecurityTypeName = 'Security Type'
		,@IsRentRoyalAgreementsName = 'IsRentRoyalAgreements'
		--,@FundingTypeIDName = 'Funding Type ID'
			
/* Insert the list type*/
/* Check security type list*/
IF EXISTS(SELECT 1 FROM TBL_ListType WHERE ListTypeName = @SecurityTypeName)
BEGIN
	IF EXISTS(SELECT 1 FROM TBL_ListItem WHERE ListTypeID = (SELECT ListTypeID FROM TBL_ListType WHERE ListTypeName = @SecurityTypeName))
	BEGIN
		DELETE FROM TBL_ListItem WHERE ListTypeID = (SELECT ListTypeID FROM TBL_ListType WHERE ListTypeName = @SecurityTypeName)
	END
	DELETE FROM TBL_ListType WHERE ListTypeName = @SecurityTypeName
END
INSERT INTO TBL_ListType (ListTypeName, IvanTableName, IvanFieldLength, [Description], Keycode, IsMutable, 
							ModifiedDate, ModifiedUserID, CreatedDate, CreatedUserID, DeletedUserID)
SELECT @SecurityTypeName, NULL, 20, @SecurityTypeName, 'SECTYP', 1, GETDATE(), 1, GETDATE(), 1, NULL

--/* Check funding type id list*/
--IF EXISTS(SELECT 1 FROM TBL_ListType WHERE ListTypeName = @FundingTypeIDName)
--BEGIN
--	IF EXISTS(SELECT 1 FROM TBL_ListItem WHERE ListTypeID = (SELECT ListTypeID FROM TBL_ListType WHERE ListTypeName = @FundingTypeIDName))
--	BEGIN
--		DELETE FROM TBL_ListItem WHERE ListTypeID = (SELECT ListTypeID FROM TBL_ListType WHERE ListTypeName = @FundingTypeIDName)
--	END
--	DELETE FROM TBL_ListType WHERE ListTypeName = @FundingTypeIDName
--END
--INSERT INTO TBL_ListType (ListTypeName, IvanTableName, IvanFieldLength, [Description], Keycode, IsMutable, 
--							ModifiedDate, ModifiedUserID, CreatedDate, CreatedUserID, DeletedUserID)
--SELECT @FundingTypeIDName, NULL, 20, @FundingTypeIDName, 'FNDTYPID', 1, GETDATE(), 1, GETDATE(), 1, NULL


--/* Check rent royal agreement list*/
--IF EXISTS(SELECT 1 FROM TBL_ListType WHERE ListTypeName = @IsRentRoyalAgreementsName)
--BEGIN
--	IF EXISTS(SELECT 1 FROM TBL_ListItem WHERE ListTypeID = (SELECT ListTypeID FROM TBL_ListType WHERE ListTypeName = @IsRentRoyalAgreementsName))
--	BEGIN
--		DELETE FROM TBL_ListItem WHERE ListTypeID = (SELECT ListTypeID FROM TBL_ListType WHERE ListTypeName = @IsRentRoyalAgreementsName)
--	END
--	DELETE FROM TBL_ListType WHERE ListTypeName = @IsRentRoyalAgreementsName
--END
--INSERT INTO TBL_ListType (ListTypeName, IvanTableName, IvanFieldLength, [Description], Keycode, IsMutable, 
--							ModifiedDate, ModifiedUserID, CreatedDate, CreatedUserID, DeletedUserID)
--SELECT @IsRentRoyalAgreementsName, NULL, 20, @IsRentRoyalAgreementsName, 'ISRNTRY', 1, GETDATE(), 1, GETDATE(), 1, NULL


--/* Get the ListTypeID */
--SELECT @FundingTypeListTypeID = ListTypeID
--FROM TBL_ListType WHERE ListTypeName = @FundingTypeIDName

SELECT @SecurityTypeListTypeID = ListTypeID
FROM TBL_ListType WHERE ListTypeName = @SecurityTypeName

--SELECT @IsRentRoyalAgreementsListTypeID = ListTypeID
--FROM TBL_ListType WHERE ListTypeName = @IsRentRoyalAgreementsName


/* Insert the individual list items */
INSERT INTO TBL_ListItem (ListTypeID, ListItemName, IvanValue, Abbrev, DisplaySequence, IsMutable, ModifiedDate, ModifiedUserID, CreatedDate, CreatedUserID, DeletedUserID, CustomFlag)
-- DECLARE @SecurityTypeListTypeID INT
SELECT @SecurityTypeListTypeID, 'Equities', 'Equities', 'EQ', 1, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
UNION SELECT @SecurityTypeListTypeID, 'Fixed Income', 'Fixed Income', 'FI', 2, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
UNION SELECT @SecurityTypeListTypeID, 'Cash', 'Cash', 'CSH', 3, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
UNION SELECT @SecurityTypeListTypeID, 'Cash Equivalents', 'Cash Equivalents', 'CSHEQ', 4, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
UNION SELECT @SecurityTypeListTypeID, 'Other Assets', 'Other Assets', 'OTAS', 5, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
UNION SELECT @SecurityTypeListTypeID, 'Mineral Interests', 'Mineral Interests', 'MININT', 6, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
UNION SELECT @SecurityTypeListTypeID, 'Real Estate', 'Real Estate', 'RLEST', 7, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
UNION SELECT @SecurityTypeListTypeID, 'Mutual Funds', 'Mutual Funds', 'MF', 8, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
UNION SELECT @SecurityTypeListTypeID, 'Common Trust Funds', 'Common Trust Funds', 'CTF', 9, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
UNION SELECT @SecurityTypeListTypeID, 'Unknown', 'Unknown', 'UKN', 10, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
 

--INSERT INTO TBL_ListItem (ListTypeID, ListItemName, IvanValue, Abbrev, DisplaySequence, IsMutable, ModifiedDate, ModifiedUserID, CreatedDate, CreatedUserID, DeletedUserID, CustomFlag)
--SELECT @FundingTypeListTypeID, 'Addition', 'Addition', 'ADD', 1, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
--UNION SELECT @FundingTypeListTypeID, 'Initial', 'Initial', 'INTL', 2, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
--UNION SELECT @FundingTypeListTypeID, 'InitialTestamentary', 'InitialTestamentary', 'INTSTM', 3, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL


--INSERT INTO TBL_ListItem (ListTypeID, ListItemName, IvanValue, Abbrev, DisplaySequence, IsMutable, ModifiedDate, ModifiedUserID, CreatedDate, CreatedUserID, DeletedUserID, CustomFlag)
--SELECT @IsRentRoyalAgreementsListTypeID, 'Yes', 'Yes', 'YES', 1, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL
--UNION SELECT @IsRentRoyalAgreementsListTypeID, 'No', 'No', 'NO', 2, 1, GETDATE(), 1, GETDATE(), 1, NULL, NULL


--SELECT * FROM TBL_ListType WHERE ListTypeName = @IsRentRoyalAgreementsName
--SELECT * FROM TBL_ListItem WHERE ListTypeID = (SELECT ListTypeID FROM TBL_ListType WHERE ListTypeName = @IsRentRoyalAgreementsName)