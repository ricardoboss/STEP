using StepLang.Expressions.Results;
using StepLang.Framework.Mutating;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tests.Framework.Mutating;

public class DoAddFunctionTest
{
	[TestCaseSource(typeof(DoAddData))]
	public void TestDoAdd(ListResult list, IExpressionNode elementExpression)
	{
		const string listIdentifier = "mylist";

		var interpreter = new Interpreter();
		interpreter.CurrentScope.CreateVariable(listIdentifier, list);
		var listVarExpression = new IdentifierExpressionNode(new(TokenType.Identifier, listIdentifier));
		var function = new DoAddFunction();

		var previousCount = list.Value.Count;

		var result = function.Invoke(new(), interpreter, [listVarExpression, elementExpression]);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(result, Is.SameAs(VoidResult.Instance));
			Assert.That(list.Value, Has.Count.EqualTo(previousCount + 1));
		}
	}

	[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by NUnit")]
	private sealed class DoAddData : IEnumerable<TestCaseData>
	{
		public IEnumerator<TestCaseData> GetEnumerator()
		{
			yield return new(ListResult.Empty, LiteralExpressionNode.FromString("test"));
			yield return new(ListResult.Empty, LiteralExpressionNode.FromInt32(1));
			yield return new(ListResult.Empty, LiteralExpressionNode.FromBoolean(true));
			yield return new(ListResult.Empty,
					new AddExpressionNode(new(TokenType.PlusSymbol, "+"), LiteralExpressionNode.FromInt32(1),
							LiteralExpressionNode.FromInt32(2)));

			yield return new(ListResult.From(BoolResult.True), LiteralExpressionNode.FromBoolean(false));
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
