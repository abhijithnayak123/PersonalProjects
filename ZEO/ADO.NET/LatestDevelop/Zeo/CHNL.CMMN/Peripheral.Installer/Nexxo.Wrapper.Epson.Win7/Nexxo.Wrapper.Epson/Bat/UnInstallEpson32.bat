@echo Configuring System, Please wait...
@echo
ping 1.1.1.1 -n 1 -w 30000 > nul
cd %temp%
@echo Initiating PDS Removal Process
"Epson_Pds_Driver_v102_Win7_32Bit.exe" /s /Uninstall
@echo Successfully configured the system.
