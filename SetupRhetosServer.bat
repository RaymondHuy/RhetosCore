SET sqlServer=%1%
SET databaseName=%2%
ECHO %sqlServer%
CD RhetosCore\Source\Rhetos\bin\Debug\netcoreapp2.0\publish
dotnet CreateAndSetDatabase.dll %sqlServer% %databaseName%
dotnet DeployPackage.dll