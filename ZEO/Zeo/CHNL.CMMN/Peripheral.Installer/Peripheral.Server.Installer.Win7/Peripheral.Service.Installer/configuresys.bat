
@echo Configuring System, Please wait...

icacls "%~dp0\*.reg" /grant:r "everyone":(OI)(CI)F
icacls "%~dp0\*.bat" /grant:r "everyone":(OI)(CI)F
icacls "%~dp0\*.log" /grant:r "everyone":(OI)(CI)F
icacls "%~dp0\*.txt" /grant:r "everyone":(OI)(CI)F
ping 1.1.1.1 -n 1 -w 6000 > nul
netsh http delete urlacl url=https://+:18732/Peripheral/
netsh http delete urlacl url=https://+:18731/Peripheral/
netsh http delete urlacl url=http://+:18732/Peripheral/
netsh http delete urlacl url=http://+:18731/Peripheral/
#delete ssl cert bindings
netsh http delete sslcert ipport=0.0.0.0:18732
netsh http delete sslcert ipport=0.0.0.0:18731
netsh http delete sslcert ipport=127.0.0.1:18732
netsh http delete sslcert ipport=127.0.0.1:18731
#add url reservation and ssl cert to port 18731
netsh.exe http add urlacl url=https://+:18731/Peripheral/ user=Everyone
netsh.exe http add sslcert ipport=0.0.0.0:18731 certhash=6be13aad94263813906fe9676f41ef89b37580f4 appid={00112233-4455-6677-8899-AABBCCDDEEFF}
regedit /s "%~dp0\registernxo.reg"
#sc stop "ZEO Peripheral Service"
#sc delete "ZEO Peripheral Service"
rmdir /S /Q "C:\Program Files (x86)\Peripheral Service"
start /D "%~dp0" /B VCRedistInstaller.exe
@echo Successfully configured the system.
