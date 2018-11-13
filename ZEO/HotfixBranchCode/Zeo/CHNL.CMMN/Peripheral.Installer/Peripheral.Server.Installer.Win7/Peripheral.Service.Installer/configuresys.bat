
@echo Configuring System, Please wait...

icacls "%~dp0\*.reg" /grant:r "everyone":(OI)(CI)F
icacls "%~dp0\*.bat" /grant:r "everyone":(OI)(CI)F
icacls "%~dp0\*.log" /grant:r "everyone":(OI)(CI)F
icacls "%~dp0\*.txt" /grant:r "everyone":(OI)(CI)F
ping 1.1.1.1 -n 1 -w 6000 > nul
netsh http delete urlacl url=https://+:18732/Peripheral/
netsh http delete urlacl url=http://+:18731/Peripheral/
netsh http delete sslcert ipport=0.0.0.0:18732
netsh.exe http add urlacl url=https://+:18732/Peripheral/ user=Everyone
netsh.exe http add urlacl url=http://+:18731/Peripheral/ user=Everyone
netsh.exe http add sslcert ipport=0.0.0.0:18732 certhash=5cf28a3ade4c18bef8209d33f50ce79c66e85ad6 appid={00112233-4455-6677-8899-AABBCCDDEEFF}
regedit /s "%~dp0\registernxo.reg"
#sc stop "Nexxo Peripheral Service"
#sc delete "Nexxo Peripheral Service"
rmdir /S /Q "C:\Program Files (x86)\Nexxo Peripheral Service"
start /D "%~dp0" /B VCRedistInstaller.exe
@echo Successfully configured the system.
