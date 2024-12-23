﻿// Copyright (c) .NET Foundation and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// <auto-generated>

using System;

namespace Roslynator
{
    public static partial class DiagnosticIdentifiers
    {
        public const string AddBlankLineAfterEmbeddedStatement = "RCS0001";
        public const string AddBlankLineAfterRegionDirective = "RCS0002";
        public const string AddBlankLineAfterUsingDirectiveList = "RCS0003";
        public const string AddBlankLineBeforeEndRegionDirective = "RCS0005";
        public const string AddBlankLineBeforeUsingDirectiveList = "RCS0006";
        public const string AddBlankLineBetweenAccessors = "RCS0007";
        public const string AddBlankLineBetweenClosingBraceAndNextStatement = "RCS0008";
        public const string AddBlankLineBetweenDeclarationAndDocumentationComment = "RCS0009";
        public const string AddBlankLineBetweenDeclarations = "RCS0010";
        public const string BlankLineBetweenSingleLineAccessors = "RCS0011";
        public const string AddBlankLineBetweenSingleLineDeclarations = "RCS0012";
        public const string AddBlankLineBetweenSingleLineDeclarationsOfDifferentKind = "RCS0013";
        public const string AddBlankLineBetweenSwitchSections = "RCS0014";
        public const string BlankLineBetweenUsingDirectives = "RCS0015";
        public const string PutAttributeListOnItsOwnLine = "RCS0016";
        public const string FormatAccessorBraces = "RCS0020";
        public const string FormatBlockBraces = "RCS0021";
        public const string AddNewLineAfterOpeningBraceOfEmptyBlock = "RCS0022";
        public const string FormatTypeDeclarationBraces = "RCS0023";
        public const string AddNewLineAfterSwitchLabel = "RCS0024";
        public const string PutFullAccessorOnItsOwnLine = "RCS0025";
        public const string PlaceNewLineAfterOrBeforeBinaryOperator = "RCS0027";
        public const string PlaceNewLineAfterOrBeforeConditionalOperator = "RCS0028";
        public const string PutConstructorInitializerOnItsOwnLine = "RCS0029";
        public const string PutEmbeddedStatementOnItsOwnLine = "RCS0030";
        public const string PutEnumMemberOnItsOwnLine = "RCS0031";
        public const string PlaceNewLineAfterOrBeforeArrowToken = "RCS0032";
        public const string PutStatementOnItsOwnLine = "RCS0033";
        public const string PutTypeParameterConstraintOnItsOwnLine = "RCS0034";
        public const string RemoveBlankLineBetweenSingleLineDeclarationsOfSameKind = "RCS0036";
        public const string RemoveBlankLineBetweenUsingDirectivesWithSameRootNamespace = "RCS0038";
        public const string RemoveNewLineBeforeBaseList = "RCS0039";
        public const string RemoveNewLineBetweenIfKeywordAndElseKeyword = "RCS0041";
        public const string PutAutoAccessorsOnSingleLine = "RCS0042";
        public const string FormatAccessorBracesOnSingleLineWhenExpressionIsOnSingleLine = "RCS0043";
        public const string UseCarriageReturnAndLinefeedAsNewLine = "RCS0044";
        public const string UseLinefeedAsNewLine = "RCS0045";
        public const string UseSpacesInsteadOfTab = "RCS0046";
        public const string WrapAndIndentEachNodeInList = "RCS0047";
        public const string PutInitializerOnSingleLine = "RCS0048";
        public const string AddBlankLineAfterTopComment = "RCS0049";
        public const string AddBlankLineBeforeTopDeclaration = "RCS0050";
        public const string AddOrRemoveNewLineBeforeWhileInDoStatement = "RCS0051";
        public const string PlaceNewLineAfterOrBeforeEqualsToken = "RCS0052";
        public const string FixFormattingOfList = "RCS0053";
        public const string FixFormattingOfCallChain = "RCS0054";
        public const string FixFormattingOfBinaryExpressionChain = "RCS0055";
        public const string LineIsTooLong = "RCS0056";
        public const string NormalizeWhitespaceAtBeginningOfFile = "RCS0057";
        public const string NormalizeWhitespaceAtEndOfFile = "RCS0058";
        public const string PlaceNewLineAfterOrBeforeNullConditionalOperator = "RCS0059";
        public const string BlankLineAfterFileScopedNamespaceDeclaration = "RCS0060";
        public const string BlankLineBetweenSwitchSections = "RCS0061";
        public const string RemoveUnnecessaryBlankLine = "RCS0063";
        public const string AddBracesWhenExpressionSpansOverMultipleLines = "RCS1001";
        public const string RemoveBraces = "RCS1002";
        public const string AddBracesToIfElseWhenExpressionSpansOverMultipleLines = "RCS1003";
        public const string RemoveBracesFromIfElse = "RCS1004";
        public const string SimplifyNestedUsingStatement = "RCS1005";
        public const string MergeElseWithNestedIf = "RCS1006";
        public const string AddBraces = "RCS1007";
        public const string UseExplicitTypeInsteadOfVarWhenTypeIsNotObvious = "RCS1008";
        public const string UseExplicitTypeInsteadOfVarInForEach = "RCS1009";
        public const string UseVarInsteadOfExplicitTypeWhenTypeIsObvious = "RCS1010";
        public const string UseExplicitTypeInsteadOfVarWhenTypeIsObvious = "RCS1012";
        public const string UsePredefinedType = "RCS1013";
        public const string UseExplicitlyOrImplicitlyTypedArray = "RCS1014";
        public const string UseNameOfOperator = "RCS1015";
        public const string UseBlockBodyOrExpressionBody = "RCS1016";
        public const string AddOrRemoveAccessibilityModifiers = "RCS1018";
        public const string OrderModifiers = "RCS1019";
        public const string SimplifyNullableOfT = "RCS1020";
        public const string ConvertLambdaExpressionBodyToExpressionBody = "RCS1021";
        public const string RemoveUnnecessaryBracesInSwitchSection = "RCS1031";
        public const string RemoveRedundantParentheses = "RCS1032";
        public const string RemoveRedundantBooleanLiteral = "RCS1033";
        public const string RemoveRedundantSealedModifier = "RCS1034";
        public const string RemoveRedundantCommaInInitializer = "RCS1035";
        public const string Obsolete_RemoveUnnecessaryBlankLine = "RCS1036";
        public const string RemoveTrailingWhitespace = "RCS1037";
        public const string RemoveEmptyStatement = "RCS1038";
        public const string RemoveArgumentListFromAttribute = "RCS1039";
        public const string RemoveEmptyElseClause = "RCS1040";
        public const string RemoveEmptyInitializer = "RCS1041";
        public const string RemoveEnumDefaultUnderlyingType = "RCS1042";
        public const string RemovePartialModifierFromTypeWithSinglePart = "RCS1043";
        public const string RemoveOriginalExceptionFromThrowStatement = "RCS1044";
        public const string AsynchronousMethodNameShouldEndWithAsync = "RCS1046";
        public const string NonAsynchronousMethodNameShouldNotEndWithAsync = "RCS1047";
        public const string UseLambdaExpressionInsteadOfAnonymousMethod = "RCS1048";
        public const string SimplifyBooleanComparison = "RCS1049";
        public const string IncludeParenthesesWhenCreatingNewObject = "RCS1050";
        public const string AddOrRemoveParenthesesFromConditionInConditionalOperator = "RCS1051";
        public const string DeclareEachAttributeSeparately = "RCS1052";
        public const string UnnecessarySemicolonAtEndOfDeclaration = "RCS1055";
        public const string AvoidUsageOfUsingAliasDirective = "RCS1056";
        public const string UseCompoundAssignment = "RCS1058";
        public const string AvoidLockingOnPubliclyAccessibleInstance = "RCS1059";
        public const string DeclareEachTypeInSeparateFile = "RCS1060";
        public const string MergeIfWithNestedIf = "RCS1061";
        public const string AvoidUsageOfDoStatementToCreateInfiniteLoop = "RCS1063";
        public const string AvoidUsageOfForStatementToCreateInfiniteLoop = "RCS1064";
        public const string AvoidUsageOfWhileStatementToCreateInfiniteLoop = "RCS1065";
        public const string RemoveEmptyFinallyClause = "RCS1066";
        public const string SimplifyLogicalNegation = "RCS1068";
        public const string RemoveUnnecessaryCaseLabel = "RCS1069";
        public const string RemoveRedundantDefaultSwitchSection = "RCS1070";
        public const string RemoveRedundantBaseConstructorCall = "RCS1071";
        public const string RemoveEmptyNamespaceDeclaration = "RCS1072";
        public const string ConvertIfToReturnStatement = "RCS1073";
        public const string RemoveRedundantConstructor = "RCS1074";
        public const string AvoidEmptyCatchClauseThatCatchesSystemException = "RCS1075";
        public const string OptimizeLinqMethodCall = "RCS1077";
        public const string UseEmptyStringLiteralOrStringEmpty = "RCS1078";
        public const string ThrowingOfNewNotImplementedException = "RCS1079";
        public const string UseCountOrLengthPropertyInsteadOfAnyMethod = "RCS1080";
        public const string SplitVariableDeclaration = "RCS1081";
        public const string UseCoalesceExpressionInsteadOfConditionalExpression = "RCS1084";
        public const string UseAutoProperty = "RCS1085";
        public const string UseUnaryOperatorInsteadOfAssignment = "RCS1089";
        public const string ConfigureAwait = "RCS1090";
        public const string RemoveEmptyRegion = "RCS1091";
        public const string FileContainsNoCode = "RCS1093";
        public const string DeclareUsingDirectiveOnTopLevel = "RCS1094";
        public const string UseHasFlagMethodOrBitwiseOperator = "RCS1096";
        public const string RemoveRedundantToStringCall = "RCS1097";
        public const string ConstantValuesShouldBePlacedOnRightSideOfComparisons = "RCS1098";
        public const string DefaultLabelShouldBeLastLabelInSwitchSection = "RCS1099";
        public const string FormatDocumentationSummaryOnSingleLine = "RCS1100";
        public const string FormatDocumentationSummaryOnMultipleLines = "RCS1101";
        public const string MakeClassStatic = "RCS1102";
        public const string ConvertIfToAssignment = "RCS1103";
        public const string SimplifyConditionalExpression = "RCS1104";
        public const string UnnecessaryInterpolation = "RCS1105";
        public const string RemoveEmptyDestructor = "RCS1106";
        public const string RemoveRedundantStringToCharArrayCall = "RCS1107";
        public const string AddStaticModifierToAllPartialClassDeclarations = "RCS1108";
        public const string DeclareTypeInsideNamespace = "RCS1110";
        public const string AddBracesToSwitchSectionWithMultipleStatements = "RCS1111";
        public const string CombineEnumerableWhereMethodChain = "RCS1112";
        public const string UseStringIsNullOrEmptyMethod = "RCS1113";
        public const string RemoveRedundantDelegateCreation = "RCS1114";
        public const string MarkLocalVariableAsConst = "RCS1118";
        public const string AddParenthesesWhenNecessary = "RCS1123";
        public const string InlineLocalVariable = "RCS1124";
        public const string AddBracesToIfElse = "RCS1126";
        public const string UseCoalesceExpression = "RCS1128";
        public const string RemoveRedundantFieldInitialization = "RCS1129";
        public const string BitwiseOperationOnEnumWithoutFlagsAttribute = "RCS1130";
        public const string RemoveRedundantOverridingMember = "RCS1132";
        public const string RemoveRedundantDisposeOrCloseCall = "RCS1133";
        public const string RemoveRedundantStatement = "RCS1134";
        public const string DeclareEnumMemberWithZeroValue = "RCS1135";
        public const string MergeSwitchSectionsWithEquivalentContent = "RCS1136";
        public const string AddSummaryToDocumentationComment = "RCS1138";
        public const string AddSummaryElementToDocumentationComment = "RCS1139";
        public const string AddExceptionToDocumentationComment = "RCS1140";
        public const string AddParamElementToDocumentationComment = "RCS1141";
        public const string AddTypeParamElementToDocumentationComment = "RCS1142";
        public const string SimplifyCoalesceExpression = "RCS1143";
        public const string RemoveRedundantAsOperator = "RCS1145";
        public const string UseConditionalAccess = "RCS1146";
        public const string RemoveRedundantCast = "RCS1151";
        public const string SortEnumMembers = "RCS1154";
        public const string UseStringComparison = "RCS1155";
        public const string UseStringLengthInsteadOfComparisonWithEmptyString = "RCS1156";
        public const string CompositeEnumValueContainsUndefinedFlag = "RCS1157";
        public const string StaticMemberInGenericTypeShouldUseTypeParameter = "RCS1158";
        public const string UseGenericEventHandler = "RCS1159";
        public const string AbstractTypeShouldNotHavePublicConstructors = "RCS1160";
        public const string EnumShouldDeclareExplicitValues = "RCS1161";
        public const string AvoidChainOfAssignments = "RCS1162";
        public const string UnusedParameter = "RCS1163";
        public const string UnusedTypeParameter = "RCS1164";
        public const string UnconstrainedTypeParameterCheckedForNull = "RCS1165";
        public const string ValueTypeObjectIsNeverEqualToNull = "RCS1166";
        public const string ParameterNameDiffersFromBase = "RCS1168";
        public const string MakeFieldReadOnly = "RCS1169";
        public const string UseReadOnlyAutoProperty = "RCS1170";
        public const string SimplifyLazyInitialization = "RCS1171";
        public const string UseIsOperatorInsteadOfAsOperator = "RCS1172";
        public const string UseCoalesceExpressionInsteadOfIf = "RCS1173";
        public const string RemoveRedundantAsyncAwait = "RCS1174";
        public const string UnusedThisParameter = "RCS1175";
        public const string UseVarInsteadOfExplicitTypeWhenTypeIsNotObvious = "RCS1176";
        public const string UseVarInsteadOfExplicitTypeInForEach = "RCS1177";
        public const string UnnecessaryAssignment = "RCS1179";
        public const string InlineLazyInitialization = "RCS1180";
        public const string ConvertCommentToDocumentationComment = "RCS1181";
        public const string RemoveRedundantBaseInterface = "RCS1182";
        public const string UseRegexInstanceInsteadOfStaticMethod = "RCS1186";
        public const string UseConstantInsteadOfField = "RCS1187";
        public const string RemoveRedundantAutoPropertyInitialization = "RCS1188";
        public const string AddOrRemoveRegionName = "RCS1189";
        public const string JoinStringExpressions = "RCS1190";
        public const string DeclareEnumValueAsCombinationOfNames = "RCS1191";
        public const string UnnecessaryUsageOfVerbatimStringLiteral = "RCS1192";
        public const string OverridingMemberShouldNotChangeParamsModifier = "RCS1193";
        public const string ImplementExceptionConstructors = "RCS1194";
        public const string UseExclusiveOrOperator = "RCS1195";
        public const string CallExtensionMethodAsInstanceMethod = "RCS1196";
        public const string OptimizeStringBuilderAppendCall = "RCS1197";
        public const string AvoidBoxingOfValueType = "RCS1198";
        public const string UnnecessaryNullCheck = "RCS1199";
        public const string CallThenByInsteadOfOrderBy = "RCS1200";
        public const string UseMethodChaining = "RCS1201";
        public const string AvoidNullReferenceException = "RCS1202";
        public const string UseAttributeUsageAttribute = "RCS1203";
        public const string UseEventArgsEmpty = "RCS1204";
        public const string OrderNamedArguments = "RCS1205";
        public const string UseConditionalAccessInsteadOfConditionalExpression = "RCS1206";
        public const string UseAnonymousFunctionOrMethodGroup = "RCS1207";
        public const string ReduceIfNesting = "RCS1208";
        public const string OrderTypeParameterConstraints = "RCS1209";
        public const string ReturnCompletedTaskInsteadOfNull = "RCS1210";
        public const string RemoveUnnecessaryElse = "RCS1211";
        public const string RemoveRedundantAssignment = "RCS1212";
        public const string RemoveUnusedMemberDeclaration = "RCS1213";
        public const string UnnecessaryInterpolatedString = "RCS1214";
        public const string ExpressionIsAlwaysEqualToTrueOrFalse = "RCS1215";
        public const string UnnecessaryUnsafeContext = "RCS1216";
        public const string ConvertInterpolatedStringToConcatenation = "RCS1217";
        public const string SimplifyCodeBranching = "RCS1218";
        public const string UsePatternMatchingInsteadOfIsAndCast = "RCS1220";
        public const string UsePatternMatchingInsteadOfAsAndNullCheck = "RCS1221";
        public const string MergePreprocessorDirectives = "RCS1222";
        public const string MarkTypeWithDebuggerDisplayAttribute = "RCS1223";
        public const string MakeMethodExtensionMethod = "RCS1224";
        public const string MakeClassSealed = "RCS1225";
        public const string AddParagraphToDocumentationComment = "RCS1226";
        public const string ValidateArgumentsCorrectly = "RCS1227";
        public const string UnusedElementInDocumentationComment = "RCS1228";
        public const string UseAsyncAwait = "RCS1229";
        public const string UnnecessaryExplicitUseOfEnumerator = "RCS1230";
        public const string MakeParameterRefReadOnly = "RCS1231";
        public const string OrderElementsInDocumentationComment = "RCS1232";
        public const string UseShortCircuitingOperator = "RCS1233";
        public const string DuplicateEnumValue = "RCS1234";
        public const string OptimizeMethodCall = "RCS1235";
        public const string UseExceptionFilter = "RCS1236";
        public const string UseBitShiftOperator = "RCS1237";
        public const string AvoidNestedConditionalOperators = "RCS1238";
        public const string UseForStatementInsteadOfWhileStatement = "RCS1239";
        public const string UnnecessaryOperator = "RCS1240";
        public const string ImplementNonGenericCounterpart = "RCS1241";
        public const string DoNotPassNonReadOnlyStructByReadOnlyReference = "RCS1242";
        public const string DuplicateWordInComment = "RCS1243";
        public const string SimplifyDefaultExpression = "RCS1244";
        public const string UseElementAccess = "RCS1246";
        public const string FixDocumentationCommentTag = "RCS1247";
        public const string NormalizeNullCheck = "RCS1248";
        public const string UnnecessaryNullForgivingOperator = "RCS1249";
        public const string UseImplicitOrExplicitObjectCreation = "RCS1250";
        public const string RemoveUnnecessaryBraces = "RCS1251";
        public const string NormalizeUsageOfInfiniteLoop = "RCS1252";
        public const string FormatDocumentationCommentSummary = "RCS1253";
        public const string NormalizeFormatOfEnumFlagValue = "RCS1254";
        public const string SimplifyArgumentNullCheck = "RCS1255";
        public const string InvalidArgumentNullCheck = "RCS1256";
        public const string UseEnumFieldExplicitly = "RCS1257";
        public const string UnnecessaryEnumFlag = "RCS1258";
        public const string RemoveEmptySyntax = "RCS1259";
        public const string AddOrRemoveTrailingComma = "RCS1260";
        public const string DisposeResourceAsynchronously = "RCS1261";
        public const string UnnecessaryRawStringLiteral = "RCS1262";
        public const string InvalidReferenceInDocumentationComment = "RCS1263";
        public const string UseVarOrExplicitType = "RCS1264";
        public const string RemoveRedundantCatchBlock = "RCS1265";
        public const string UseRawStringLiteral = "RCS1266";
        public const string UseStringInterpolationInsteadOfStringConcat = "RCS1267";
        public const string SimplifyNumericComparison = "RCS1268";
    }
}