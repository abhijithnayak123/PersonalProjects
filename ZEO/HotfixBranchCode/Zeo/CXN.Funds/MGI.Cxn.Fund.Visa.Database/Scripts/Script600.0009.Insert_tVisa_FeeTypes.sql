-- ==========================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <11/27/2015>
-- Description:	<Insert Card Replacement Fee into tVisa_FeeTypes table>
-- Rally ID:	<AL-1639>
-- ==========================================================================
DECLARE @CardReplacementFee varchar(50) = 'Card Replacement Fee'
DECLARE @MailOrderFee  varchar(50) = 'Mail Order Fee'

IF NOT EXISTS (SELECT 1 FROM tVisa_FeeTypes WHERE Name = @CardReplacementFee)
BEGIN

	INSERT INTO tVisa_FeeTypes (VisaFeeTypePK, Name,DTServerCreate)
	VALUES ('69780075-09E3-4963-BFB8-61FD8FD134E2', @CardReplacementFee, GETDATE())

END

IF NOT EXISTS (SELECT 1 FROM tVisa_FeeTypes WHERE Name = @MailOrderFee)
BEGIN

	INSERT INTO tVisa_FeeTypes (VisaFeeTypePK, Name,DTServerCreate)
	VALUES ('476062D8-4981-4AC2-B546-2442E95CB256', @MailOrderFee, GETDATE())

END