--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <18-11-2016>
-- Description:	Get WU Bill Pay transaction request details.
-- Jira ID:		<AL-8320>
-- ================================================================================

--  exec usp_GetWUBillPayTransactionRequest 2000000022 

IF OBJECT_ID('usp_GetWUBillPayTransactionRequest') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_GetWUBillPayTransactionRequest
END
GO


CREATE PROCEDURE usp_GetWUBillPayTransactionRequest
(
	@wuBillPayTrxID BIGINT
)
AS
BEGIN
	BEGIN TRY
		SELECT  
			tc.FirstName,
			tc.LAStName,
			tc.Address1,
			tc.Address2,
			tc.City,
			tc.State,
			tc.ZipCode AS Zip,
			tc.DOB,
			tc.Phone1 AS CustomerPhoneNumber,
			tc.Email,
			tc.GovtIdentification AS PrimaryIdNumber,
			tc.SSN AS SecondIdNumber,
			tc.CountryOfBirth,
			o.Name AS Occupation,
			tn.Name AS PrimaryIdType,
			CASE
				WHEN tc.Phone1 IS NOT NULL AND tc.Phone1Type = 'Cell'
			    THEN tc.Phone1
				WHEN tc.Phone2 IS NOT NULL AND tc.Phone2Type = 'Cell'
				THEN tc.Phone2
				ELSE ''
			END AS CustomerMobileNumber,			
			ISNULL(tm.Name,'') AS PrimaryIdCountryOfIssue,
			ISNULL(tn.Country,'') AS PrimaryCountryOfIssue,
			ISNULL(ts.Name ,'') AS PrimaryIdPlaceOfIssue,
			ISNULL(tm.Abbr2,'') AS PrimaryIdCountryOfIssueCode,
			ISNULL(ts.Abbr,'') AS PrimaryIdPlaceOfIssueCode,
			'SSN' AS SecondIdType,
			'US' AS SecondIdCountryOfIssue,
			isnull(tmc.Abbr3,'') AS CountryOfBirthAbbr3,
			twt.Sender_City AS SenderCity,
			twt.ForeignRemoteSystem_Reference_no AS ForeignRemoteSystemReferenceNo,
			wbc.CardNumber,
			twt.Sender_ComplianceDetails_ComplianceData_Buffer AS SenderComplianceDataBuffer,
			twt.BillerName,
			twt.Biller_CityCode AS BillerCityCode,
			twt.Customer_AccountNumber AS CustomerAccountNumber,
			twt.QPCompany_Department AS QPCompanyDepartment,
			twt.Financials_OriginatorsPrincipalAmount AS FinancialsOriginatorsPrincipalAmount,
			twt.Financials_DestinationPrincipalAmount AS FinancialsDestinationPrincipalAmount,
			twt.Financials_Fee AS FinancialsFee,
			twt.Financials_PlusChargesAmount AS FinancialsPlusChargesAmount,
			twt.Financials_UndiscountedCharges AS FinancialsUndiscountedCharges,
			twt.Financials_TotalDiscount AS FinancialsTotalDiscount,
			twt.Financials_DiscountedCharges AS FinancialsDiscountedCharges,
			twt.PaymentDetails_ExchangeRate AS PaymentDetailsExchangeRate,
			twt.PaymentDetails_AuthStatus AS PaymentDetailsAuthStatus,
			ISNULL(twt.promotions_promo_code_description,'') AS PromotionsPromoCodeDescription,
			ISNULL(twt.promotions_promo_sequence_no,'') AS PromotionsPromoSequenceNo,
			IsNULL(twt.promotions_promo_name,'') AS PromotionsPromoName,
			ISNULL(twt.promotions_promo_message,'') AS PromotionsPromoMessage,
			twt.promotions_promo_discount_amount AS PromotionsPromoDiscountAmount,
			ISNULL(twt.promotions_promotion_error,'') AS PromotionsPromotionError,
			ISNULL(twt.promotions_sender_promo_code,'') AS PromotionsSenderPromoCode,
			twt.DeliveryCode,
			twt.ConvSessionCookie,
			twt.MTCN,
			twt.NewMTCN
		FROM 
			tCustomers tc 
			INNER JOIN tNexxoIdTypes tn WITH (NOLOCK) ON tn.NexxoIdTypeID = tc.GovtIdTypeId
			INNER JOIN tOccupations o WITH (NOLOCK) ON o.Code = tc.Occupation
			INNER JOIN tMasterCountries tm WITH (NOLOCK) ON tm.MASterCountriesID = tn.MASterCountriesID
			LEFT JOIN tStates ts WITH (NOLOCK) ON ts.StateId = tn.StateId
			INNER JOIN tMasterCountries tmc WITH (NOLOCK) ON tmc.Abbr2 = tc.CountryOfBirth
			INNER JOIN tWUnion_BillPay_Account wbc WITH (NOLOCK) ON wbc.CustomerId = tc.CustomerID
			INNER JOIN tWUnion_BillPay_Trx twt WITH (NOLOCK) ON twt.WUBillPayAccountId = wbc.WUBillPayAccountID
		WHERE 
			twt.WUBillPayTrxID = @wuBillPayTrxID
	END TRY

	BEGIN CATCH
		EXECUTE dbo.usp_CreateErrorInfo
	END CATCH
END
GO


