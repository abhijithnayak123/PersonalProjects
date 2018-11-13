

@echo Initializing uninstallation for 32 Bit System, Please wait...
cd %temp%
ping 1.1.1.1 -n 1 -w 5000 > nul
@echo Copying Epson 64Bit Driver to temporary directory
copy /Y "%~dp0\Epson\64Bit\Epson_Pds_Driver_v102_Win7_64Bit.exe" "%temp%"
@echo Copying 64 Bit Batch Script to temporary directory
copy /Y "%~dp0\UnInstallEpson64.bat" "%temp%"
@echo Successfully configured the system.