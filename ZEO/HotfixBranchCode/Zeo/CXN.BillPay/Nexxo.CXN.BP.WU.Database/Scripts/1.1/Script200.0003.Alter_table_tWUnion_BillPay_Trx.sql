-- ====================================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <12/27/2013>
-- Description:	<Drop unused columns from table 'tWUnion_BillPay_Trx'>
-- ====================================================================

IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'PromotionMessage' and Object_ID = Object_ID(N'tWUnion_BillPay_Trx'))
BEGIN
	ALTER TABLE 
		tWUnion_BillPay_Trx
	DROP COLUMN 
		PromotionMessage,
		Promotion_DiscountAmount,
		Promotion_Error,
		Promotions_SenderCode
END
GO

-- ============================================================
-- Description:	<Add new columns to table 'tWUnion_BillPay_Trx' 
--				 part of promo/coupon code implementation>
-- ============================================================
ALTER TABLE 
	tWUnion_BillPay_Trx
ADD	
	promotions_coupons_promotions			VARCHAR(30),
	promotions_promo_code_description		VARCHAR(250),
	promotions_promo_sequence_no			VARCHAR(15),
	promotions_promo_name					VARCHAR(50),
	promotions_promo_message				VARCHAR(250),
	promotions_promo_discount_amount		BIGINT,
	promotions_promotion_error				VARCHAR(250),
	promotions_sender_promo_code			VARCHAR(30)
GO