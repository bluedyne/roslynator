#dotnet tool install -g roslynator.dotnet.cli

roslynator find-symbol "$PSScriptRoot/../src/Roslynator.sln" `
 --symbol-kind type --unused --remove --exclude CommandLine/Orang/** --without-attribute System.ObsoleteAttribute