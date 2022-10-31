@echo off

rd /S /Q "..\src\CommandLine\bin\Debug\net6.0"

del /Q "..\src\CommandLine\bin\Debug\Roslynator.DotNet.Cli.*.nupkg"

dotnet pack "..\src\CommandLine\CommandLine.csproj" -c Debug -v normal ^
 /p:RoslynatorDotNetCli=true,Deterministic=true,TreatWarningsAsErrors=true,WarningsNotAsErrors="1591"

dotnet tool uninstall roslynator.dotnet.cli -g

dotnet tool install roslynator.dotnet.cli -g --add-source "..\src\CommandLine\bin\Debug"

echo OK
pause
