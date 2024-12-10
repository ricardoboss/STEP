using StepLang.Expressions.Results;
using StepLang.Interpreting;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Tests.Interpreting;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes")]
public class InterpretationTest
{
	[Fact]
	public void TestUndefinedIdentifierThrows()
	{
		const string source = "number a = b + 1";

		var exception = Assert.Throws<UndefinedIdentifierException>(() => source.Interpret());

		Assert.Equal("INT001", exception.ErrorCode);
	}

	[Fact]
	public void TestInvalidArgumentCountThrows()
	{
		const string source = """
		                      function add = (number a, number b) {
		                          return a + b
		                      }

		                      number result = add(1, 2, 3)
		                      """;

		var exception = Assert.Throws<InvalidArgumentCountException>(() => source.Interpret());

		Assert.Equal("INT002", exception.ErrorCode);
	}

	[Fact]
	public void TestInvalidExpressionTypeThrows()
	{
		const string source = "toTypeName(1)";

		var exception = Assert.Throws<InvalidExpressionTypeException>(() => source.Interpret());

		Assert.Equal("INT004", exception.ErrorCode);
	}

	[Fact]
	public void TestListIndexOutOfBoundsThrows()
	{
		const string source = """
		                      list k = [1, 2, 3]
		                      doRemoveAt(k, 3)
		                      """;

		var exception = Assert.Throws<IndexOutOfBoundsException>(() => source.Interpret());

		Assert.Equal("INT005", exception.ErrorCode);
	}

	[Fact]
	public void TestInvalidValueAssignmentThrows()
	{
		const string source = "number a = \"Hello\"";

		var exception = Assert.Throws<NonNullableVariableAssignmentException>(() => source.Interpret());

		Assert.Equal("INT007", exception.ErrorCode);
	}

	[Fact]
	public void TestAddWithBoolsThrowsException()
	{
		const string source = "string a = true + false";

		var exception = Assert.Throws<IncompatibleExpressionOperandsException>(() => source.Interpret());

		Assert.Equal("TYP002", exception.ErrorCode);
	}

	[Fact]
	public void TestInvalidArgumentTypeThrows()
	{
		const string source = """
		                      function add = (number a, number b) {
		                          return a + b
		                      }

		                      _ = add(1, "2")
		                      """;

		var exception = Assert.Throws<NonNullableVariableAssignmentException>(() => source.Interpret());

		Assert.Equal("INT007", exception.ErrorCode);
	}

	[Fact]
	public void TestInvalidResultTypeThrows()
	{
		const string source = """
		                      function add = (number a, number b) {
		                          return a + b
		                      }

		                      add(1, 2)
		                      """;

		var exception = Assert.Throws<InvalidResultTypeException>(() => source.Interpret());

		Assert.Equal("TYP004", exception.ErrorCode);
	}
}
