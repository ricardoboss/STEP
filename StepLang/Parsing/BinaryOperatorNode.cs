using StepLang.Tokenizing;

namespace StepLang.Parsing;

public sealed record BinaryOperatorNode(IReadOnlyCollection<Token> Tokens, BinaryExpressionOperator Operator) : INode;