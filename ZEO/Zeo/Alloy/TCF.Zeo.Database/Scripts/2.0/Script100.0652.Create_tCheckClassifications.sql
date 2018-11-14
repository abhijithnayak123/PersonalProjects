-- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-20-2017>
-- Description:	Create a new table to store the Check Classifications for determining the Provider. 
-- Jira ID:		<B-08674>
-- ================================================================================

-- Create new table as 'tCheckClassifications'
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE OBJECT_NAME(parent_object_id) = 'tCheckClassifications')
BEGIN
    CREATE TABLE tCheckClassifications
    ( 
		CheckClassificationId	BIGINT IDENTITY(1000000000,1) NOT NULL,
		CheckTypeId				INT NOT NULL,
		StartRoutingNumber 		BIGINT NOT NULL,
		EndRoutingNumber  		BIGINT NOT NULL,
		StartAccountNumber 		BIGINT NULL,
		EndAccountNumber  		BIGINT NULL,
		AccountNumberLength		INT NULL,
		StateId					BIGINT NULL,
		DTServerCreate			DATETIME NOT NULL,
		DTServerLastModified	DATETIME NULL,

		CONSTRAINT PK_tCheckClassifications PRIMARY KEY CLUSTERED 
		(
			CheckClassificationId ASC
		),

		CONSTRAINT FK_tCheckClassifications_tStates FOREIGN KEY (StateId)
		REFERENCES tstates(StateId),

		CONSTRAINT [FK_tCheckClassifications_tCheckTypes] FOREIGN KEY ([CheckTypeId])
		REFERENCES [tCheckTypes]([CheckTypeId])
     )
	
END
GO

