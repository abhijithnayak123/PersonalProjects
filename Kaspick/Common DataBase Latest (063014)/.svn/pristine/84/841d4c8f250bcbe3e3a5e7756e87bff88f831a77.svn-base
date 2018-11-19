
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE type = 'P'
			AND NAME = 'USP_EX_DelDeliverable '
		)
BEGIN
	DROP PROCEDURE USP_EX_DelDeliverable;

	PRINT 'DROPPED USP_EX_DelDeliverable ';
END
GO

/******************************************************************************          
** New Name : USP_EX_DelDeliverable
** Old Name   :   USP_EIS_DLV_Deliverable_DelProc           
**    
** Short Desc :    Deleting deliverables  
**          
** Full Description          
**          
**  Deleting deliverables    
**    
** Sample Call     
 EXEC USP_EX_DelDeliverable  78,0,0    
**          
** Return values: NONE          
**          
** Standard declarations          
**       SET LOCK_TIMEOUT         30000   -- 30 seconds          
** Created By :  Tanuj Gupta    
** Company  :  Kaspick & Company          
** Project  :  Deliverable Tool         
** Created DT :  May/19/2011           
**                      
*******************************************************************************          
**       Change History          
*******************************************************************************          
** Date:     Author:  Bug #  Description:        Rvwd          
** --------  -------- ------ ------------------------------------------ -------    
** Mar-12-2012 Ashvin   Changes for Adding Comments as per Stored Procedure standard template.  
** Mar-14-2012 Ashvin   ERD changes implemetation.  
** Mar-22-2012 Ashvin   Removing TBL_EIS_DT_Process_Methods as it is master table  
** Apr-2-2014  Yugandhar	  EXCREQ5.4 Modified
** 23-May-2014  Sanath   Sp name renamed as per Kaspick naming convention standard 
*******************************************************************************          
** Copyright (C) 2007 Kaspick & Company, All Rights Reserved          
** COMPANY CONFIDENTIAL -- NOT FOR DISTRIBUTION          
*******************************************************************************/
CREATE PROCEDURE USP_EX_DelDeliverable (
	@DeliverableID INT
	,@IsClientAssociated BIT OUTPUT
	,@IsReportAssociated BIT OUTPUT
	)
AS
BEGIN

		DECLARE @ReportInaPackageAssociated INT

		SELECT @ReportInaPackageAssociated = DeliverableID
		FROM TBL_DLV_ManagerCodeDeliverableReportInPackage
		WHERE DeliverableID = @DeliverableID

		SET @IsClientAssociated = 0;
		SET @IsReportAssociated = 0;

		IF @DeliverableID > 0
		BEGIN
			IF (
					NOT EXISTS (
						SELECT DeliverableID
						FROM TBL_DLV_ManagerCodeDeliverable
						WHERE DeliverableID = @DeliverableID
						)
					)
			BEGIN
				IF (
						NOT EXISTS (
							SELECT DeliverableID
							FROM TBL_DLV_Deliverable
							WHERE Parent_DeliverableID = @DeliverableID
							)
						)
				BEGIN
				  
				
				  
					IF (isnull(@ReportInaPackageAssociated, 0) = 0)
					BEGIN
					Begin Transaction     
                       Begin Try  
						DELETE
						FROM TBL_DLV_DeliverableWebsiteOption
						WHERE DeliverableID = @DeliverableID

						DELETE
						FROM TBL_DLV_DeliverableYearType
						WHERE DeliverableID = @DeliverableID

						DELETE
						FROM TBL_DLV_DeliverableFrequency
						WHERE DeliverableID = @DeliverableID

						DELETE
						FROM TBL_DLV_DeliverableServiceOffering
						WHERE DeliverableID = @DeliverableID

						DELETE
						FROM TBL_DLV_DeliverableMethod
						WHERE DeliverableID = @DeliverableID
						
						DELETE
						FROM TBL_DLV_Deliverable
						WHERE DeliverableID = @DeliverableID
						
					

						COMMIT TRANSACTION
					
						 END TRY
						BEGIN CATCH
							ROLLBACK TRANSACTION

							DECLARE @ProcName VARCHAR(60);
							DECLARE @ErrorMessage NVARCHAR(4000);
							DECLARE @ErrorSeverity INT;
							DECLARE @ErrorState INT;

							SET @ProcName = 'USP_EX_DelDeliverable';

							DECLARE @ErrorNumber INT;

							SELECT @ErrorMessage = ERROR_MESSAGE()
								,@ErrorSeverity = ERROR_SEVERITY()
								,@ErrorState = ERROR_STATE()
								,@ErrorNumber = ERROR_NUMBER();

							RAISERROR (
									@ErrorMessage
									,
									-- Message text.
									@ErrorSeverity
									,
									-- Severity.
									@ErrorState -- State.
									);
						END CATCH
					END
					ELSE
					BEGIN
						SET @IsReportAssociated = 1;
					END
				END
				ELSE
				BEGIN
					SET @IsReportAssociated = 1;
				END
			END
			ELSE
			BEGIN
				SET @IsClientAssociated = 1;
			END
		END 
			END 
			GO

		IF EXISTS (
				SELECT *
				FROM sysobjects
				WHERE type = 'P'
					AND NAME = 'USP_EX_DelDeliverable '
				)
		BEGIN
			PRINT 'CREATED PROCEDURE USP_EX_DelDeliverable ';
		END