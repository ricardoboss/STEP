using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;
using System.CodeDom.Compiler;
using System.Globalization;

namespace StepLang.Framework.Conversion;

public class ToStringFunction : GenericFunction<ExpressionResult, ExpressionResult>
{
	public const string Identifier = "toString";

	protected override IEnumerable<NativeParameter> NativeParameters =>
	[
		new(AnyValueType, "value"),
		new(NullableBool, "pretty", LiteralExpressionNode.Null),
	];

	protected override IEnumerable<ResultType> ReturnTypes => OnlyString;

	protected override StringResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		ExpressionResult argument1, ExpressionResult argument2)
	{
		return Render(argument1, argument2.IsTruthy());
	}

	internal static string Render(ExpressionResult result, bool pretty = false)
	{
		using var writer = new StringWriter();

		if (pretty)
		{
			using var indentWriter = new IndentedTextWriter(writer, "  ");

			Render(indentWriter, result);
		}
		else
		{
			Render(writer, result);
		}

		return writer.ToString();
	}

	private static void Render(TextWriter writer, ExpressionResult result)
	{
		switch (result)
		{
			case StringResult { Value: var stringValue }:
				writer.Write(stringValue);
				break;
			case NumberResult { Value: var numberValue }:
				writer.Write(numberValue.ToString(CultureInfo.InvariantCulture));
				break;
			case BoolResult { Value: var boolValue }:
				writer.Write(boolValue.ToString());
				break;
			case NullResult:
				writer.Write("null");
				break;
			case VoidResult:
				writer.Write("void");
				break;
			case FunctionResult:
				writer.Write("function");
				break;
			case ListResult list:
				RenderList(writer, list);
				break;
			case MapResult map:
				RenderMap(writer, map);
				break;
			default:
				throw new NotSupportedException("Unknown expression result type");
		}
	}

	private static void RenderList(TextWriter writer, ListResult result)
	{
		writer.Write("[");

		if (writer is IndentedTextWriter openingWriter)
		{
			openingWriter.WriteLine();
			openingWriter.Indent++;
		}

		for (var index = 0; index < result.Value.Count; index++)
		{
			var value = result.Value[index];
			var isLast = index == result.Value.Count - 1;

			if (value is StringResult { Value: { } stringValue })
			{
				RenderQuoted(writer, stringValue);
			}
			else
			{
				Render(writer, value);
			}

			if (!isLast)
			{
				writer.Write(',');
			}

			if (writer is IndentedTextWriter)
			{
				writer.WriteLine();
			}
			else if (!isLast)
			{
				writer.Write(' ');
			}
		}

		if (writer is IndentedTextWriter closingWriter)
		{
			closingWriter.Indent--;
		}

		writer.Write("]");
	}

	private static void RenderMap(TextWriter writer, MapResult result)
	{
		writer.Write("{");

		if (writer is IndentedTextWriter openingWriter)
		{
			openingWriter.WriteLine();
			openingWriter.Indent++;
		}

		var keysArray = result.Value.Keys.ToArray();
		for (var index = 0; index < keysArray.Length; index++)
		{
			var key = keysArray[index];
			var value = result.Value[key];
			var isLast = index == keysArray.Length - 1;

			RenderQuoted(writer, key);
			writer.Write(": ");

			if (value is StringResult { Value: { } stringValue })
			{
				RenderQuoted(writer, stringValue);
			}
			else
			{
				Render(writer, value);
			}

			if (!isLast)
			{
				writer.Write(",");
			}

			if (writer is IndentedTextWriter)
			{
				writer.WriteLine();
			}
			else if (!isLast)
			{
				writer.Write(' ');
			}
		}

		if (writer is IndentedTextWriter closingWriter)
		{
			closingWriter.Indent--;
		}

		writer.Write("}");
	}

	private static void RenderQuoted(TextWriter writer, string value)
	{
		writer.Write('\"');
		writer.Write(value);
		writer.Write('\"');
	}
}
