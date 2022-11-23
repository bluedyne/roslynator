---
sidebar_label: TrimTrivia
---

# SyntaxExtensions\.TrimTrivia Method

**Containing Type**: [SyntaxExtensions](../index.md)

**Assembly**: Roslynator\.CSharp\.dll

## Overloads

| Method | Summary |
| ------ | ------- |
<<<<<<<< HEAD:docs/api/Roslynator/CSharp/SyntaxExtensions/TrimTrivia/index.md
| [TrimTrivia(SyntaxToken)](#Roslynator_CSharp_SyntaxExtensions_TrimTrivia_Microsoft_CodeAnalysis_SyntaxToken_) | Removes all leading whitespace from the leading trivia and all trailing whitespace from the trailing trivia and returns a new token with the new trivia\. [SyntaxKind.WhitespaceTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.whitespacetrivia) and [SyntaxKind.EndOfLineTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.endoflinetrivia) is considered to be a whitespace\. Returns the same token if there is nothing to trim\. |
| [TrimTrivia&lt;TNode&gt;(SeparatedSyntaxList&lt;TNode&gt;)](#Roslynator_CSharp_SyntaxExtensions_TrimTrivia__1_Microsoft_CodeAnalysis_SeparatedSyntaxList___0__) | Removes all leading whitespace from the leading trivia of the first node in a list and all trailing whitespace from the trailing trivia of the last node in a list and returns a new list with the new trivia\. [SyntaxKind.WhitespaceTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.whitespacetrivia) and [SyntaxKind.EndOfLineTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.endoflinetrivia) is considered to be a whitespace\. |
| [TrimTrivia&lt;TNode&gt;(SyntaxList&lt;TNode&gt;)](#Roslynator_CSharp_SyntaxExtensions_TrimTrivia__1_Microsoft_CodeAnalysis_SyntaxList___0__) | Removes all leading whitespace from the leading trivia of the first node in a list and all trailing whitespace from the trailing trivia of the last node in a list and returns a new list with the new trivia\. [SyntaxKind.WhitespaceTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.whitespacetrivia) and [SyntaxKind.EndOfLineTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.endoflinetrivia) is considered to be a whitespace\. |
| [TrimTrivia&lt;TNode&gt;(TNode)](#Roslynator_CSharp_SyntaxExtensions_TrimTrivia__1___0_) | Removes all leading whitespace from the leading trivia and all trailing whitespace from the trailing trivia and returns a new node with the new trivia\. [SyntaxKind.WhitespaceTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.whitespacetrivia) and [SyntaxKind.EndOfLineTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.endoflinetrivia) is considered to be a whitespace\. Returns the same node if there is nothing to trim\. |
========
| [TrimTrivia(SyntaxToken)](#3557770056) | Removes all leading whitespace from the leading trivia and all trailing whitespace from the trailing trivia and returns a new token with the new trivia\. [SyntaxKind.WhitespaceTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.whitespacetrivia) and [SyntaxKind.EndOfLineTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.endoflinetrivia) is considered to be a whitespace\. Returns the same token if there is nothing to trim\. |
| [TrimTrivia\<TNode\>(SeparatedSyntaxList\<TNode\>)](#1776013108) | Removes all leading whitespace from the leading trivia of the first node in a list and all trailing whitespace from the trailing trivia of the last node in a list and returns a new list with the new trivia\. [SyntaxKind.WhitespaceTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.whitespacetrivia) and [SyntaxKind.EndOfLineTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.endoflinetrivia) is considered to be a whitespace\. |
| [TrimTrivia\<TNode\>(SyntaxList\<TNode\>)](#92538413) | Removes all leading whitespace from the leading trivia of the first node in a list and all trailing whitespace from the trailing trivia of the last node in a list and returns a new list with the new trivia\. [SyntaxKind.WhitespaceTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.whitespacetrivia) and [SyntaxKind.EndOfLineTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.endoflinetrivia) is considered to be a whitespace\. |
| [TrimTrivia\<TNode\>(TNode)](#3568210656) | Removes all leading whitespace from the leading trivia and all trailing whitespace from the trailing trivia and returns a new node with the new trivia\. [SyntaxKind.WhitespaceTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.whitespacetrivia) and [SyntaxKind.EndOfLineTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.endoflinetrivia) is considered to be a whitespace\. Returns the same node if there is nothing to trim\. |
>>>>>>>> main:docs/api/Roslynator/CSharp/SyntaxExtensions/TrimTrivia/README.md

<a id="3557770056"></a>

<<<<<<<< HEAD:docs/api/Roslynator/CSharp/SyntaxExtensions/TrimTrivia/index.md
========
## TrimTrivia\(SyntaxToken\) 

>>>>>>>> main:docs/api/Roslynator/CSharp/SyntaxExtensions/TrimTrivia/README.md
  
Removes all leading whitespace from the leading trivia and all trailing whitespace from the trailing trivia and returns a new token with the new trivia\.
[SyntaxKind.WhitespaceTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.whitespacetrivia) and [SyntaxKind.EndOfLineTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.endoflinetrivia) is considered to be a whitespace\.
Returns the same token if there is nothing to trim\.

```csharp
public static Microsoft.CodeAnalysis.SyntaxToken TrimTrivia(this Microsoft.CodeAnalysis.SyntaxToken token)
```

### Parameters

**token** &ensp; [SyntaxToken](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxtoken)

### Returns

[SyntaxToken](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxtoken)

<<<<<<<< HEAD:docs/api/Roslynator/CSharp/SyntaxExtensions/TrimTrivia/index.md
## TrimTrivia&lt;TNode&gt;\(SeparatedSyntaxList&lt;TNode&gt;\) <a id="Roslynator_CSharp_SyntaxExtensions_TrimTrivia__1_Microsoft_CodeAnalysis_SeparatedSyntaxList___0__"></a>

========
<a id="1776013108"></a>

## TrimTrivia\<TNode\>\(SeparatedSyntaxList\<TNode\>\) 

>>>>>>>> main:docs/api/Roslynator/CSharp/SyntaxExtensions/TrimTrivia/README.md
  
Removes all leading whitespace from the leading trivia of the first node in a list
and all trailing whitespace from the trailing trivia of the last node in a list and returns a new list with the new trivia\.
[SyntaxKind.WhitespaceTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.whitespacetrivia) and [SyntaxKind.EndOfLineTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.endoflinetrivia) is considered to be a whitespace\.

```csharp
public static Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> TrimTrivia<TNode>(this Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> list) where TNode : Microsoft.CodeAnalysis.SyntaxNode
```

### Type Parameters

**TNode**

### Parameters

<<<<<<<< HEAD:docs/api/Roslynator/CSharp/SyntaxExtensions/TrimTrivia/index.md
**list** &ensp; [SeparatedSyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.separatedsyntaxlist-1)&lt;TNode&gt;

### Returns

[SeparatedSyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.separatedsyntaxlist-1)&lt;TNode&gt;

## TrimTrivia&lt;TNode&gt;\(SyntaxList&lt;TNode&gt;\) <a id="Roslynator_CSharp_SyntaxExtensions_TrimTrivia__1_Microsoft_CodeAnalysis_SyntaxList___0__"></a>

========
**list** &ensp; [SeparatedSyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.separatedsyntaxlist-1)\<TNode\>

### Returns

[SeparatedSyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.separatedsyntaxlist-1)\<TNode\>

<a id="92538413"></a>

## TrimTrivia\<TNode\>\(SyntaxList\<TNode\>\) 

>>>>>>>> main:docs/api/Roslynator/CSharp/SyntaxExtensions/TrimTrivia/README.md
  
Removes all leading whitespace from the leading trivia of the first node in a list
and all trailing whitespace from the trailing trivia of the last node in a list and returns a new list with the new trivia\.
[SyntaxKind.WhitespaceTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.whitespacetrivia) and [SyntaxKind.EndOfLineTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.endoflinetrivia) is considered to be a whitespace\.

```csharp
public static Microsoft.CodeAnalysis.SyntaxList<TNode> TrimTrivia<TNode>(this Microsoft.CodeAnalysis.SyntaxList<TNode> list) where TNode : Microsoft.CodeAnalysis.SyntaxNode
```

### Type Parameters

**TNode**

### Parameters

<<<<<<<< HEAD:docs/api/Roslynator/CSharp/SyntaxExtensions/TrimTrivia/index.md
**list** &ensp; [SyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxlist-1)&lt;TNode&gt;

### Returns

[SyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxlist-1)&lt;TNode&gt;

## TrimTrivia&lt;TNode&gt;\(TNode\) <a id="Roslynator_CSharp_SyntaxExtensions_TrimTrivia__1___0_"></a>

========
**list** &ensp; [SyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxlist-1)\<TNode\>

### Returns

[SyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxlist-1)\<TNode\>

<a id="3568210656"></a>

## TrimTrivia\<TNode\>\(TNode\) 

>>>>>>>> main:docs/api/Roslynator/CSharp/SyntaxExtensions/TrimTrivia/README.md
  
Removes all leading whitespace from the leading trivia and all trailing whitespace from the trailing trivia and returns a new node with the new trivia\.
[SyntaxKind.WhitespaceTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.whitespacetrivia) and [SyntaxKind.EndOfLineTrivia](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind.endoflinetrivia) is considered to be a whitespace\.
Returns the same node if there is nothing to trim\.

```csharp
public static TNode TrimTrivia<TNode>(this TNode node) where TNode : Microsoft.CodeAnalysis.SyntaxNode
```

### Type Parameters

**TNode**

### Parameters

**node** &ensp; TNode

### Returns

TNode

