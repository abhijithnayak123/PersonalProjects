SET NOCOUNT ON
BEGIN 
	DECLARE @ApplicationID INT
	DECLARE @DISP_SEQ INT
	DECLARE @MainModuleID INT

--1. Insert Application table
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_APPLICATION WHERE ApplicationName ='Middleware') 
	BEGIN
		SET @DISP_SEQ = 0
		SELECT @DISP_SEQ = ISNULL(MAX(DisplaySequence)+10,0) FROM TBL_KS_APPLICATION
		INSERT INTO [TBL_KS_APPLICATION] ([ApplicationName], [ABBREV], [DESCRIPTION], [DisplaySequence], [IsActive]) VALUES ('Middleware','Mdl','Middleware', @DISP_SEQ, 1)
	End
	SELECT @ApplicationID = ApplicationID FROM TBL_KS_APPLICATION WHERE ApplicationName ='Middleware'

--2. Insert Main Module table
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_MAINMODULE WHERE MainModuleName ='Middleware')
	BEGIN
		SET @DISP_SEQ = 0
		SELECT @DISP_SEQ = ISNULL(MAX(DisplaySequence)+10,0) FROM TBL_KS_Module
		INSERT INTO [TBL_KS_MAINMODULE] ([ApplicationID], [MainModuleName], [ABBREV], [DESCRIPTION], [DisplaySequence], [IsActive]) VALUES ( @ApplicationID,'Middleware','Mddl','User can set Payment Process Rules', @DISP_SEQ, 1)
	End
	SELECT @MainModuleID = MainModuleID FROM TBL_KS_MAINMODULE WHERE MainModuleName ='Middleware'

--3. Insert Module table
	SELECT @DISP_SEQ = ISNULL(MAX(DisplaySequence),0) FROM TBL_KS_Module 
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Module WHERE ModuleName = 'MiddlewareRules')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT Into TBL_KS_Module (ApplicationID,ModuleName,ABBREV,DESCRIPTION,DisplaySequence,IsActive,MainModuleID) VALUES (@ApplicationID,'MiddlewareRules','Rule','Pargon Middleware Rule Management', @DISP_SEQ, 1 ,@MainModuleID)
	END
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Module WHERE ModuleName = 'MiddlewareMessage')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT Into TBL_KS_Module (ApplicationID,ModuleName,ABBREV,DESCRIPTION,DisplaySequence,IsActive,MainModuleID) VALUES (@ApplicationID,'MiddlewareMessage','MgMmt','Pargon Middleware Message Management', @DISP_SEQ, 1 ,@MainModuleID)
	END
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Module WHERE ModuleName = 'MiddlewarePaymentProcessing')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT Into TBL_KS_Module (ApplicationID,ModuleName,ABBREV,DESCRIPTION,DisplaySequence,IsActive,MainModuleID) VALUES (@ApplicationID,'MiddlewarePaymentProcessing','PProc','Pargon Middleware Payment Processing', @DISP_SEQ, 1 ,@MainModuleID)
	END
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Module WHERE ModuleName = 'MiddlewareReports')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT Into TBL_KS_Module (ApplicationID,ModuleName,ABBREV,DESCRIPTION,DisplaySequence,IsActive,MainModuleID) VALUES (@ApplicationID,'MiddlewareReports','MRpt','Pargon Middleware Report Module', @DISP_SEQ, 1 ,@MainModuleID)
	END


--4. Get Functions IDs from Function Table
	DECLARE @ModuleID int,@funcid1 int, @funcid2 int, @funcid3 int, @funcid4 int, @funcid5 int, @funcid6 int, @funcid7 int, @funcid8 int
	SELECT @funcid1= FunctionID FROM TBL_KS_Function WHERE FunctionName ='Read'
	SELECT @funcid2= FunctionID FROM TBL_KS_Function WHERE FunctionName ='Add'
	SELECT @funcid3= FunctionID FROM TBL_KS_Function WHERE FunctionName ='Edit'
	SELECT @funcid4= FunctionID FROM TBL_KS_Function WHERE FunctionName ='Delete'
	SELECT @funcid5= FunctionID FROM TBL_KS_Function WHERE FunctionName ='Print'
	SELECT @funcid6= FunctionID FROM TBL_KS_Function WHERE FunctionName ='Export'
	SELECT @funcid7= FunctionID FROM TBL_KS_Function WHERE FunctionName ='Execute'
	SELECT @funcid8= FunctionID FROM TBL_KS_Function WHERE FunctionName ='Non-Standard'

