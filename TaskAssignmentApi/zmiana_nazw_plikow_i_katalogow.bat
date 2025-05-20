@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

set /a files_changed=0
set /a dirs_changed=0

REM Ustawienia zmiennych - zastąp "stary_tekst" i "nowy_tekst" odpowiednimi wartościami
set "old_text=AdministrationService4"
set "new_text=AdministrationService"

echo Rekurencyjne przejście przez wszystkie pliki w bieżącym katalogu i jego podfolderach
for /r %%k in (*) do (
    set "filename=%%~nxk"
    if "!filename!" neq "!filename:%old_text%=%new_text%!" (
        set "newfilename=!filename:%old_text%=%new_text%!"
        ren "%%~dpk!filename!" "!newfilename!"
        echo Zmieniono plik "!filename!" na "!newfilename!"
		set /a files_changed+=1
    )
)

echo/

echo Przejście przez wszystkie katalogi tylko w bieżącym katalogu
for /d %%i in (*) do (
    set "dirname=%%i"
    set "newdirname=!dirname:%old_text%=%new_text%!"

    if "!dirname!" neq "!newdirname!" (
        ren "%%i" "!newdirname!"
        echo Zmieniono katalog "%%i" na "!newdirname!"
		set /a dirs_changed+=1
    )
)

echo/
echo Zmieniono plików: %files_changed%
echo Zmieniono katalogów: %dirs_changed%
echo/

pause
