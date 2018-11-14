-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <07/03/2014>
-- Description:	<User defined type for DeliveryOption> 
-- Rally ID:	<NA>
-- ============================================================
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'DeliveryOptionTableType' AND ss.name = N'dbo')
DROP TYPE [dbo].[DeliveryOptionTableType]
GO

CREATE TYPE [dbo].[DeliveryOptionTableType] AS TABLE
(
	DeliveryOptionId VARCHAR(10) NOT NULL,
	Delivery_Option VARCHAR(255) NOT NULL,
	DeliveryOptionName VARCHAR(255) NOT NULL
)
GO