--5. Insert Privilege Table
	SET @DISP_SEQ = 0
	SELECT @DISP_SEQ = ISNULL(MAX(DisplaySequence),0)  FROM TBL_KS_Privilege
--5.1 Insert Privilege Table - Middleware Rule
	SELECT @ModuleID = ModuleID from TBL_KS_Module where ModuleName ='MiddlewareRules'
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareRules_Read')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareRules_Read','Allows the ability to Read for Middleware Rules', @DISP_SEQ,@ModuleID,@FUNCID1)
	END
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareRules_Add')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareRules_Add','Allows the ability to Add for Middleware Rules', @DISP_SEQ,@ModuleID,@FUNCID2)
	END
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareRules_Edit')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareRules_Edit','Allows the ability to Edit for Middleware Rules', @DISP_SEQ,@ModuleID,@FUNCID3)
	END
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareRules_Delete')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareRules_Delete','Allows the ability to Delete for Middleware Rules', @DISP_SEQ,@ModuleID,@FUNCID4)
	END
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareRules_Export')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareRules_Export','Allows the ability to Export for Middleware Rules', @DISP_SEQ,@ModuleID,@FUNCID6)
	END
	
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareRules_Print')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareRules_Print','Allows the ability to Print for Middleware Rules', @DISP_SEQ,@ModuleID,@FUNCID5)
	END
	
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareRules_Execute')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareRules_Execute','Allows the ability to Execute for Middleware Rules', @DISP_SEQ,@ModuleID,@FUNCID7)
	END
	
--5.2 Insert Privilege Table - Middleware Message	
	SELECT @ModuleID = ModuleID from TBL_KS_Module where ModuleName ='MiddlewareMessage'
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareMessage_Read')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareMessage_Read','Allows the ability to Read for Middleware Message', @DISP_SEQ,@ModuleID,@FUNCID1)
	END
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareMessage_Add')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareMessage_Add','Allows the ability to Add for Middleware Message', @DISP_SEQ,@ModuleID,@FUNCID2)
	END
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareMessage_Edit')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareMessage_Edit','Allows the ability to Edit for Middleware Message', @DISP_SEQ,@ModuleID,@FUNCID3)
	END
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareMessage_Delete')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareMessage_Delete','Allows the ability to Delete for Middleware Message', @DISP_SEQ,@ModuleID,@FUNCID4)
	END
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareMessage_Export')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareMessage_Export','Allows the ability to Export for Middleware Message', @DISP_SEQ,@ModuleID,@FUNCID6)
	END

	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareMessage_Print')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareMessage_Print','Allows the ability to Print for Middleware Message', @DISP_SEQ,@ModuleID,@FUNCID5)
	END
	
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareMessage_Execute')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareMessage_Execute','Allows the ability to Execute for Middleware Message', @DISP_SEQ,@ModuleID,@FUNCID7)
	END

--5.3 Insert Privilege Table - Middleware Payment Processing
	SELECT @ModuleID = ModuleID from TBL_KS_Module where ModuleName ='MiddlewarePaymentProcessing'
	
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewarePaymentProcessing_Execute')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewarePaymentProcessing_Execute','Allows the ability to Execute for Middleware Payment Processing', @DISP_SEQ,@ModuleID,@FUNCID7)
	END

--5.4 Insert Privilege Table - Middleware Report
	SELECT @ModuleID = ModuleID from TBL_KS_Module where ModuleName ='MiddlewareReports'
	
	IF NOT EXISTS (SELECT 1 FROM TBL_KS_Privilege WHERE ModuleID = @ModuleID AND PrivilegeName = 'MiddlewareReports_Execute')
	BEGIN
		SET @DISP_SEQ = @DISP_SEQ + 10
		INSERT into TBL_KS_Privilege (PrivilegeName,DESCRIPTION,DisplaySequence,ModuleID,FunctionID) VALUES ('MiddlewareReports_Execute','Allows the ability to Execute for Middleware Reports', @DISP_SEQ,@ModuleID,@FUNCID7)
	END

	
