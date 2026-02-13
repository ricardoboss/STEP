using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework.Pure;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tests.Framework.Pure;

public class LengthFunctionTest
{
	[TestCaseSource(typeof(LengthData))]
	public void TestInvoke(IExpressionNode value, NumberResult expected)
	{
		var interpreter = new Interpreter();
		var function = new LengthFunction();
		var result = function.Invoke(new(), interpreter, [value]);

		Assert.That(result.Value, Is.EqualTo(expected.Value));
	}

	[Test]
	public void TestEvaluateWithVariable()
	{
		var interpreter = new Interpreter();
		interpreter.CurrentScope.CreateVariable("foo", new StringResult("Hello"));

		var function = new LengthFunction();
		var result = function.Invoke(new(), interpreter,
			[new IdentifierExpressionNode(new(TokenType.Identifier, "foo"))]);

		Assert.That(result.Value, Is.EqualTo(5));
	}

	[Test]
	public void TestEvaluateThrowsForUnexpectedType()
	{
		var interpreter = new Interpreter();
		var function = new LengthFunction();

		_ = Assert.Throws<InvalidArgumentTypeException>(() =>
			function.Invoke(new(), interpreter, [LiteralExpressionNode.FromInt32(0)]));
	}

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by NUnit")]
	private sealed class LengthData : IEnumerable<TestCaseData>
	{
		public IEnumerator<TestCaseData> GetEnumerator()
		{
			yield return new(LiteralExpressionNode.FromString(""), NumberResult.Zero);
			yield return new(LiteralExpressionNode.FromString("Hello"), NumberResult.FromInt32(5));

			var list = new ListExpressionNode(new(TokenType.OpeningSquareBracket, "["),
					new List<IExpressionNode>
					{
										LiteralExpressionNode.FromString("A"),
										LiteralExpressionNode.FromInt32(1),
										LiteralExpressionNode.FromBoolean(true),
					});

			yield return new(list, NumberResult.FromInt32(3));

			var constantList = new ListExpressionNode(new(TokenType.OpeningSquareBracket, "["),
					new List<IExpressionNode>([
							LiteralExpressionNode.FromInt32(123),
					]));

			yield return new(constantList, NumberResult.FromInt32(1));

			var map = new MapExpressionNode(new(TokenType.OpeningCurlyBracket, "{"),
					new Dictionary<Token, IExpressionNode>
					{
										{ new(TokenType.LiteralString, "\"Foo\""), LiteralExpressionNode.FromString("A") },
										{ new(TokenType.LiteralString, "\"Bar\""), LiteralExpressionNode.FromInt32(1) },
										{ new(TokenType.LiteralString, "\"Baz\""), LiteralExpressionNode.FromBoolean(true) },
										{ new(TokenType.LiteralString, "\"Bum\""), LiteralExpressionNode.FromString("lol") },
					});

			yield return new(map, NumberResult.FromInt32(4));
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
