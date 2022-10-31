# SyntaxExtensions\.IsFirst Method

[Home](../../../README.md)

**Containing Type**: [SyntaxExtensions](../README.md)

**Assembly**: Roslynator\.Core\.dll

## Overloads

| Method | Summary |
| ------ | ------- |
| [IsFirst\<TNode\>(SeparatedSyntaxList\<TNode\>, TNode)](#1292391442) | Returns true if the specified node is a first node in the list\. |
| [IsFirst\<TNode\>(SyntaxList\<TNode\>, TNode)](#1691317763) | Returns true if the specified node is a first node in the list\. |

<a id="1292391442"></a>

## IsFirst\<TNode\>\(SeparatedSyntaxList\<TNode\>, TNode\) 

  
Returns true if the specified node is a first node in the list\.

```csharp
public static bool IsFirst<TNode>(this Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> list, TNode node) where TNode : Microsoft.CodeAnalysis.SyntaxNode
```

### Type Parameters

**TNode**

### Parameters

**list** &ensp; [SeparatedSyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.separatedsyntaxlist-1)\<TNode\>

**node** &ensp; TNode

### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)

<a id="1691317763"></a>

## IsFirst\<TNode\>\(SyntaxList\<TNode\>, TNode\) 

  
Returns true if the specified node is a first node in the list\.

```csharp
public static bool IsFirst<TNode>(this Microsoft.CodeAnalysis.SyntaxList<TNode> list, TNode node) where TNode : Microsoft.CodeAnalysis.SyntaxNode
```

### Type Parameters

**TNode**

### Parameters

**list** &ensp; [SyntaxList](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxlist-1)\<TNode\>

**node** &ensp; TNode

### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)

