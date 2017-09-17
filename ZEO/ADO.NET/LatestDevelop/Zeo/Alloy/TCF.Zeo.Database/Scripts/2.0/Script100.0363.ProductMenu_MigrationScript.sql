--- ===============================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <01-23-2017>
-- Description:	As an engineer, I want to implement ADO.Net for Products
-- Jira ID:		<AL->
-- ================================================================================

BEGIN TRY
	BEGIN TRAN;


		UPDATE 
			tProductProcessorsMapping
		SET 
			ProductId = p.ProductsID ,
			ProcessorId = pr.ProcessorsID
		FROM 
			tProductProcessorsMapping ppm 
			INNER JOIN tProducts p ON ppm.ProductPK = p.ProductsPK
			INNER JOIN tProcessors pr ON ppm.ProcessorPK = pr.ProcessorsPK

		ALTER TABLE 
			tProductProcessorsMapping 
		ALTER COLUMN 
			ProductId BIGINT NOT NULL

		ALTER TABLE 
			tProductProcessorsMapping 
		ALTER COLUMN 
			ProcessorId BIGINT NOT NULL

		IF NOT EXISTS (
				SELECT 1 
				FROM sys.foreign_keys 
				WHERE object_id = OBJECT_ID(N'[dbo].[FK_tProductProcessorsMapping_tProducts]'))
		BEGIN
			ALTER TABLE 
				tProductProcessorsMapping  
			ADD CONSTRAINT 
				FK_tProductProcessorsMapping_tProducts 
			FOREIGN KEY
				(ProductId)
			REFERENCES 
				tProducts(ProductsID)	
		END

		IF NOT EXISTS (
				SELECT 1 
				FROM sys.foreign_keys 
				WHERE object_id = OBJECT_ID(N'[dbo].[FK_tProductProcessorsMapping_tProcessors]'))
		BEGIN
			ALTER TABLE 
				tProductProcessorsMapping  
			ADD CONSTRAINT 
				FK_tProductProcessorsMapping_tProcessors
			FOREIGN KEY
				(ProcessorID)
			REFERENCES 
				tProcessors(ProcessorsID)	
		END

		UPDATE 
			tChannelPartnerProductProcessorsMapping
		SET 
			ProductProcessorId = ppm.ProductProcessorsMappingID
		FROM 
			tChannelPartnerProductProcessorsMapping cppm 
			INNER JOIN tProductProcessorsMapping ppm ON ppm.ProductProcessorsMappingPK = cppm.ProductProcessorPK


		ALTER TABLE 
			tChannelPartnerProductProcessorsMapping 
		ALTER COLUMN 
			ProductProcessorId BIGINT NOT NULL


		IF NOT EXISTS (
				SELECT 1 
				FROM sys.foreign_keys 
				WHERE object_id = OBJECT_ID(N'[dbo].[FK_tChannelPartnerProductProcessorsMapping_tProductProcessorsMapping]'))
		BEGIN
			ALTER TABLE 
				tChannelPartnerProductProcessorsMapping  
			ADD CONSTRAINT 
				FK_tChannelPartnerProductProcessorsMapping_tProductProcessorsMapping
			FOREIGN KEY
				(ProductProcessorId)
			REFERENCES 
				tProductProcessorsMapping(ProductProcessorsMappingID)	
		END

	COMMIT TRAN
END TRY
BEGIN CATCH
	 IF(@@TRANCOUNT > 0)
		SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage,
		XACT_STATE()as state;
		ROLLBACK TRAN
END CATCH;
