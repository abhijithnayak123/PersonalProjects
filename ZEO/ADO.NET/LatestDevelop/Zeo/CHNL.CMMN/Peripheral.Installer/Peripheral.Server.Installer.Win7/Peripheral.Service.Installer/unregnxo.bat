
@echo Configuring System, Please wait...
regedit /s "%~dp0\unregnxo.reg"
sc stop "NexxoPeripheralService"
sc delete "NexxoPeripheralService"
@echo Successfully configured the system.
