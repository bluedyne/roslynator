# SyntaxExtensions\.WithTriviaFrom Method

[Home](../../../README.md)

**Containing Type**: [SyntaxExtensions](../README.md)

**Assembly**: Roslynator\.Core\.dll

## Overloads

| Method | Summary |
| ------ | ------- |
| [WithTriviaFrom(SyntaxToken, SyntaxNode)](#3436644061) | Creates a new token from this token with both the leading and trailing trivia of the specified node\. |
| [WithTriviaFrom\<TNode\>(SeparatedSyntaxList\<TNode\>, SyntaxNode)](#2087578213) | Creates a new separated list with both leading and trailing trivia of the specified node\. If the list contains more than one item, first item is updated with leading trivia and last item is updated with trailing trivia\. |
| [WithTriviaFrom\<TNode\>(SyntaxList\<TNode\>, SyntaxNode)](#301376900) | Creates a new list with both leading and trailing trivia of the specified node\. If the list contains more than one item, first item is updated with leading trivia and last item is updated with trailing trivia\. |
| [WithTriviaFrom\<TNode\>(TNode, SyntaxToken)](#441639473) | Creates a new node from this node with both the leading and trailing trivia of the specified token\. |

<a id="3436644061"></a>

## WithTriviaFrom\(SyntaxToken, SyntaxNode\) 

  
Creates a new token from this token with both the leading and trailing trivia of the specified node\.

```csharp
public static Microsoft.CodeAnalysis.SyntaxToken WithTriviaFrom(this Microsoft.CodeAnalysis.SyntaxToken token, Microsoft.CodeAnalysis.SyntaxNode node)
```

### Parameters

**token** &ensp; [SyntaxToken](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxtoken)

**node** &ensp; [SyntaxNode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxnode)

### Returns

[SyntaxToken](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxtoken)

<a id="2087578213"></a>

## WithTriviaFrom\<TNode\>\(SeparatedSyntaxList\<TNode\>, SyntaxNode\) 

  
Creates a new separated list with both leading and trailing trivia of the specified node\.
If the list contains more than one item, first item is updated with leading trivia and last item is updated with trailing trivia\.

```csharp
public static Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> WithTriviaFrom<TNode>(this Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> list, Microsoft.CodeAnalysis.SyntaxNode node) where TNode : Microsoft.CodeAnalysis.SyntaxNode
```

### Type Parameters

**TNode**

### Parameters

**list** &ensp; [SeparatedSyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.separatedsyntaxlist-1)\<TNode\>

**node** &ensp; [SyntaxNode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxnode)

### Returns

[SeparatedSyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.separatedsyntaxlist-1)\<TNode\>

<a id="301376900"></a>

## WithTriviaFrom\<TNode\>\(SyntaxList\<TNode\>, SyntaxNode\) 

  
Creates a new list with both leading and trailing trivia of the specified node\.
If the list contains more than one item, first item is updated with leading trivia and last item is updated with trailing trivia\.

```csharp
public static Microsoft.CodeAnalysis.SyntaxList<TNode> WithTriviaFrom<TNode>(this Microsoft.CodeAnalysis.SyntaxList<TNode> list, Microsoft.CodeAnalysis.SyntaxNode node) where TNode : Microsoft.CodeAnalysis.SyntaxNode
```

### Type Parameters

**TNode**

### Parameters

**list** &ensp; [SyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxlist-1)\<TNode\>

**node** &ensp; [SyntaxNode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxnode)

### Returns

[SyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxlist-1)\<TNode\>

<a id="441639473"></a>

## WithTriviaFrom\<TNode\>\(TNode, SyntaxToken\) 

  
Creates a new node from this node with both the leading and trailing trivia of the specified token\.

```csharp
public static TNode WithTriviaFrom<TNode>(this TNode node, Microsoft.CodeAnalysis.SyntaxToken token) where TNode : Microsoft.CodeAnalysis.SyntaxNode
```

### Type Parameters

**TNode**

### Parameters

**node** &ensp; TNode

**token** &ensp; [SyntaxToken](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxtoken)

### Returns

TNode

