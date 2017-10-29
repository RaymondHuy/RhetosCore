@SETLOCAL
SET Config=%1%
IF [%1] == [] SET Config=Debug\netcoreapp2.0

REM Set current working folder to folder where this script is, to ensure that the relative paths in this script are valid.
PUSHD "%~dp0"

XCOPY /Y/D/R ..\..\Source\CreateAndSetDatabase\bin\%Config%\CreateAndSetDatabase.dll bin\%Config%\publish || GOTO Error1
XCOPY /Y/D/R ..\..\Source\CreateAndSetDatabase\bin\%Config%\CreateAndSetDatabase.deps.json bin\%Config%\publish || GOTO Error1
XCOPY /Y/D/R ..\..\Source\CreateAndSetDatabase\bin\%Config%\CreateAndSetDatabase.runtimeconfig.dev.json bin\%Config%\publish || GOTO Error1
XCOPY /Y/D/R ..\..\Source\CreateAndSetDatabase\bin\%Config%\CreateAndSetDatabase.runtimeconfig.json bin\%Config%\publish || GOTO Error1
XCOPY /Y/D/R ..\..\Source\CreateAndSetDatabase\appsettings_template.json bin\%Config%\publish || GOTO Error1

XCOPY /Y/D/R ..\..\Source\DeployPackages\bin\%Config%\DeployPackages.dll bin\%Config%\publish || GOTO Error1
XCOPY /Y/D/R ..\..\Source\DeployPackages\bin\%Config%\DeployPackages.runtimeconfig.json bin\%Config%\publish || GOTO Error1
XCOPY /Y/D/R ..\..\Source\DeployPackages\bin\%Config%\DeployPackages.runtimeconfig.dev.json bin\%Config%\publish || GOTO Error1
XCOPY /Y/D/R ..\..\Source\DeployPackages\bin\%Config%\DeployPackages.deps.json bin\%Config%\publish || GOTO Error1

@POPD

@REM ================================================

@ECHO.
@ECHO %~nx0 SUCCESSFULLY COMPLETED.
@EXIT /B 0

:Error1
@POPD
:Error0
@ECHO.
@ECHO %~nx0 FAILED.
@IF /I [%2] NEQ [/NOPAUSE] @PAUSE
@EXIT /B 1
