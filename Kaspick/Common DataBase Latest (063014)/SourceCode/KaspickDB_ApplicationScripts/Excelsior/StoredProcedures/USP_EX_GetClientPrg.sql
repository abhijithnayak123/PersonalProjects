IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientPrg'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetClientPrg;

	PRINT 'DROPPED USP_EX_GetClientPrg';
END
GO

/*********************************************************************************************************************                                                     
* New Procedure Name  : USP_EX_GetClientPrg            
* Old Procedure Name  : USP_EIS_EX_CLIENT_PRG_SelProc            
* Description     : Retrieving all program details  along with the no of active accounts           
*      : argument not considered while retrieving the values           
* Input Parameter : @ManagerCode          
* Modification Log                                                         
*                                        
* Date       Modified By    Description          

Sample Call
   EXEC USP_EX_GetClientPrg 'AK'                                           
*--------------------------------------------------------------------------------------------------------------------                                                     
* 20-Nov-06  Pranav     Created             
* 06-Feb-06  Venugopal B   Modified:CS.STATUS_ID IS CHANGED TO PS.STATUS_ID IS TO FIX ELEMEN TOOL ISSUE NO: 4041            
* 25-Apr-14 Mallikarjun  Modified inner client page
* 22-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard
*********************************************************************************************************************/
CREATE PROCEDURE [dbo].[USP_EX_GetClientPrg] 
	(@ManagerCode VARCHAR(15))
AS
BEGIN
	SELECT DISTINCT (AccMgrCds.ManagerCode)
		,ISNULL(ConMstr.CONTACTNAME, '') AS CLIENTNAME
		,ISNULL(ConMstr.SSN, '') AS TAXID
		,ISNULL(AccMgrCds.ManagerCode, '') AS CLIENTBRIEFNAME
		,AlnsNmbr.ALLIANCENUMBER
		,ISNULL(AlnsNmbr.ALLIANCENUMBER, '') AS BRIEFNAME
		,ISNULL(AlnsNmbr.AllianceDesc, '') AS PROGRAMNAME
		,'' AS PROGRAMTYPE
		,
		--ISNULL(P.PROGRAMTYPE, '') AS PROGRAMTYPE,
		'' AS GROUPID
		,
		--ISNULL(AlnsNmbr.GROUPID, '') AS GROUPID,
		CASE AccMgrCds.ActiveFlag
			WHEN - 1
				THEN 'Active'
			WHEN 0
				THEN 'Inactive'
			END AS STATUS
		,
		--LI.LIST_ITEM_NAME AS STATUS,
		NOACTIVEACCOUNTS = ISNULL((
				SELECT COUNT(AccMstr.CustomerAccountNumber)
				FROM SYN_IT_AccountMaster AccMstr
				INNER JOIN SYN_IT_UDF_AccountMaster UdfAccMstr ON AccMstr.CustomerAccountNumber = UdfAccMstr.CustomerAccountNumber_KEY
				--INNER JOIN TBL_EIS_EX_DEFERREDGIFTACCOUNT_SUPPLEMENT DS ON DS.AccountID = AccMstr.AccountID
				--INNER JOIN V_EIS_LIST_ITEMS VLI ON VLI.LIST_ITEM_ID = DS.STATUS_ID
				WHERE AlnsNmbr.ALLIANCENUMBER = AccMstr.ALLIANCENUMBER
					AND UDFAMColumn030 IS NULL
				), 0)
		,NOPIFACCOUNTS = ISNULL((
				SELECT COUNT(ACCOUNTTYPECODE)
				FROM SYN_IT_AccountMaster AccMstr
				--INNER JOIN TBL_EIS_EX_DEFERREDGIFTACCOUNT_SUPPLEMENT DS ON DS.AccountID = AccMstr.AccountID
				--INNER JOIN V_EIS_LIST_ITEMS VLI ON VLI.LIST_ITEM_ID = DS.STATUS_ID
				WHERE AlnsNmbr.ALLIANCENUMBER = AccMstr.ALLIANCENUMBER
					AND AccMstr.ACCOUNTTYPECODE = 'PIF'
				), '')
		,NOGAPACCOUNTS = ISNULL((
				SELECT COUNT(ACCOUNTTYPECODE)
				FROM SYN_IT_AccountMaster AccMstr
				WHERE AlnsNmbr.ALLIANCENUMBER = AccMstr.ALLIANCENUMBER
					AND AccMstr.ACCOUNTTYPECODE = 'GAP'
				), '')
		,0 AS Association
	FROM SYN_IT_AccountManagerCodes AccMgrCds
	 INNER JOIN SYN_IT_AccountMaster AccMstr ON AccMstr.ManagerCode=AccMgrCds.ManagerCode  
	INNER JOIN SYN_IT_ContactMaster ConMstr ON AccMgrCds.ManagerCode = ConMstr.ManagerCode
	INNER JOIN SYN_IT_AllianceNumbers AlnsNmbr ON ConMstr.ContactID = AlnsNmbr.ContactID
	--INNER JOIN SYN_IT_AllianceNumbers AlnsNmbr ON AlnsNmbr.Alliancenumber =  AccMstr.Alliancenumber   
	WHERE AccMgrCds.ManagerCode = @ManagerCode
	ORDER BY AlnsNmbr.ALLIANCENUMBER
END
GO

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetClientPrg'
		)
BEGIN
	PRINT 'CREATED USP_EX_GetClientPrg';
END
GO

