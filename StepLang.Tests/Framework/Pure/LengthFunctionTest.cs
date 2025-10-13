using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework.Pure;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tests.Framework.Pure;

public class LengthFunctionTest
{
	[TestCaseSource(typeof(LengthData))]
	public void TestInvoke(ExpressionNode value, NumberResult expected)
	{
		var interpreter = new Interpreter();
		var function = new LengthFunction();
		var result = function.Invoke(new TokenLocation(), interpreter, [value]);

		Assert.That(result.Value, Is.EqualTo(expected.Value));
	}

	[Test]
	public void TestEvaluateWithVariable()
	{
		var interpreter = new Interpreter();
		interpreter.CurrentScope.CreateVariable("foo", new StringResult("Hello"));

		var function = new LengthFunction();
		var result = function.Invoke(new TokenLocation(), interpreter,
			[new IdentifierExpressionNode(new Token(TokenType.Identifier, "foo"))]);

		Assert.That(result.Value, Is.EqualTo(5));
	}

	[Test]
	public void TestEvaluateThrowsForUnexpectedType()
	{
		var interpreter = new Interpreter();
		var function = new LengthFunction();

		_ = Assert.Throws<InvalidArgumentTypeException>(() =>
			function.Invoke(new TokenLocation(), interpreter, [LiteralExpressionNode.FromInt32(0)]));
	}

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by NUnit")]
	private sealed class LengthData : IEnumerable<TestCaseData>
	{
		public IEnumerator<TestCaseData> GetEnumerator()
		{
			yield return new TestCaseData(LiteralExpressionNode.FromString(""), NumberResult.Zero);
			yield return new TestCaseData(LiteralExpressionNode.FromString("Hello"), NumberResult.FromInt32(5));

			var list = new ListExpressionNode(new Token(TokenType.OpeningSquareBracket, "["),
					new List<ExpressionNode>
					{
										LiteralExpressionNode.FromString("A"),
										LiteralExpressionNode.FromInt32(1),
										LiteralExpressionNode.FromBoolean(true),
					});

			yield return new TestCaseData(list, NumberResult.FromInt32(3));

			var constantList = new ListExpressionNode(new Token(TokenType.OpeningSquareBracket, "["),
					new List<ExpressionNode>([
							LiteralExpressionNode.FromInt32(123),
					]));

			yield return new TestCaseData(constantList, NumberResult.FromInt32(1));

			var map = new MapExpressionNode(new Token(TokenType.OpeningCurlyBracket, "{"),
					new Dictionary<Token, ExpressionNode>
					{
										{ new Token(TokenType.LiteralString, "\"Foo\""), LiteralExpressionNode.FromString("A") },
										{ new Token(TokenType.LiteralString, "\"Bar\""), LiteralExpressionNode.FromInt32(1) },
										{ new Token(TokenType.LiteralString, "\"Baz\""), LiteralExpressionNode.FromBoolean(true) },
										{ new Token(TokenType.LiteralString, "\"Bum\""), LiteralExpressionNode.FromString("lol") },
					});

			yield return new TestCaseData(map, NumberResult.FromInt32(4));
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
