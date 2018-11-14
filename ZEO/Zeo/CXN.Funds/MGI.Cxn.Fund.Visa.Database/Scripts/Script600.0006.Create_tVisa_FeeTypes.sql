-- =======================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <11/27/2015>
-- Description:	<DDL script to create tVisa_FeeTypes table>
-- Rally ID:	<AL-1639>
-- =======================================================================

IF NOT EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[tVisa_FeeTypes]') AND TYPE in (N'U'))
BEGIN

CREATE TABLE tVisa_FeeTypes
(
	VisaFeeTypePK UNIQUEIDENTIFIER NOT NULL,
	VisaFeeTypeId BIGINT IDENTITY(1,1) NOT NULL,
	Name VARCHAR(500) NOT NULL,
	DTServerCreate DATETIME NOT NULL,
	DTServerLastModified DATETIME NULL

CONSTRAINT PK_tVisa_FeeTypes PRIMARY KEY CLUSTERED 
(
	VisaFeeTypePK ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]

END
GO
