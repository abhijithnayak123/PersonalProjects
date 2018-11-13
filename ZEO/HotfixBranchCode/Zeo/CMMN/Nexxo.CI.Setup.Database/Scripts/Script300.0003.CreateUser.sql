IF NOT EXISTS (select loginname from master.dbo.syslogins where name = '$DBUserName$')
BEGIN
	CREATE LOGIN $DBUserName$ WITH PASSWORD = '$DBUserPassword$', CHECK_POLICY= OFF;
END
 
USE $DBPrifix$_PCAT
BEGIN
	IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = '$DBUserName$')
	BEGIN
		CREATE USER [$DBUserName$] FOR LOGIN [$DBUserName$]
	END
	ELSE
	BEGIN
		ALTER USER [$DBUserName$] WITH LOGIN = [$DBUserName$]
	END
	EXEC sp_addrolemember N'db_owner', N'$DBUserName$'
END

USE [$DBPrifix$_COMPLIANCE]
BEGIN
	IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = '$DBUserName$')
	BEGIN
		CREATE USER [$DBUserName$] FOR LOGIN [$DBUserName$]
	END
	ELSE
	BEGIN
		ALTER USER [$DBUserName$] WITH LOGIN = [$DBUserName$]
	END
	EXEC sp_addrolemember N'db_owner', N'$DBUserName$'
END

USE [$DBPrifix$_CXE]
BEGIN
	IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = '$DBUserName$')
	BEGIN
		CREATE USER [$DBUserName$] FOR LOGIN [$DBUserName$]
	END
	ELSE
	BEGIN
		ALTER USER [$DBUserName$] WITH LOGIN = [$DBUserName$]
	END
	EXEC sp_addrolemember N'db_owner', N'$DBUserName$'
END

USE [$DBPrifix$_CXN]
BEGIN
	IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = '$DBUserName$')
	BEGIN
		CREATE USER [$DBUserName$] FOR LOGIN [$DBUserName$]
	END
	ELSE
	BEGIN
		ALTER USER [$DBUserName$] WITH LOGIN = [$DBUserName$]
	END
	EXEC sp_addrolemember N'db_owner', N'$DBUserName$'
END

USE [$DBPrifix$_PTNR]
BEGIN
	IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = '$DBUserName$')
	BEGIN
		CREATE USER [$DBUserName$] FOR LOGIN [$DBUserName$]
	END
	ELSE
	BEGIN
		ALTER USER [$DBUserName$] WITH LOGIN = [$DBUserName$]
	END
	EXEC sp_addrolemember N'db_owner', N'$DBUserName$'
END
