IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetGiftwrap'
		)
BEGIN
	DROP PROCEDURE USP_EX_GetGiftwrap;

	PRINT 'DROPPED USP_EX_GetGiftwrap';
END
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************
** Name : USP_EX_GetGiftwrap 
** Old Name: It was not there in old it is new
** Short Desc:	
** Full Description
**        This stored proc is used to populate Gift wrap details for DonorBeneAddressChange request type
**
** Sample Call
	USP_EX_GetGiftwrap 'E2266B5F-47FE-4229-BDDF-A263F4A85DB8' 
	
** Return values: 
**
**
** Standard declarations
**       SET NOCOUNT             ON
**       SET LOCK_TIMEOUT         30000   -- 30 seconds
**	
**	Created By: Sanath 
**	Company	  :	Opteamix
**	Project	  :	Excelsior TAG
**	Created DT:	10-June-2014
**            
*******************************************************************************
**       Change History
*******************************************************************************
** Date:        Author:  Bug #     Description:                           Rvwd
** --------     -------- ------    -------------------------------------- --------
** 09/09/2014	Yashasvi 16952		Modified the query to join tblPerson table with client id as 4195
*******************************************************************************
** Copyright (C) <CopyrightYear,,Year> Kaspick & Company, All Rights Reserved
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION
** 
*******************************************************************************/
CREATE PROCEDURE [dbo].USP_EX_GetGiftwrap  
(
@Guid VARCHAR(255)
)

AS
BEGIN
     BEGIN TRY
   SELECT DISTINCT ISNULL(GiftwrAddr.Street1, '') + ' ' + ISNULL(GiftwrAddr.Street2, '') +' ' + ISNULL(GiftwrAddr.City, '') + ' ' + ISNULL(GiftwrAddr.StateCode, '') + ' ' + ISNULL(GiftwrAddr.ZipCode, '') + ' ' + ISNULL(GiftwrAddr.Country, '') AS Giftwrap
    ,GiftwrAddrType.Short AS Type--,GiftwrAddr.AddressID
     from syn_gw_tbladdress GiftwrAddr
INNER JOIN syn_gw_tblperson sgp on sgp.personid = GiftwrAddr.sourceid and sgp.ClientId=4195
INNER JOIN tbl_wft_donorbenecontactaddress dba on cast(dba.contactid as varchar(20)) = sgp.personaccount4
INNER JOIN TBL_WFT_DBAddressChange AddChg ON dba.DBAddressChangeID=AddChg.DBAddressChangeID
INNER JOIN syn_gw_tbladdresstype GiftwrAddrType on GiftwrAddrType.addresstypeid = GiftwrAddr.addresstypeid and GiftwrAddrType.short = dba.addresstype
WHERE AddChg.GUID=@Guid		
   END TRY
  BEGIN CATCH
		DECLARE @ProcName VARCHAR(60);
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SET @ProcName = 'USP_EX_GetGiftwrap';

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

		RAISERROR (
				@ErrorMessage,
				-- Message text.
				@ErrorSeverity,
				-- Severity.
				@ErrorState -- State.
				);
	END CATCH 
 	
END
GO

SET NOCOUNT OFF;

IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_GetGiftwrap'
		)
BEGIN
	PRINT 'CREATED PROCEDURE USP_EX_GetGiftwrap';
END