using StepLang.Expressions.Results;
using StepLang.Interpreting;

namespace StepLang.Tests.Interpreting;

public class InterpretationTest
{
	[Test]
	public void TestUndefinedIdentifierThrows()
	{
		const string source = "number a = b + 1";

		var exception = Assert.Throws<UndefinedIdentifierException>(() => source.Interpret());

		Assert.That(exception.ErrorCode, Is.EqualTo("INT001"));
	}

	[Test]
	public void TestInvalidArgumentCountThrows()
	{
		const string source = """
		                      function add = (number a, number b) {
		                          return a + b
		                      }

		                      number result = add(1, 2, 3)
		                      """;

		var exception = Assert.Throws<InvalidArgumentCountException>(() => source.Interpret());

		Assert.That(exception.ErrorCode, Is.EqualTo("INT002"));
	}

	[Test]
	public void TestInvalidExpressionTypeThrows()
	{
		const string source = "toTypeName(1)";

		var exception = Assert.Throws<InvalidExpressionTypeException>(() => source.Interpret());

		Assert.That(exception.ErrorCode, Is.EqualTo("INT004"));
	}

	[Test]
	public void TestListIndexOutOfBoundsThrows()
	{
		const string source = """
		                      list k = [1, 2, 3]
		                      doRemoveAt(k, 3)
		                      """;

		var exception = Assert.Throws<IndexOutOfBoundsException>(() => source.Interpret());

		Assert.That(exception.ErrorCode, Is.EqualTo("INT005"));
	}

	[Test]
	public void TestInvalidValueAssignmentThrows()
	{
		const string source = "number a = \"Hello\"";

		var exception = Assert.Throws<NonNullableVariableAssignmentException>(() => source.Interpret());

		Assert.That(exception.ErrorCode, Is.EqualTo("INT007"));
	}

	[Test]
	public void TestAddWithBoolsThrowsException()
	{
		const string source = "string a = true + false";

		var exception = Assert.Throws<IncompatibleExpressionOperandsException>(() => source.Interpret());

		Assert.That(exception.ErrorCode, Is.EqualTo("TYP002"));
	}

	[Test]
	public void TestInvalidArgumentTypeThrows()
	{
		const string source = """
		                      function add = (number a, number b) {
		                          return a + b
		                      }

		                      _ = add(1, "2")
		                      """;

		var exception = Assert.Throws<NonNullableVariableAssignmentException>(() => source.Interpret());

		Assert.That(exception.ErrorCode, Is.EqualTo("INT007"));
	}

	[Test]
	public void TestInvalidResultTypeThrows()
	{
		const string source = """
		                      function add = (number a, number b) {
		                          return a + b
		                      }

		                      add(1, 2)
		                      """;

		var exception = Assert.Throws<InvalidResultTypeException>(() => source.Interpret());

		Assert.That(exception.ErrorCode, Is.EqualTo("TYP004"));
	}

	[Test]
	public void TestThrowsWhenInterpretingErrorImport()
	{
		const string source = "import 123";

		var exception = Assert.Throws<NotSupportedException>(() => source.Interpret());

		Assert.That(exception.Message, Is.EqualTo("Cannot interpret imports with errors"));
	}

	[Test]
	public void TestThrowsWhenInterpretingErrorVariableDeclaration()
	{
		const string source = "number = 1";

		var exception = Assert.Throws<NotSupportedException>(() => source.Interpret());

		Assert.That(exception.Message, Is.EqualTo("Cannot evaluate error variable declaration node"));
	}

	[Test]
	public void TestThrowsWhenInterpretingErrorExpression()
	{
		const string source = "number a =";

		var exception = Assert.Throws<NotSupportedException>(() => source.Interpret());

		Assert.That(exception.Message, Is.EqualTo("Error expression nodes cannot be interpreted"));
	}

	[Test]
	public void TestThrowsWhenInterpretingErrorStatement()
	{
		const string source = "doSomething";

		var exception = Assert.Throws<NotSupportedException>(() => source.Interpret());

		Assert.That(exception.Message, Is.EqualTo("Error statement nodes cannot be interpreted"));
	}
}
