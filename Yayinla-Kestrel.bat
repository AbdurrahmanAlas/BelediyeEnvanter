@echo off
title Belediye Envanter - Yayin
cd /d "%~dp0"
if not exist "Yayin" mkdir "Yayin"
dotnet publish -c Release -o "%~dp0Yayin"
echo.
echo Yayin tamamlandi. Yayin klasorundeki BelediyeEnvanter.dll dosyasini calistirabilirsiniz.
pause
