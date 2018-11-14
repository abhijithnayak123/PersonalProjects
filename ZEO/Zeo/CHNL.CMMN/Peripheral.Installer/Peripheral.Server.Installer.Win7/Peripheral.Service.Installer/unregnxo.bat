
@echo Configuring System, Please wait...
regedit /s "%~dp0\unregnxo.reg"
sc stop "ZEOPeripheralService"
sc delete "ZEOPeripheralService"
@echo Successfully configured the system.
