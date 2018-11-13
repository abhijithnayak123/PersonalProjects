
@echo Configuring System, Please wait...
cd "%~dp0"
net stop spooler
sc stop EPSON_Device_Control_Log_Service_TMS
sc stop EPSON_Port_Communication_Service_TMS
TM-S9000MJ_106.exe -usb -s -a nexxo es5625 -p
sc start EPSON_Port_Communication_Service_TMS
sc start EPSON_Device_Control_Log_Service_TMS
net start spooler
@echo Successfully configured the system.