$roslynatorExe="../src/CommandLine/bin/Debug/net7.0/roslynator"
$rootDirectoryUrl="../../docs/api/"

dotnet clean "../src/CommandLine.sln" -c Debug -v minimal /m
dotnet build "../src/CommandLine.sln" -c Debug -v minimal /m

& $roslynatorExe generate-doc "../src/Core.sln" `
 --properties Configuration=Release `
 -o "../docs/api" `
 --host github `
 --heading "Roslynator API Reference"

& $roslynatorExe list-symbols "../src/Core.sln" `
 --properties Configuration=Release `
 --visibility public `
 --depth member `
 --ignored-parts containing-namespace assembly-attributes `
 --output "../docs/api.txt"

& $roslynatorExe generate-doc-root "../src/Core.sln" `
 --properties Configuration=Release `
 --projects Core `
 -o "../src/Core/README.md" `
 --host github `
 --heading "Roslynator.Core" `
 --root-directory-url $rootDirectoryUrl

& $roslynatorExe generate-doc-root "../src/Core.sln" `
 --properties Configuration=Release `
 --projects CSharp `
 -o "../src/CSharp/README.md" `
 --host github `
 --heading "Roslynator.CSharp" `
 --root-directory-url $rootDirectoryUrl

& $roslynatorExe generate-doc-root "../src/Core.sln" `
 --properties Configuration=Release `
 --projects Workspaces.Core `
 -o "../src/Workspaces.Core/README.md" `
 --host github `
 --heading "Roslynator.CSharp.Workspaces" `
 --root-directory-url $rootDirectoryUrl

& $roslynatorExe generate-doc-root "../src/Core.sln" `
 --properties Configuration=Release `
 --projects CSharp.Workspaces `
 -o "../src/CSharp.Workspaces/README.md" `
 --host github `
 --heading "Roslynator.CSharp.Workspaces" `
 --root-directory-url $rootDirectoryUrl

& $roslynatorExe generate-doc-root "../src/Core.sln" `
 --properties Configuration=Release `
 --projects Testing.Common Testing.CSharp Testing.CSharp.Xunit Testing.CSharp.MSTest `
 -o "../src/Tests/README.md" `
 --host github `
 --heading "Roslynator Testing Framework" `
 --root-directory-url $rootDirectoryUrl

Write-Host DONE
