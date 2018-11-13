
@echo Configuring System, Please wait...
icacls "%~dp0\registernxo.reg" /grant:r "everyone":(F,RX,W)
icacls "%~dp0\unregnxo.reg" /grant:r "everyone":(F,RX,W)
icacls "%~dp0\unregnxo.bat" /grant:r "everyone":(F,RX,W)
icacls "%~dp0\ConfigureSys.bat" /grant:r "everyone":(F,RX,W)
icacls "%~dp0\OpenOffice" /grant:r "everyone":(F,RX,W)
icacls "%~dp0\DMSDebug.log" /grant:r "everyone":(F,RX,W)
icacls "%~dp0\reset.log" /grant:r "everyone":(F,RX,W)
icacls "%~dp0\redirect.txt" /grant:r "everyone":(F,RX,W)
certutil -delstore My nps.nexxofinancial.com
"%~dp0\winhttpcertcfg.exe" -i "%~dp0\nps_certificate.pfx" -c LOCAL_MACHINE\My -a "Everyone" -p password
@echo Successfully configured the system.
ping 1.1.1.1 -n 1 -w 6000 > nul
