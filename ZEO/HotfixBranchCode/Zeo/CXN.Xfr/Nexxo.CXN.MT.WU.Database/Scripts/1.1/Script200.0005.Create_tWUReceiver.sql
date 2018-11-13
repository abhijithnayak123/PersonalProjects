IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUReceiver]') AND type in (N'U'))
BEGIN
DROP TABLE [dbo].[tWUReceiver]
END
go
CREATE TABLE tWUReceiver
(
	rowguid UNIQUEIDENTIFIER,
	Id BIGINT IDENTITY(1000000000,1),
	FirstName VARCHAR(100),
	LastName VARCHAR(100),
	SecondLastName VARCHAR(100),
	[Status] VARCHAR(20),
	Gender VARCHAR(10),
	Country VARCHAR(200),
	[Address] VARCHAR(250),
	City VARCHAR(200),
	[State/Province] VARCHAR(200),
	ZipCode VARCHAR(10),
	PhoneNumber VARCHAR(20),
	PickupCountry VARCHAR(100),
	[PickupState/Province] VARCHAR(100),
	PickupCity VARCHAR(100),
	DeliveryMethod VARCHAR(100),
	DeliveryOption VARCHAR(100),
	Occupation VARCHAR(100),
	DOB DATETIME,
	CountryOfBirth VARCHAR(100),
	CustomerId BIGINT,
	DTCreate DATETIME,
	DTLastModified DATETIME,
	PRIMARY KEY(rowguid)
)