IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = '$DBPrifix$_PCAT') OR name = '$DBPrifix$_PCAT') 
BEGIN
	ALTER DATABASE $DBPrifix$_PCAT SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE $DBPrifix$_PCAT
	Print '$DBPrifix$_PCAT database is Droped'
END

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = '$DBPrifix$_COMPLIANCE') OR name = '$DBPrifix$_COMPLIANCE') 
BEGIN
	ALTER DATABASE $DBPrifix$_COMPLIANCE SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE $DBPrifix$_COMPLIANCE
	Print '$DBPrifix$_COMPLIANCE database is Droped'
END
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = '$DBPrifix$_CXE') OR name = '$DBPrifix$_CXE') 
BEGIN
	ALTER DATABASE $DBPrifix$_CXE SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE $DBPrifix$_CXE
	Print '$DBPrifix$_CXE database is Droped'
END
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = '$DBPrifix$_CXN') OR name = '$DBPrifix$_CXN') 
BEGIN
	ALTER DATABASE $DBPrifix$_CXN SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE $DBPrifix$_CXN
	Print '$DBPrifix$_CXN database is Droped'
END
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = '$DBPrifix$_PTNR') OR name = '$DBPrifix$_PTNR') 
BEGIN
	ALTER DATABASE $DBPrifix$_PTNR SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE $DBPrifix$_PTNR
	Print '$DBPrifix$_PTNR database is Droped'
END

CREATE DATABASE $DBPrifix$_PCAT 
PRINT '$DBPrifix$_PCAT database is Created'
CREATE DATABASE $DBPrifix$_COMPLIANCE
PRINT '$DBPrifix$_COMPLIANCE database is Created'
CREATE DATABASE $DBPrifix$_CXE
PRINT '$DBPrifix$_CXE database is Created'
CREATE DATABASE $DBPrifix$_CXN
PRINT '$DBPrifix$_CXN database is Created'
CREATE DATABASE $DBPrifix$_PTNR
PRINT '$DBPrifix$_PTNR database is Created'

