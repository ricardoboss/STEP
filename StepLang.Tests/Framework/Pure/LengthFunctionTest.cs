using System.Diagnostics.CodeAnalysis;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework.Pure;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Tests.Framework.Pure;

public class LengthFunctionTest
{
	[Theory]
	[ClassData(typeof(LengthData))]
	public void TestInvoke(ExpressionNode value, NumberResult expected)
	{
		var interpreter = new Interpreter();
		var function = new LengthFunction();
		var result = function.Invoke(new TokenLocation(), interpreter, [value]);

		Assert.Equal(expected.Value, result.Value);
	}

	[Fact]
	public void TestEvaluateWithVariable()
	{
		var interpreter = new Interpreter();
		interpreter.CurrentScope.CreateVariable("foo", new StringResult("Hello"));

		var function = new LengthFunction();
		var result = function.Invoke(new TokenLocation(), interpreter,
			[new IdentifierExpressionNode(new Token(TokenType.Identifier, "foo"))]);

		Assert.Equal(5, result.Value);
	}

	[Fact]
	public void TestEvaluateThrowsForUnexpectedType()
	{
		var interpreter = new Interpreter();
		var function = new LengthFunction();

		_ = Assert.Throws<InvalidArgumentTypeException>(() =>
			function.Invoke(new TokenLocation(), interpreter, [LiteralExpressionNode.FromInt32(0)]));
	}

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by xUnit")]
	private sealed class LengthData : TheoryData<ExpressionNode, NumberResult>
	{
		public LengthData()
		{
			Add(LiteralExpressionNode.FromString(""), 0);
			Add(LiteralExpressionNode.FromString("Hello"), 5);

			var list = new ListExpressionNode(new Token(TokenType.OpeningSquareBracket, "["),
				new List<ExpressionNode>
				{
					LiteralExpressionNode.FromString("A"),
					LiteralExpressionNode.FromInt32(1),
					LiteralExpressionNode.FromBoolean(true),
				});

			Add(list, 3);

			var constantList = new ListExpressionNode(new Token(TokenType.OpeningSquareBracket, "["),
				new List<ExpressionNode>([
					LiteralExpressionNode.FromInt32(123),
				]));

			Add(constantList, 1);

			var map = new MapExpressionNode(new Token(TokenType.OpeningCurlyBracket, "{"),
				new Dictionary<Token, ExpressionNode>
				{
					{ new Token(TokenType.LiteralString, "\"Foo\""), LiteralExpressionNode.FromString("A") },
					{ new Token(TokenType.LiteralString, "\"Bar\""), LiteralExpressionNode.FromInt32(1) },
					{ new Token(TokenType.LiteralString, "\"Baz\""), LiteralExpressionNode.FromBoolean(true) },
					{ new Token(TokenType.LiteralString, "\"Bum\""), LiteralExpressionNode.FromString("lol") },
				});

			Add(map, 4);
		}
	}
}
