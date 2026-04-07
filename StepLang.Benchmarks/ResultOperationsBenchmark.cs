using BenchmarkDotNet.Attributes;
using StepLang.Expressions.Results;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.Benchmarks;

/// <summary>
/// Micro-benchmarks for runtime value type operations that are on the interpreter hot path.
/// These target specific allocation and computation patterns in the expression result types.
/// </summary>
[SuppressMessage("Maintainability", "CA1515:Consider making public types internal")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
[MemoryDiagnoser]
public class ResultOperationsBenchmark
{
	private NumberResult numberA = null!;
	private NumberResult numberB = null!;
	private BoolResult boolA = null!;
	private BoolResult boolB = null!;
	private StringResult stringA = null!;
	private StringResult stringB = null!;
	private StringResult stringC = null!;

	[GlobalSetup]
	public void Setup()
	{
		numberA = new NumberResult(42);
		numberB = new NumberResult(7);
		boolA = BoolResult.True;
		boolB = BoolResult.False;
		stringA = new StringResult("hello");
		stringB = new StringResult("world");
		stringC = new StringResult("hello");
	}

	[Benchmark]
	public NumberResult NumberAddition()
	{
		return numberA + numberB;
	}

	[Benchmark]
	public NumberResult NumberSubtraction()
	{
		return numberA - numberB;
	}

	[Benchmark]
	public NumberResult NumberMultiplication()
	{
		return numberA * numberB;
	}

	[Benchmark]
	public BoolResult NumberEquality()
	{
		return numberA == numberB;
	}

	[Benchmark]
	public BoolResult NumberLessThan()
	{
		return numberA < numberB;
	}

	[Benchmark]
	public BoolResult NumberGreaterThan()
	{
		return numberA > numberB;
	}

	[Benchmark]
	public BoolResult BoolEquality()
	{
		return boolA == boolB;
	}

	[Benchmark]
	public BoolResult BoolInequality()
	{
		return boolA != boolB;
	}

	[Benchmark]
	public BoolResult BoolNot()
	{
		return !boolA;
	}

	[Benchmark]
	public BoolResult StringEqualitySame()
	{
		return stringA == stringC;
	}

	[Benchmark]
	public BoolResult StringEqualityDifferent()
	{
		return stringA == stringB;
	}

	[Benchmark]
	public BoolResult StringInequality()
	{
		return stringA != stringB;
	}

	[Benchmark]
	public StringResult StringConcatenation()
	{
		return stringA + stringB;
	}

	[Benchmark]
	public NumberResult NumberDeepClone()
	{
		return numberA.DeepClone();
	}

	[Benchmark]
	public BoolResult BoolDeepClone()
	{
		return boolA.DeepClone();
	}

	[Benchmark]
	public StringResult StringDeepClone()
	{
		return stringA.DeepClone();
	}

	/// <summary>
	/// Measures the cost of accessing NumberResult.Zero (static readonly field vs property).
	/// </summary>
	[Benchmark]
	public NumberResult NumberZeroAccess()
	{
		return NumberResult.Zero;
	}

	/// <summary>
	/// Measures the cost of accessing StringResult.Empty (static readonly field vs property).
	/// </summary>
	[Benchmark]
	public StringResult StringEmptyAccess()
	{
		return StringResult.Empty;
	}

	/// <summary>
	/// Simulates a tight arithmetic loop as the interpreter would execute it.
	/// Each iteration allocates a new NumberResult for the sum and loop counter.
	/// </summary>
	[Benchmark]
	public NumberResult ArithmeticLoopSimulation()
	{
		var sum = new NumberResult(0);
		var one = new NumberResult(1);
		var two = new NumberResult(2);
		var limit = new NumberResult(100);

		var i = new NumberResult(0);
		while (i < limit)
		{
			sum = sum + i * two - one;
			i = i + one;
		}

		return sum;
	}

	/// <summary>
	/// Simulates a tight comparison loop as the interpreter would execute it.
	/// Each iteration allocates BoolResult instances for comparisons.
	/// </summary>
	[Benchmark]
	public NumberResult ComparisonLoopSimulation()
	{
		var count = new NumberResult(0);
		var one = new NumberResult(1);
		var limit = new NumberResult(100);
		var threshold = new NumberResult(50);

		var i = new NumberResult(0);
		while (i < limit)
		{
			if (i > threshold)
			{
				count = count + one;
			}

			if (i == threshold)
			{
				count = count + one;
			}

			i = i + one;
		}

		return count;
	}
}
