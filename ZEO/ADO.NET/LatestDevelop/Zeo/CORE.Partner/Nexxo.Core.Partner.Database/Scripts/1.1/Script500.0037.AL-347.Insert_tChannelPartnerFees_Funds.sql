-- ================================================================================
-- Author:		<Shwetha Mohan>
-- Create date: <06/15/2015>
-- Description:	<Insert records to tChannelPartnerFees_Funds for Redstone channel partner >
-- Jira ID:		<AL-347>
-- ================================================================================
IF NOT EXISTS
	(
		SELECT 
			1 
		FROM 
			tChannelPartnerFees_Funds 
		WHERE 
			ChannelPartnerPK =  'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6'
	) 
	BEGIN
		INSERT [dbo].[tChannelPartnerFees_Funds]( [ChannelPartnerPK],[FundsType],[Fee]) VALUES 
		( 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6',0,0.00),
		( 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6',1,0.00),
		( 'F8365E3B-FDD5-439A-BE9E-9B4444E17ED6',2,0.00)
	END
	GO