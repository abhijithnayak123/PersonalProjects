	-- ============================================================
-- Author:		<Abhijith>
-- Create date: <12/09/2014>
-- Description:	<Script for updating the minimum fee and percent for TCF>
-- Rally ID:	<US2331>
-- ============================================================


UPDATE cpf
SET cpf.FeeMinimum= 1.00
FROM tChannelPartners cp
	INNER JOIN [tChannelPartnerFees_Check] cpf on cp.rowguid = cpf.ChannelPartnerPK
	INNER JOIN [tCheckTypes] ct on cpf.CheckType = ct.Id
WHERE cp.Name = 'TCF' AND ct.Name = 'Government'

GO

UPDATE cpf
SET cpf.FeeMinimum= 1.00
FROM tChannelPartners cp 
	INNER JOIN [tChannelPartnerFees_Check] cpf on cp.rowguid = cpf.ChannelPartnerPK
	INNER JOIN [tCheckTypes] ct on cpf.CheckType = ct.Id
WHERE  cp.Name = 'TCF' AND ct.Name = 'US Treasury'

GO

UPDATE cpf
SET cpf.FeeMinimum= 1.00
FROM tChannelPartners cp
	INNER JOIN [tChannelPartnerFees_Check] cpf on cp.rowguid = cpf.ChannelPartnerPK
	INNER JOIN [tCheckTypes] ct on cpf.CheckType = ct.Id
WHERE cp.Name = 'TCF' AND ct.Name = 'Printed Payroll'

GO

UPDATE cpf
SET cpf.FeeMinimum= 1.00
FROM tChannelPartners cp
	INNER JOIN [tChannelPartnerFees_Check] cpf on cp.rowguid = cpf.ChannelPartnerPK
	INNER JOIN [tCheckTypes] ct on cpf.CheckType = ct.Id
WHERE cp.Name = 'TCF' AND ct.Name = 'Handwritten Payroll'

GO

UPDATE cpf
SET cpf.FeeMinimum= 1.00
FROM tChannelPartners cp
	INNER JOIN [tChannelPartnerFees_Check] cpf on cp.rowguid = cpf.ChannelPartnerPK
	INNER JOIN [tCheckTypes] ct on cpf.CheckType = ct.Id
WHERE cp.Name = 'TCF' AND ct.Name = 'Ins/Attorney/Cashiers'

GO

UPDATE cpf
SET cpf.FeeMinimum= 1.00
FROM tChannelPartners cp
	INNER JOIN [tChannelPartnerFees_Check] cpf on cp.rowguid = cpf.ChannelPartnerPK
	INNER JOIN [tCheckTypes] ct on cpf.CheckType = ct.Id
WHERE cp.Name = 'TCF' AND ct.Name = 'Money Order'

GO

UPDATE cpf
SET cpf.FeeMinimum= 1.00 
FROM tChannelPartners cp
	INNER JOIN [tChannelPartnerFees_Check] cpf on cp.rowguid = cpf.ChannelPartnerPK
	INNER JOIN [tCheckTypes] ct on cpf.CheckType = ct.Id
WHERE cp.Name = 'TCF' AND ct.Name = 'RAC/Loan'

GO

UPDATE cpf
SET cpf.FeeMinimum= 1.00
FROM tChannelPartners cp
	INNER JOIN [tChannelPartnerFees_Check] cpf on cp.rowguid = cpf.ChannelPartnerPK
	INNER JOIN [tCheckTypes] ct on cpf.CheckType = ct.Id
WHERE cp.Name = 'TCF' AND ct.Name = 'Two Party'

GO