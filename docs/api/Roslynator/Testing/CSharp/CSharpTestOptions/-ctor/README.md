# CSharpTestOptions\(CSharpCompilationOptions, CSharpParseOptions, IEnumerable\<MetadataReference\>, IEnumerable\<String\>, DiagnosticSeverity, IEnumerable\<KeyValuePair\<String, String\>\>\) Constructor

[Home](../../../../../README.md)

**Containing Type**: [CSharpTestOptions](../README.md)

**Assembly**: Roslynator\.Testing\.CSharp\.dll

  
Initializes a new instance of [CSharpTestOptions](../README.md)\.

```csharp
public CSharpTestOptions(Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions compilationOptions = null, Microsoft.CodeAnalysis.CSharp.CSharpParseOptions parseOptions = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> metadataReferences = null, System.Collections.Generic.IEnumerable<string> allowedCompilerDiagnosticIds = null, Microsoft.CodeAnalysis.DiagnosticSeverity allowedCompilerDiagnosticSeverity = Info, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> configOptions = null)
```

### Parameters

**compilationOptions** &ensp; [CSharpCompilationOptions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.csharpcompilationoptions)

**parseOptions** &ensp; [CSharpParseOptions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.csharpparseoptions)

**metadataReferences** &ensp; [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)\<[MetadataReference](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.metadatareference)\>

**allowedCompilerDiagnosticIds** &ensp; [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)\<[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)\>

**allowedCompilerDiagnosticSeverity** &ensp; [DiagnosticSeverity](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.diagnosticseverity)

**configOptions** &ensp; [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)\<[KeyValuePair](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.keyvaluepair-2)\<[String](https://docs.microsoft.com/en-us/dotnet/api/system.string), [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)\>\>