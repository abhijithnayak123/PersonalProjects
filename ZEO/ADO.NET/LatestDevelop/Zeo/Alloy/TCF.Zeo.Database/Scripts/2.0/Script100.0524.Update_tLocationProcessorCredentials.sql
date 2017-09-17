-- ==========================================================================================
-- Author:		<Abhijith>
-- Create date: <05/01/2017>
-- Description:	<Migration scripts for VISA Location Processor Credentials.>
-- Jira ID:		<>
-- ===========================================================================================

BEGIN TRY
	BEGIN TRAN;

		DECLARE @visaLocations TABLE
		(
			LocationName NVARCHAR(100)
		)

		INSERT INTO @visaLocations (LocationName)
		VALUES('76th-Appleton'),('Wauwatosa'),('76th-Bradley'),('Kenosha North'),('Greenfield'),('Glendale'),('Shorewood-WI'),('Kenosha South'),('Elm Grove'),('West Milwaukee'),('Oak Creek'),('Uptown Brass'),('Holt Plaza'),('Mt Pleasant'),('Racine North'),('Pick n Save Cudahy'),('Pick N Save North Avenue'),('Piggly Wiggly Oklahoma'),('Kenosha West'),('Clement Manor'),('Piggly Wiggly Capitol Dr'),('Pick N Save Silver Spring'),('Pick N Save New Berlin'),('Pick N Save 35th-North'),('Highland Park'),('Brooklyn Park'),('Knollwood'),('Southtown'),('Southview'),('St Anthony'),('Scenic Hills'),('Maplewood'),('Burnsville'),('Lombard Butterfield'),('Ultra Foods Chicago Heights'),('Jefferson Park'),('Niles'),('Schiller Park'),('North Park'),('Park Ridge'),('Little Village'),('Garfield Ridge West'),('Waukegan'),('Madison'),('Southport-Fullerton'),('Glendale Heights'),('Bedford Park'),('Harlem-Belden'),('Schaumburg'),('Rolling Meadows'),('Burbank'),('New Lenox'),('Wood Dale'),('Shorewood-IL'),('87th-Dan Ryan'),('Homewood'),('Calumet City'),('Ridgedale'),('Brighton Park'),('Garfield Ridge East'),('Oak Lawn - Cicero'),('47th Street'),('Hickory Hills'),('Oak Lawn-Pulaski'),('Palos Heights'),('Willowbrook'),('Naperville'),('Orland Park'),('Mundelein'),('Jewel Ashland-95th'),('Jewel Oak Forest'),('Jewel 87th-Dan Ryan'),('Jewel Marshfield'),('Jewel Pulaski-53rd'),('Jewel Ridgeland-87th St'),('Jewel Stickney'),('Jewel Cicero-79th'),('Jewel Franklin Park'),('Jewel Lagrange Park'),('Jewel Countryside'),('Jewel Melrose Park'),('Jewel River Forest'),('Jewel Paulina-Milwaukee'),('Jewel Ashland-Wellington'),('Jewel Elston-Addison'),('Jewel State Street'),('Jewel West Foster-Pulaski'),('Jewel Boughton-Napperville'),('Jewel Darien'),('Jewel North Lincoln'),('Jewel Sheridan-Montrose'),('Jewel Westmont'),('Jewel Cumberland-Lawrence'),('Jewel Broadway-Foster'),('Jewel Main-63rd Street'),('Jewel Clark-Bryn Mawr'),('Jewel Lisle'),('Jewel Irving Park-Narragansett'),('Jewel Howard-Mccormick'),('Jewel West Touhy'),('Jewel Foster-Harlem'),('Jewel Naper-75th'),('Jewel South Main'),('Jewel Glendale Heights'),('Jewel Buffalo Grove'),('Jewel Larkin-McClean'),('Jewel Mt Prospect'),('Jewel York-Butterfield'),('Jewel Rand-Arlington'),('Jewel Bartlett'),('Jewel Des Plaines'),('Jewel Algonquin'),('Jewel Elk Grove Village'),('Jewel West Dundee'),('Jewel Wood Dale'),('Jewel South Lee'),('Jewel Streamwood'),('Jewel Hoffman Estates'),('Jewel Schaumburg'),('Jewel Irving Park-Kilpatrick'),('Jewel 95th-Route 59'),('Jewel Glenview'),('Jewel Brickyard'),('Jewel Tinley Park'),('Jewel North Broadway'),('Jewel Arlington Heights'),('Jewel Highland Park'),('Jewel 95th-Stony Island'),('Jewel 75th-Stony Island'),('Jewel Hickory Hills'),('Jewel Alsip'),('Jewel Round Lake'),('Jewel North Riverside'),('Jewel Homewood'),('Jewel Frankfort'),('Jewel Skokie'),('Jewel Villa Park'),('Jewel Pfingsten Rd'),('Jewel Palos Park'),('Jewel Addison'),('Jewel Westchester'),('Jewel Roosevelt-Wabash'),('Jewel Schiller-Elmhurst'),('Jewel Dekalb'),('Jewel S Pulaski-Oaklawn'),('Jewel Western-Roscoe'),('Jewel Roosevelt-Ashland'),('Jewel Park Ridge'),('Jewel Archer-Austin'),('Jewel Huntley'),('Jewel Romeoville'),('Jewel Downers Grove'),('Jewel Bensenville'),('Minnehaha'),('Apple Valley'),('Uptown'),('Shoreview'),('Anoka'),('Dixie Highway'),('Lake Orion'),('Waterford'),('Union Lake'),('Clinton Township'),('Roseville-MI'),('Washington'),('Chesterfield'),('Royal Oak'),('East Pointe'),('Rochester Hills'),('Warren'),('Troy'),('Southfield'),('Sterling Heights'),('Commerce Township'),('Shelby Township'),('Northville'),('Livonia'),('Canton'),('Westland'),('Universal Mall'),('Fraser'),('Novi'),('Dequindre Road'),('Rochester Hills South'),('Westland II'),('Southgate'),('Coolidge Highway'),('Independence Township'),('Macomb Township'),('Allen Park'),('North Pontiac'),('Ecorse Road'),('Dearborn Heights'),('Middlebelt Road'),('Mankato'),('Westside'),('Northside'),('Ann Arbor Main'),('Chelsea'),('Dexter'),('Ypsilanti'),('Brighton'),('Saline'),('Briarwood'),('Howell'),('South University'),('Belleville'),('Hartland'),('Farmington Road'),('Carpenter Road'),('Ferndale Kroger'),('Robbinsdale'),('60-Greenfield'),('75th-Cactus'),('Tempe Marketplace'),('Cooper-Ray'),('Signal Butte-Southern'),('Stapley-McKellips'),('35th-Southern'),('Cub Silver Lake'),('Cub Southdale'),('Cub Shakopee'),('Cub Monticello'),('Excelsior'),('Cub Maplewood East'),('Cub Blaine North'),('Cub Woodbury'),('White Bear Lake'),('Dinkytown'),('Cub Minnetonka'),('Edina'),('Plymouth'),('St Cloud West'),('Cub Eagan'),('Cub Blaine'),('Cub Burnsville'),('Cub Stillwater'),('Broadway In-store'),('Parker Pavilions'),('Northglenn'),('Park Meadows'),('Powers-Constitution'),('Buckley'),('Westminster'),('Powers Peak'),('Prince-Belleview'),('80th-Yarrow'),('Havana-Mexico'),('Quebec Square'),('Edgewater Marketplace'),('Belmar'),('Hancock-Academy'),('6th-Chambers'),('96th-Washington'),('Quincy-Buckley'),('Lafayette'),('Chelton-Platte'),('144th-Lowell'),('Union-Alameda'),('Centennial-Fillmore'),('Highlands Ranch'),('Academy-Morning Sun'),('Federal-72nd'),('64th-Gardenia'),('Broadway-Evans'),('University-Evans'),('Green Valley Ranch'),('Briargate Crossing'),('Academy'),('Arvada'),('Melrose Park'),('River Forest'),('Palatine'),('Roseville-MN'),('Richfield'),('Cub West St Paul'),('Cub Lake Street'),('Cub Cottage Grove'),('Cub Apple Valley'),('Northeast'),('Cub Knollwood'),('Cub Mankato'),('Cub Plymouth'),('Cub Coon Rapids'),('Cub Bloomington West'),('Cub St Cloud East'),('Cub White Bear Lake'),('Cub Brookdale'),('Cub Maple Grove'),('Cub Brooklyn Park'),('Cub 60th-Nicollet'),('Cub Crystal'),('Cub Eden Prairie'),('Cub Rosemount'),('IDS'),('Woodbury'),('Forest Lake'),('Eagan Promenade'),('Elk River'),('Cub Midway'),('Chanhassen'),('Crystal'),('Maple Grove'),('Lakeville'),('PCC-Plymouth Corporate Center'),('Coon Rapids'),('Cub Lyndale'),('Cub Fridley'),('Cub Rockford Road'),('Cub Burnsville South'),('Cub Blaine East'),('Cub Har Mar'),('Oak Park Heights'),('Cottage Grove'),('Cub Coon Rapids South'),('Blaine'),('Lexington Park'),('Cub Arden Hills'),('Cub Savage'),('Cub Inver Grove Heights'),('Cub Lakeville'),('Wayzata'),('Cub Sunray'),('Cub Broadway'),('Cub Champlin'),('Cub Buffalo'),('Cub Brooklyn Park North'),('Coffman Union'),('Sioux Falls'),('Cub Eagan East'),('Cub Phalen'),('Jewel North Central'),('Fridley'),('Northtown'),('Brookdale'),('Eden Prairie'),('UMD'),('Cub Maplewood'),('Contact Center-All Support Areas')

		DELETE lp
		FROM dbo.tLocationProcessorCredentials lp
			INNER JOIN tLocations l ON l.LocationId = lp.LocationId
			INNER JOIN @visaLocations vl ON l.LocationName = vl.LocationName
		WHERE lp.ProviderId = 103

		--Insert records for VISA Processor Credentials.
		-- Starts Here 

		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12624',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-202' FROM tLocations WHERE LocationName ='76th-Appleton'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12626',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-203' FROM tLocations WHERE LocationName ='Wauwatosa'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12632',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-206' FROM tLocations WHERE LocationName ='76th-Bradley'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12634',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-207' FROM tLocations WHERE LocationName ='Kenosha North'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12636',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-208' FROM tLocations WHERE LocationName ='Greenfield'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12638',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-209' FROM tLocations WHERE LocationName ='Glendale'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12640',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-210' FROM tLocations WHERE LocationName ='Shorewood-WI'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12642',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-211' FROM tLocations WHERE LocationName ='Kenosha South'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12648',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-214' FROM tLocations WHERE LocationName ='Elm Grove'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'42688',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-1271019' FROM tLocations WHERE LocationName ='West Milwaukee'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'62037',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-1020' FROM tLocations WHERE LocationName ='Oak Creek'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'65188',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-1021' FROM tLocations WHERE LocationName ='Uptown Brass'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'67076',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-1022' FROM tLocations WHERE LocationName ='Holt Plaza'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12652',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-216' FROM tLocations WHERE LocationName ='Mt Pleasant'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12654',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-217' FROM tLocations WHERE LocationName ='Racine North'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12088',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-185' FROM tLocations WHERE LocationName ='Pick n Save Cudahy'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12598',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-189' FROM tLocations WHERE LocationName ='Pick N Save North Avenue'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12606',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-193' FROM tLocations WHERE LocationName ='Piggly Wiggly Oklahoma'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12658',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-219' FROM tLocations WHERE LocationName ='Kenosha West'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'212036',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-220' FROM tLocations WHERE LocationName ='Clement Manor'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12610',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-195' FROM tLocations WHERE LocationName ='Piggly Wiggly Capitol Dr'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12612',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-196' FROM tLocations WHERE LocationName ='Pick N Save Silver Spring'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12614',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-197' FROM tLocations WHERE LocationName ='Pick N Save New Berlin'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12620',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-200' FROM tLocations WHERE LocationName ='Pick N Save 35th-North'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12500',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-145' FROM tLocations WHERE LocationName ='Highland Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12502',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-146' FROM tLocations WHERE LocationName ='Brooklyn Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12504',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-147' FROM tLocations WHERE LocationName ='Knollwood'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12506',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-148' FROM tLocations WHERE LocationName ='Southtown'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12508',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-149' FROM tLocations WHERE LocationName ='Southview'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12510',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-150' FROM tLocations WHERE LocationName ='St Anthony'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12512',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-151' FROM tLocations WHERE LocationName ='Scenic Hills'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12514',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-152' FROM tLocations WHERE LocationName ='Maplewood'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12516',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-153' FROM tLocations WHERE LocationName ='Burnsville'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12940',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-389' FROM tLocations WHERE LocationName ='Lombard Butterfield'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12700',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-225' FROM tLocations WHERE LocationName ='Ultra Foods Chicago Heights'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12942',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-390' FROM tLocations WHERE LocationName ='Jefferson Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13076',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-391' FROM tLocations WHERE LocationName ='Niles'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12944',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-392' FROM tLocations WHERE LocationName ='Schiller Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13078',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-393' FROM tLocations WHERE LocationName ='North Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13080',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-394' FROM tLocations WHERE LocationName ='Park Ridge'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12946',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-395' FROM tLocations WHERE LocationName ='Little Village'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12950',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-397' FROM tLocations WHERE LocationName ='Garfield Ridge West'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12952',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-398' FROM tLocations WHERE LocationName ='Waukegan'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13082',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-400' FROM tLocations WHERE LocationName ='Madison'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12958',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-402' FROM tLocations WHERE LocationName ='Southport-Fullerton'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12962',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-404' FROM tLocations WHERE LocationName ='Glendale Heights'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12964',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-405' FROM tLocations WHERE LocationName ='Bedford Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'30424',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-426' FROM tLocations WHERE LocationName ='Harlem-Belden'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'30422',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-425' FROM tLocations WHERE LocationName ='Schaumburg'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12966',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-406' FROM tLocations WHERE LocationName ='Rolling Meadows'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'30426',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-427' FROM tLocations WHERE LocationName ='Burbank'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'61250',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-2093' FROM tLocations WHERE LocationName ='New Lenox'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'54127',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-2095' FROM tLocations WHERE LocationName ='Wood Dale'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'61249',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-2096' FROM tLocations WHERE LocationName ='Shorewood-IL'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'67074',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-2097' FROM tLocations WHERE LocationName ='87th-Dan Ryan'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'62510',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-2098' FROM tLocations WHERE LocationName ='Homewood'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'65187',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-2099' FROM tLocations WHERE LocationName ='Calumet City'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12518',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-155' FROM tLocations WHERE LocationName ='Ridgedale'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12968',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-407' FROM tLocations WHERE LocationName ='Brighton Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12972',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-410' FROM tLocations WHERE LocationName ='Garfield Ridge East'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12976',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-412' FROM tLocations WHERE LocationName ='Oak Lawn - Cicero'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13086',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-413' FROM tLocations WHERE LocationName ='47th Street'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12978',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-414' FROM tLocations WHERE LocationName ='Hickory Hills'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12982',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-416' FROM tLocations WHERE LocationName ='Oak Lawn-Pulaski'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12984',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-417' FROM tLocations WHERE LocationName ='Palos Heights'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12986',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-418' FROM tLocations WHERE LocationName ='Willowbrook'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12988',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-419' FROM tLocations WHERE LocationName ='Naperville'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12990',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-420' FROM tLocations WHERE LocationName ='Orland Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'56550',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-2125' FROM tLocations WHERE LocationName ='Mundelein'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12714',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-233' FROM tLocations WHERE LocationName ='Jewel Ashland-95th'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12716',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-234' FROM tLocations WHERE LocationName ='Jewel Oak Forest'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12994',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-235' FROM tLocations WHERE LocationName ='Jewel 87th-Dan Ryan'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12718',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-236' FROM tLocations WHERE LocationName ='Jewel Marshfield'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12720',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-237' FROM tLocations WHERE LocationName ='Jewel Pulaski-53rd'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12722',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-238' FROM tLocations WHERE LocationName ='Jewel Ridgeland-87th St'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12996',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-239' FROM tLocations WHERE LocationName ='Jewel Stickney'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12724',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-240' FROM tLocations WHERE LocationName ='Jewel Cicero-79th'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12726',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-241' FROM tLocations WHERE LocationName ='Jewel Franklin Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12728',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-242' FROM tLocations WHERE LocationName ='Jewel Lagrange Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12730',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-243' FROM tLocations WHERE LocationName ='Jewel Countryside'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12732',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-244' FROM tLocations WHERE LocationName ='Jewel Melrose Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12736',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-246' FROM tLocations WHERE LocationName ='Jewel River Forest'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12742',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-250' FROM tLocations WHERE LocationName ='Jewel Paulina-Milwaukee'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12746',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-252' FROM tLocations WHERE LocationName ='Jewel Ashland-Wellington'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12748',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-254' FROM tLocations WHERE LocationName ='Jewel Elston-Addison'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13002',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-255' FROM tLocations WHERE LocationName ='Jewel State Street'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12750',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-256' FROM tLocations WHERE LocationName ='Jewel West Foster-Pulaski'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12752',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-257' FROM tLocations WHERE LocationName ='Jewel Boughton-Napperville'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13004',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-259' FROM tLocations WHERE LocationName ='Jewel Darien'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13006',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-260' FROM tLocations WHERE LocationName ='Jewel North Lincoln'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12758',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-262' FROM tLocations WHERE LocationName ='Jewel Sheridan-Montrose'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13008',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-263' FROM tLocations WHERE LocationName ='Jewel Westmont'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12760',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-264' FROM tLocations WHERE LocationName ='Jewel Cumberland-Lawrence'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12762',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-266' FROM tLocations WHERE LocationName ='Jewel Broadway-Foster'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12764',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-267' FROM tLocations WHERE LocationName ='Jewel Main-63rd Street'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12766',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-268' FROM tLocations WHERE LocationName ='Jewel Clark-Bryn Mawr'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13012',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-269' FROM tLocations WHERE LocationName ='Jewel Lisle'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12768',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-270' FROM tLocations WHERE LocationName ='Jewel Irving Park-Narragansett'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12770',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-272' FROM tLocations WHERE LocationName ='Jewel Howard-Mccormick'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13018',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-276' FROM tLocations WHERE LocationName ='Jewel West Touhy'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12778',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-278' FROM tLocations WHERE LocationName ='Jewel Foster-Harlem'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12780',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-279' FROM tLocations WHERE LocationName ='Jewel Naper-75th'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13020',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-285' FROM tLocations WHERE LocationName ='Jewel South Main'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12792',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-287' FROM tLocations WHERE LocationName ='Jewel Glendale Heights'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12794',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-288' FROM tLocations WHERE LocationName ='Jewel Buffalo Grove'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13024',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-289' FROM tLocations WHERE LocationName ='Jewel Larkin-McClean'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12796',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-290' FROM tLocations WHERE LocationName ='Jewel Mt Prospect'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12798',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-291' FROM tLocations WHERE LocationName ='Jewel York-Butterfield'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12800',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-292' FROM tLocations WHERE LocationName ='Jewel Rand-Arlington'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13026',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-293' FROM tLocations WHERE LocationName ='Jewel Bartlett'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12802',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-294' FROM tLocations WHERE LocationName ='Jewel Des Plaines'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12804',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-295' FROM tLocations WHERE LocationName ='Jewel Algonquin'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12806',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-296' FROM tLocations WHERE LocationName ='Jewel Elk Grove Village'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12808',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-299' FROM tLocations WHERE LocationName ='Jewel West Dundee'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12810',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-300' FROM tLocations WHERE LocationName ='Jewel Wood Dale'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13032',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-302' FROM tLocations WHERE LocationName ='Jewel South Lee'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12814',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-303' FROM tLocations WHERE LocationName ='Jewel Streamwood'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12816',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-304' FROM tLocations WHERE LocationName ='Jewel Hoffman Estates'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12818',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-305' FROM tLocations WHERE LocationName ='Jewel Schaumburg'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12822',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-307' FROM tLocations WHERE LocationName ='Jewel Irving Park-Kilpatrick'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12824',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-309' FROM tLocations WHERE LocationName ='Jewel 95th-Route 59'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13036',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-310' FROM tLocations WHERE LocationName ='Jewel Glenview'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'112391',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-2281' FROM tLocations WHERE LocationName ='Jewel Brickyard'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12832',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-314' FROM tLocations WHERE LocationName ='Jewel Tinley Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12834',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-315' FROM tLocations WHERE LocationName ='Jewel North Broadway'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12836',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-316' FROM tLocations WHERE LocationName ='Jewel Arlington Heights'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12840',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-318' FROM tLocations WHERE LocationName ='Jewel Highland Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12842',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-319' FROM tLocations WHERE LocationName ='Jewel 95th-Stony Island'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12850',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-324' FROM tLocations WHERE LocationName ='Jewel 75th-Stony Island'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12854',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-327' FROM tLocations WHERE LocationName ='Jewel Hickory Hills'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13042',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-328' FROM tLocations WHERE LocationName ='Jewel Alsip'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12862',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-332' FROM tLocations WHERE LocationName ='Jewel Round Lake'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12866',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-336' FROM tLocations WHERE LocationName ='Jewel North Riverside'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12868',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-337' FROM tLocations WHERE LocationName ='Jewel Homewood'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13048',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-338' FROM tLocations WHERE LocationName ='Jewel Frankfort'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13050',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-340' FROM tLocations WHERE LocationName ='Jewel Skokie'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12872',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-341' FROM tLocations WHERE LocationName ='Jewel Villa Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12874',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-343' FROM tLocations WHERE LocationName ='Jewel Pfingsten Rd'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12878',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-346' FROM tLocations WHERE LocationName ='Jewel Palos Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13056',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-348' FROM tLocations WHERE LocationName ='Jewel Addison'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12888',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-352' FROM tLocations WHERE LocationName ='Jewel Westchester'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12900',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-363' FROM tLocations WHERE LocationName ='Jewel Roosevelt-Wabash'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12908',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-368' FROM tLocations WHERE LocationName ='Jewel Schiller-Elmhurst'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13066',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-369' FROM tLocations WHERE LocationName ='Jewel Dekalb'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12912',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-371' FROM tLocations WHERE LocationName ='Jewel S Pulaski-Oaklawn'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12914',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-372' FROM tLocations WHERE LocationName ='Jewel Western-Roscoe'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12916',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-373' FROM tLocations WHERE LocationName ='Jewel Roosevelt-Ashland'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12918',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-374' FROM tLocations WHERE LocationName ='Jewel Park Ridge'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12922',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-376' FROM tLocations WHERE LocationName ='Jewel Archer-Austin'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13070',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-380' FROM tLocations WHERE LocationName ='Jewel Huntley'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12928',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-381' FROM tLocations WHERE LocationName ='Jewel Romeoville'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'62509',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-2357' FROM tLocations WHERE LocationName ='Jewel Downers Grove'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'383373',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-2368' FROM tLocations WHERE LocationName ='Jewel Bensenville'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12524',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-158' FROM tLocations WHERE LocationName ='Minnehaha'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12528',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-160' FROM tLocations WHERE LocationName ='Apple Valley'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12530',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-161' FROM tLocations WHERE LocationName ='Uptown'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12532',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-162' FROM tLocations WHERE LocationName ='Shoreview'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12572',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-137' FROM tLocations WHERE LocationName ='Anoka'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12202',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-033' FROM tLocations WHERE LocationName ='Dixie Highway'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12258',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-034' FROM tLocations WHERE LocationName ='Lake Orion'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12260',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-035' FROM tLocations WHERE LocationName ='Waterford'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12204',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-036' FROM tLocations WHERE LocationName ='Union Lake'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12206',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-037' FROM tLocations WHERE LocationName ='Clinton Township'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12208',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-038' FROM tLocations WHERE LocationName ='Roseville-MI'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12210',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-039' FROM tLocations WHERE LocationName ='Washington'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12212',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-040' FROM tLocations WHERE LocationName ='Chesterfield'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12262',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-041' FROM tLocations WHERE LocationName ='Royal Oak'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12214',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-042' FROM tLocations WHERE LocationName ='East Pointe'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12216',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-043' FROM tLocations WHERE LocationName ='Rochester Hills'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12264',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-044' FROM tLocations WHERE LocationName ='Warren'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12266',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-045' FROM tLocations WHERE LocationName ='Troy'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12248',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-079' FROM tLocations WHERE LocationName ='Southfield'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12220',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-048' FROM tLocations WHERE LocationName ='Sterling Heights'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12222',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-049' FROM tLocations WHERE LocationName ='Commerce Township'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12224',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-050' FROM tLocations WHERE LocationName ='Shelby Township'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12226',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-051' FROM tLocations WHERE LocationName ='Northville'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12298',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-052' FROM tLocations WHERE LocationName ='Livonia'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12270',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-053' FROM tLocations WHERE LocationName ='Canton'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12272',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-054' FROM tLocations WHERE LocationName ='Westland'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12300',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-055' FROM tLocations WHERE LocationName ='Universal Mall'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12274',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-056' FROM tLocations WHERE LocationName ='Fraser'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12304',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-057' FROM tLocations WHERE LocationName ='Novi'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'56650',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-3055' FROM tLocations WHERE LocationName ='Dequindre Road'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'54027',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-3056' FROM tLocations WHERE LocationName ='Rochester Hills South'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'29822',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-422' FROM tLocations WHERE LocationName ='Westland II'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12302',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-058' FROM tLocations WHERE LocationName ='Southgate'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'67290',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-3064' FROM tLocations WHERE LocationName ='Coolidge Highway'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'29826',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-424' FROM tLocations WHERE LocationName ='Independence Township'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'42886',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-3067' FROM tLocations WHERE LocationName ='Macomb Township'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'67469',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-3069' FROM tLocations WHERE LocationName ='Allen Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'64776',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-3070' FROM tLocations WHERE LocationName ='North Pontiac'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'68000',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-3072' FROM tLocations WHERE LocationName ='Ecorse Road'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'67470',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-3074' FROM tLocations WHERE LocationName ='Dearborn Heights'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'75045',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-431' FROM tLocations WHERE LocationName ='Middlebelt Road'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12534',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-163' FROM tLocations WHERE LocationName ='Mankato'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12276',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-059' FROM tLocations WHERE LocationName ='Westside'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12278',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-060' FROM tLocations WHERE LocationName ='Northside'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12228',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-061' FROM tLocations WHERE LocationName ='Ann Arbor Main'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12280',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-062' FROM tLocations WHERE LocationName ='Chelsea'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12282',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-063' FROM tLocations WHERE LocationName ='Dexter'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12284',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-064' FROM tLocations WHERE LocationName ='Ypsilanti'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12230',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-065' FROM tLocations WHERE LocationName ='Brighton'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12286',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-066' FROM tLocations WHERE LocationName ='Saline'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12232',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-067' FROM tLocations WHERE LocationName ='Briarwood'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12234',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-068' FROM tLocations WHERE LocationName ='Howell'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12236',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-069' FROM tLocations WHERE LocationName ='South University'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12238',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-070' FROM tLocations WHERE LocationName ='Belleville'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12288',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-071' FROM tLocations WHERE LocationName ='Hartland'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'67291',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-3119' FROM tLocations WHERE LocationName ='Farmington Road'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12252',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-082' FROM tLocations WHERE LocationName ='Carpenter Road'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12192',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-027' FROM tLocations WHERE LocationName ='Ferndale Kroger'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12492',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-138' FROM tLocations WHERE LocationName ='Robbinsdale'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'67213',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-4001' FROM tLocations WHERE LocationName ='60-Greenfield'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'67239',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-4002' FROM tLocations WHERE LocationName ='75th-Cactus'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'68610',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-4003' FROM tLocations WHERE LocationName ='Tempe Marketplace'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'69628',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-4004' FROM tLocations WHERE LocationName ='Cooper-Ray'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'91858',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-4006' FROM tLocations WHERE LocationName ='Signal Butte-Southern'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'88580',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-4007' FROM tLocations WHERE LocationName ='Stapley-McKellips'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'88428',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-4008' FROM tLocations WHERE LocationName ='35th-Southern'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12086',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-084' FROM tLocations WHERE LocationName ='Cub Silver Lake'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12568',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-085' FROM tLocations WHERE LocationName ='Cub Southdale'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12392',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-086' FROM tLocations WHERE LocationName ='Cub Shakopee'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12394',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-087' FROM tLocations WHERE LocationName ='Cub Monticello'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12574',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-139' FROM tLocations WHERE LocationName ='Excelsior'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12396',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-088' FROM tLocations WHERE LocationName ='Cub Maplewood East'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12398',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-089' FROM tLocations WHERE LocationName ='Cub Blaine North'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12402',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-091' FROM tLocations WHERE LocationName ='Cub Woodbury'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12540',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-168' FROM tLocations WHERE LocationName ='White Bear Lake'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12542',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-169' FROM tLocations WHERE LocationName ='Dinkytown'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12404',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-092' FROM tLocations WHERE LocationName ='Cub Minnetonka'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12576',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-140' FROM tLocations WHERE LocationName ='Edina'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12586',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-170' FROM tLocations WHERE LocationName ='Plymouth'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12546',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-172' FROM tLocations WHERE LocationName ='St Cloud West'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12406',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-093' FROM tLocations WHERE LocationName ='Cub Eagan'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12408',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-094' FROM tLocations WHERE LocationName ='Cub Blaine'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12410',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-095' FROM tLocations WHERE LocationName ='Cub Burnsville'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12412',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-096' FROM tLocations WHERE LocationName ='Cub Stillwater'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12134',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-019' FROM tLocations WHERE LocationName ='Broadway In-store'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12082',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-001' FROM tLocations WHERE LocationName ='Parker Pavilions'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12092',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-002' FROM tLocations WHERE LocationName ='Northglenn'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12094',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-003' FROM tLocations WHERE LocationName ='Park Meadows'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12096',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-004' FROM tLocations WHERE LocationName ='Powers-Constitution'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12126',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-005' FROM tLocations WHERE LocationName ='Buckley'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12098',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-006' FROM tLocations WHERE LocationName ='Westminster'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12100',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-007' FROM tLocations WHERE LocationName ='Powers Peak'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12102',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-008' FROM tLocations WHERE LocationName ='Prince-Belleview'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12104',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-009' FROM tLocations WHERE LocationName ='80th-Yarrow'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12106',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-010' FROM tLocations WHERE LocationName ='Havana-Mexico'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12108',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-011' FROM tLocations WHERE LocationName ='Quebec Square'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12110',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-012' FROM tLocations WHERE LocationName ='Edgewater Marketplace'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12128',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-013' FROM tLocations WHERE LocationName ='Belmar'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12112',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-014' FROM tLocations WHERE LocationName ='Hancock-Academy'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12114',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-015' FROM tLocations WHERE LocationName ='6th-Chambers'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'28821',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7030' FROM tLocations WHERE LocationName ='96th-Washington'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'42588',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-1277031' FROM tLocations WHERE LocationName ='Quincy-Buckley'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12130',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-016' FROM tLocations WHERE LocationName ='Lafayette'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'28823',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7033' FROM tLocations WHERE LocationName ='Chelton-Platte'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'62035',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7034' FROM tLocations WHERE LocationName ='144th-Lowell'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'51500',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7035' FROM tLocations WHERE LocationName ='Union-Alameda'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'51496',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7037' FROM tLocations WHERE LocationName ='Centennial-Fillmore'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'63951',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7038' FROM tLocations WHERE LocationName ='Highlands Ranch'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'68257',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7040' FROM tLocations WHERE LocationName ='Academy-Morning Sun'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'63952',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7041' FROM tLocations WHERE LocationName ='Federal-72nd'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'63953',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7042' FROM tLocations WHERE LocationName ='64th-Gardenia'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'64775',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7043' FROM tLocations WHERE LocationName ='Broadway-Evans'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'68777',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7045' FROM tLocations WHERE LocationName ='University-Evans'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'87876',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7061' FROM tLocations WHERE LocationName ='Green Valley Ranch'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'88581',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-7062' FROM tLocations WHERE LocationName ='Briargate Crossing'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12116',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-017' FROM tLocations WHERE LocationName ='Academy'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12132',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-018' FROM tLocations WHERE LocationName ='Arvada'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12932',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-384' FROM tLocations WHERE LocationName ='Melrose Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12934',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-385' FROM tLocations WHERE LocationName ='River Forest'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'13074',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-387' FROM tLocations WHERE LocationName ='Palatine'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12494',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-142' FROM tLocations WHERE LocationName ='Roseville-MN'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12496',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-143' FROM tLocations WHERE LocationName ='Richfield'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12416',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-098' FROM tLocations WHERE LocationName ='Cub West St Paul'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12418',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-099' FROM tLocations WHERE LocationName ='Cub Lake Street'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12420',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-100' FROM tLocations WHERE LocationName ='Cub Cottage Grove'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12422',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-101' FROM tLocations WHERE LocationName ='Cub Apple Valley'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12548',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-173' FROM tLocations WHERE LocationName ='Northeast'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12426',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-103' FROM tLocations WHERE LocationName ='Cub Knollwood'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12428',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-104' FROM tLocations WHERE LocationName ='Cub Mankato'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12430',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-105' FROM tLocations WHERE LocationName ='Cub Plymouth'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12432',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-106' FROM tLocations WHERE LocationName ='Cub Coon Rapids'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12434',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-107' FROM tLocations WHERE LocationName ='Cub Bloomington West'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12436',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-108' FROM tLocations WHERE LocationName ='Cub St Cloud East'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12438',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-109' FROM tLocations WHERE LocationName ='Cub White Bear Lake'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12440',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-110' FROM tLocations WHERE LocationName ='Cub Brookdale'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12442',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-111' FROM tLocations WHERE LocationName ='Cub Maple Grove'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12444',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-112' FROM tLocations WHERE LocationName ='Cub Brooklyn Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12446',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-113' FROM tLocations WHERE LocationName ='Cub 60th-Nicollet'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12448',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-114' FROM tLocations WHERE LocationName ='Cub Crystal'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12450',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-115' FROM tLocations WHERE LocationName ='Cub Eden Prairie'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12452',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-116' FROM tLocations WHERE LocationName ='Cub Rosemount'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12550',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-174' FROM tLocations WHERE LocationName ='IDS'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12588',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-175' FROM tLocations WHERE LocationName ='Woodbury'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12552',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-176' FROM tLocations WHERE LocationName ='Forest Lake'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12554',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-177' FROM tLocations WHERE LocationName ='Eagan Promenade'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12556',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-178' FROM tLocations WHERE LocationName ='Elk River'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12454',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-117' FROM tLocations WHERE LocationName ='Cub Midway'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12558',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-179' FROM tLocations WHERE LocationName ='Chanhassen'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'67254',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-9101' FROM tLocations WHERE LocationName ='Crystal'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12560',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-180' FROM tLocations WHERE LocationName ='Maple Grove'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12562',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-181' FROM tLocations WHERE LocationName ='Lakeville'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'299196',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-9105' FROM tLocations WHERE LocationName ='PCC-Plymouth Corporate Center'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12564',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-182' FROM tLocations WHERE LocationName ='Coon Rapids'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12456',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-118' FROM tLocations WHERE LocationName ='Cub Lyndale'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12458',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-119' FROM tLocations WHERE LocationName ='Cub Fridley'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12460',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-120' FROM tLocations WHERE LocationName ='Cub Rockford Road'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12462',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-121' FROM tLocations WHERE LocationName ='Cub Burnsville South'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12466',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-123' FROM tLocations WHERE LocationName ='Cub Blaine East'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12468',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-124' FROM tLocations WHERE LocationName ='Cub Har Mar'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12566',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-183' FROM tLocations WHERE LocationName ='Oak Park Heights'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12590',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-184' FROM tLocations WHERE LocationName ='Cottage Grove'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12472',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-126' FROM tLocations WHERE LocationName ='Cub Coon Rapids South'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'29922',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-421' FROM tLocations WHERE LocationName ='Blaine'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'88425',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-9124' FROM tLocations WHERE LocationName ='Lexington Park'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12478',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-129' FROM tLocations WHERE LocationName ='Cub Arden Hills'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12570',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-130' FROM tLocations WHERE LocationName ='Cub Savage'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12480',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-131' FROM tLocations WHERE LocationName ='Cub Inver Grove Heights'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12482',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-132' FROM tLocations WHERE LocationName ='Cub Lakeville'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'190928',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-9131' FROM tLocations WHERE LocationName ='Wayzata'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12484',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-133' FROM tLocations WHERE LocationName ='Cub Sunray'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12486',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-134' FROM tLocations WHERE LocationName ='Cub Broadway'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'67250',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-9134' FROM tLocations WHERE LocationName ='Cub Champlin'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'67252',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-9136' FROM tLocations WHERE LocationName ='Cub Buffalo'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'65182',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-9137' FROM tLocations WHERE LocationName ='Cub Brooklyn Park North'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'51506',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-9139' FROM tLocations WHERE LocationName ='Coffman Union'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'95508',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-9141' FROM tLocations WHERE LocationName ='Sioux Falls'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'91950',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-9144' FROM tLocations WHERE LocationName ='Cub Eagan East'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'96342',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-9145' FROM tLocations WHERE LocationName ='Cub Phalen'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12858',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-330' FROM tLocations WHERE LocationName ='Jewel North Central'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12498',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-144' FROM tLocations WHERE LocationName ='Fridley'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12580',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-154' FROM tLocations WHERE LocationName ='Northtown'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12520',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-156' FROM tLocations WHERE LocationName ='Brookdale'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12522',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-157' FROM tLocations WHERE LocationName ='Eden Prairie'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12584',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-167' FROM tLocations WHERE LocationName ='UMD'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'12414',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-097' FROM tLocations WHERE LocationName ='Cub Maplewood'
		INSERT INTO [dbo].[tLocationProcessorCredentials]([ProviderId],[UserName],[Password],[Identifier],[DTServerCreate],[DTServerLastModified],[DTTerminalCreate]
		,[DTTerminalLastModified],[LocationId],[Identifier2]) SELECT 103,NULL,NULL,'383376',GETDATE(),NULL,GETDATE(),NULL,LocationId,'SC127-00299' FROM tLocations WHERE LocationName ='Contact Center-All Support Areas'
		
		-- Ends Here

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
