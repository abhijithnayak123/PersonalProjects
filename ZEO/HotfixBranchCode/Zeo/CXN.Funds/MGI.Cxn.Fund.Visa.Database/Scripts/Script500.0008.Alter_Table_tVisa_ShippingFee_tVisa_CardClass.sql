-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <22/09/2015>
-- Description:	<Rename DateTime Columns For tVisa_ShippingFee and tVisa_CardClass>
-- Jira ID:		<AL-823>
-- ================================================================================
IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tVisa_ShippingFee'
			AND COLUMN_NAME = 'DTServerLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tVisa_ShippingFee.DTServerLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO


IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tVisa_CardClass'
			AND COLUMN_NAME = 'DTServerLastMod'
		)
BEGIN
	EXEC sp_rename @objname = 'tVisa_CardClass.DTServerLastMod'
		,@newname = 'DTServerLastModified'
		,@objtype = 'COLUMN'
END
GO