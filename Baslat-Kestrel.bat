@echo off
title Belediye Envanter - Kestrel
cd /d "%~dp0"
echo Belediye Envanter baslatiliyor...
echo Ag adresi: http://BILGISAYAR-IP-ADRESI:5000
dotnet run --urls "http://0.0.0.0:5000"
pause
