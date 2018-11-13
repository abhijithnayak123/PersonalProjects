
@echo Configuring System, Please wait...

@echo SDK Driver is already installed

ping 1.1.1.1 -n 1 -w 15000 > nul

@echo Uninstalling PDS Driver
"%~dp0\Epson\32Bit\Epson_Pds_Driver_v102_Win7_32Bit.exe" /s /uninstall
@echo Uninstallation complete

ping 1.1.1.1 -n 1 -w 10000 > nul

@echo Installing PDS Driver
"%~dp0\Epson\32Bit\Epson_Pds_Driver_v102_Win7_32Bit.exe"
@echo Installation Complete

@echo Successfully configured the system.
