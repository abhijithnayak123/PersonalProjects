BEGIN
	DECLARE @centrisGuid UNIQUEIDENTIFIER

	SELECT
		@centrisGuid = rowguid 
	FROM 
		tChannelPartners
	WHERE 
		Name = 'Centris'

	INSERT INTO tChannelPartnerConfig 
	(
		ChannelPartnerID,
		DisableWithdrawCNP,
		CashOverCounter
	) 
	VALUES 
	--Centris 
	(
		@centrisGuid,
		0,
		0
	)
	,
	-- Synovus
	(
		'EC6AAAE3-2BA7-4E0B-898E-D296DB432C17',
		1,
		1
	)
END