
@echo Initializing uninstallation for 32 Bit System, Please wait...
cd %temp%
"%~dp0\Epson\32Bit\TMS9000DRV102\Setup\Setup.exe" /s "%~dp0\Epson\32Bit\TMS9000DRV102\Setup\SInst.ini"
ping 1.1.1.1 -n 1 -w 5000 > nul
@echo Copying Epson 32 Bit Driver to temporary directory
copy /Y "%~dp0\Epson\32Bit\Epson_Pds_Driver_v102_Win7_32Bit.exe" "%temp%"
@echo Copying 32 Bit Batch Script to temporary directory
copy /Y "%~dp0\UnInstallEpson32.bat" "%temp%"
@echo Successfully configured the system.