--6. Insert Role-Privilege
	DELETE FROM TBL_KS_RolePrivilege WHERE PrivilegeID IN ( SELECT PrivilegeID FROM TBL_KS_Privilege WHERE ModuleID IN  (SELECT ModuleID FROM TBL_KS_Module WHERE ApplicationID = @ApplicationID AND MainModuleID = @MainModuleID)) 

	DECLARE @RoleID_CAM INT,@RoleID_MTA INT,@RoleID_TA INT,@RoleID_CTM INT, @RoleID_PS INT, @RoleID_IT INT , @RoleID_PA INT, @PrivilegeID INT 
	SELECT @RoleID_CAM = RoleID From TBL_KS_Role where RoleName ='Client Account Manager'
	SELECT @RoleID_MTA = RoleID From TBL_KS_Role where RoleName ='Manager - Trust Administration'
	SELECT @RoleID_TA = RoleID From TBL_KS_Role where RoleName ='Trust Administrator'
	SELECT @RoleID_CTM = RoleID From TBL_KS_Role where RoleName ='Client Transition Manager'
	SELECT @RoleID_PS = RoleID From TBL_KS_Role where RoleName ='Production Services'
	SELECT @RoleID_IT = RoleID From TBL_KS_Role where RoleName ='IT'
	SELECT @RoleID_PA = RoleID From TBL_KS_Role where RoleName ='Payment Administrator'

	SELECT @PrivilegeID = PrivilegeID From TBL_KS_Privilege where PrivilegeName ='MiddlewareRules_Read' 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CAM,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_MTA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_TA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CTM,@PrivilegeID) 
	
	SELECT @PrivilegeID = PrivilegeID From TBL_KS_Privilege where PrivilegeName ='MiddlewareRules_Add' 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CAM,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_MTA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_TA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CTM,@PrivilegeID) 
	
	SELECT @PrivilegeID = PrivilegeID From TBL_KS_Privilege where PrivilegeName ='MiddlewareRules_Edit' 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CAM,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_MTA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_TA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CTM,@PrivilegeID) 
	
	SELECT @PrivilegeID = PrivilegeID From TBL_KS_Privilege where PrivilegeName ='MiddlewareRules_Delete' 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CAM,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_MTA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_TA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CTM,@PrivilegeID) 
	
	SELECT @PrivilegeID = PrivilegeID From TBL_KS_Privilege where PrivilegeName ='MiddlewareRules_Export' 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CAM,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_MTA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_TA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CTM,@PrivilegeID) 
	
	SELECT @PrivilegeID = PrivilegeID From TBL_KS_Privilege where PrivilegeName ='MiddlewareMessage_Read' 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CAM,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_MTA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_TA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CTM,@PrivilegeID) 
	
	SELECT @PrivilegeID = PrivilegeID From TBL_KS_Privilege where PrivilegeName ='MiddlewareMessage_Add' 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CAM,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_MTA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_TA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CTM,@PrivilegeID) 
	
	SELECT @PrivilegeID = PrivilegeID From TBL_KS_Privilege where PrivilegeName ='MiddlewareMessage_Edit' 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CAM,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_MTA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_TA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CTM,@PrivilegeID) 
	
	SELECT @PrivilegeID = PrivilegeID From TBL_KS_Privilege where PrivilegeName ='MiddlewareMessage_Delete' 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CAM,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_MTA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_TA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CTM,@PrivilegeID) 
	
	SELECT @PrivilegeID = PrivilegeID From TBL_KS_Privilege where PrivilegeName ='MiddlewareMessage_Export' 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CAM,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_MTA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_TA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CTM,@PrivilegeID) 
	
	SELECT @PrivilegeID = PrivilegeID From TBL_KS_Privilege where PrivilegeName ='MiddlewarePaymentProcessing_Execute' 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_PS,@PrivilegeID) 
	
	SELECT @PrivilegeID = PrivilegeID From TBL_KS_Privilege where PrivilegeName ='MiddlewareReports_Execute' 
	
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CAM,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_MTA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_TA,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_CTM,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_PS,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_IT,@PrivilegeID) 
	INSERT into TBL_KS_RolePrivilege (RoleID,PrivilegeID) Values (@RoleID_PA,@PrivilegeID) 
	
	Declare @ParagonAppID INT, @MWareAppID INT
	
	SELECT @ParagonAppID = ApplicationID FROM TBL_KS_Application WHERE ApplicationName = 'Payments'
	SELECT @MWareAppID =  ApplicationID FROM TBL_KS_Application WHERE ApplicationName = 'Middleware'
	   
	-- Middleware user migration to Paragon   
	INSERT INTO dbo.TBL_KS_UserApplication
	SELECT UserID, @MWareAppID FROM dbo.TBL_KS_UserApplication WHERE ApplicationID = @ParagonAppID

END
GO



