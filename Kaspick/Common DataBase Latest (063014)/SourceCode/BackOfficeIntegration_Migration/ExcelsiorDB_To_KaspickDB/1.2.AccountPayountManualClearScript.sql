SET NOCOUNT ON

DECLARE @DISPSEQ INT
	,@MAINMODULEID INT
	,@APPLICATIONID INT
	,@MODULEID INT
	,@FUNCID8 INT
	,@ROLEID INT
	,@PRIVILEGEID INT

--1. Insert Application table
IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_APPLICATION
		WHERE ApplicationName = 'Payments'
		)
BEGIN
	SET @DISPSEQ = 0

	SELECT @DISPSEQ = ISNULL(MAX(DisplaySequence) + 10, 0)
	FROM TBL_KS_APPLICATION

	INSERT INTO [TBL_KS_APPLICATION] (
		[ApplicationName]
		,[ABBREV]
		,[DESCRIPTION]
		,[DisplaySequence]
		,[IsActive]
		)
	VALUES (
		'Payments'
		,''
		,'Payments'
		,@DISPSEQ
		,1
		)
END

SELECT @ApplicationID = ApplicationID
FROM TBL_KS_APPLICATION
WHERE ApplicationName = 'Payments'

--2. Insert Main Module table
IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_MAINMODULE
		WHERE MainModuleName = 'Payments'
		)
BEGIN
	SET @DISPSEQ = 0

	SELECT @DISPSEQ = ISNULL(MAX(DisplaySequence) + 10, 0)
	FROM TBL_KS_Module

	INSERT INTO [TBL_KS_MAINMODULE] (
		[ApplicationID]
		,[MainModuleName]
		,[ABBREV]
		,[DESCRIPTION]
		,[DisplaySequence]
		,[IsActive]
		)
	VALUES (
		@ApplicationID
		,'Payments'
		,''
		,'User can set Payments'
		,@DISPSEQ
		,1
		)
END

SELECT @MainModuleID = MainModuleID
FROM TBL_KS_MAINMODULE
WHERE MainModuleName = 'Payments'

--3. Insert Module table
SELECT @DISPSEQ = ISNULL(MAX(DisplaySequence), 0)
FROM TBL_KS_Module

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_Module
		WHERE ModuleName = 'AccountPayOut'
		)
BEGIN
	SET @DISPSEQ = @DISPSEQ + 10

	INSERT INTO TBL_KS_Module (
		ApplicationID
		,ModuleName
		,ABBREV
		,DESCRIPTION
		,DisplaySequence
		,IsActive
		,MainModuleID
		)
	VALUES (
		@ApplicationID
		,'AccountPayOut'
		,'APO'
		,'Account Payout'
		,@DISPSEQ
		,1
		,@MainModuleID
		)
END

--4. Get Functions IDs from Function Table
SELECT @funcid8 = FunctionID
FROM TBL_KS_Function
WHERE FunctionName = 'Non-Standard'

--5. Insert Privilege Table
SET @DISPSEQ = 0

SELECT @DISPSEQ = ISNULL(MAX(DisplaySequence), 0)
FROM TBL_KS_Privilege

--5.1 Insert Privilege Table - Middleware Rule
SELECT @ModuleID = ModuleID
FROM TBL_KS_Module
WHERE ModuleName = 'AccountPayOut'

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_Privilege
		WHERE ModuleID = @ModuleID
			AND PrivilegeName = 'ManualClear_visibility'
		)
BEGIN
	SET @DISPSEQ = @DISPSEQ + 10

	INSERT INTO TBL_KS_Privilege (
		PrivilegeName
		,DESCRIPTION
		,DisplaySequence
		,ModuleID
		,FunctionID
		)
	VALUES (
		'ManualClear_visibility'
		,'Allow to access manual clear in Account payout'
		,@DISPSEQ
		,@ModuleID
		,@FUNCID8
		)
END

--6 Insert TBL_KS_RolePrivilege table
SELECT @PrivilegeID = PrivilegeID
FROM TBL_KS_Privilege
WHERE PrivilegeName = 'ManualClear_visibility'

SELECT @ROLEID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Manager - Client Account Management'

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_RolePrivilege
		WHERE RoleID = @ROLEID
			AND PrivilegeID = @PrivilegeID
		)
BEGIN
	INSERT INTO TBL_KS_RolePrivilege (
		RoleID
		,PrivilegeID
		)
	VALUES (
		@ROLEID
		,@PrivilegeID
		)
END

SELECT @ROLEID = RoleID
FROM TBL_KS_Role
WHERE RoleName = 'Manager - Trust Administration'

IF NOT EXISTS (
		SELECT 1
		FROM TBL_KS_RolePrivilege
		WHERE RoleID = @RoleID
			AND PrivilegeID = @PrivilegeID
		)
BEGIN
	INSERT INTO TBL_KS_RolePrivilege (
		RoleID
		,PrivilegeID
		)
	VALUES (
		@ROLEID
		,@PrivilegeID
		)
END
