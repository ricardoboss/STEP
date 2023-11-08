using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework;

public abstract class GenericFunction : GenericParameterlessFunction
{
    protected abstract ExpressionResult Invoke(Interpreter interpreter);

    protected override IEnumerable<NativeParameter> NativeParameters { get; } = Enumerable.Empty<NativeParameter>();

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        if (arguments.Count > 0)
            throw new InvalidArgumentCountException(0, arguments.Count);

        return Invoke(interpreter);
    }
}

public abstract class GenericFunction<T1> : GenericOneParameterFunction where T1 : ExpressionResult
{
    protected abstract ExpressionResult Invoke(Interpreter interpreter, T1 argument1);

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        var requiredCount = GetRequiredCount();

        if (arguments.Count < requiredCount)
            throw new InvalidArgumentCountException(requiredCount, arguments.Count);

        var argument1 = GetArgument<T1>(0, interpreter, arguments);

        return Invoke(interpreter, argument1);
    }
}

public abstract class GenericFunction<T1, T2> : GenericTwoParameterFunction
    where T1 : ExpressionResult
    where T2 : ExpressionResult
{
    protected abstract ExpressionResult Invoke(Interpreter interpreter, T1 argument1, T2 argument2);

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        var requiredCount = GetRequiredCount();

        if (arguments.Count < requiredCount)
            throw new InvalidArgumentCountException(requiredCount, arguments.Count);

        var argument1 = GetArgument<T1>(0, interpreter, arguments);
        var argument2 = GetArgument<T2>(1, interpreter, arguments);

        return Invoke(interpreter, argument1, argument2);
    }
}

public abstract class GenericFunction<T1, T2, T3> : GenericThreeParameterFunction
    where T1 : ExpressionResult
    where T2 : ExpressionResult
    where T3 : ExpressionResult
{
    protected abstract ExpressionResult Invoke(Interpreter interpreter, T1 argument1, T2 argument2, T3 argument3);

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        var requiredCount = GetRequiredCount();

        if (arguments.Count < requiredCount)
            throw new InvalidArgumentCountException(requiredCount, arguments.Count);

        var argument1 = GetArgument<T1>(0, interpreter, arguments);
        var argument2 = GetArgument<T2>(1, interpreter, arguments);
        var argument3 = GetArgument<T3>(2, interpreter, arguments);

        return Invoke(interpreter, argument1, argument2, argument3);
    }
}

public abstract class GenericParameterlessFunction : NativeFunction
{
    protected virtual int GetRequiredCount() => 0;
    protected virtual ExpressionNode GetDefaultExpression(int index) => throw new InvalidOperationException();
    protected virtual IReadOnlyList<ResultType> GetArgumentTypes(int index) => throw new InvalidOperationException();

    private TArgument GetDefaultArgumentValue<TArgument>(int index, IExpressionEvaluator interpreter)
    {
        var defaultExpression = GetDefaultExpression(index);
        var defaultResult = defaultExpression.EvaluateUsing(interpreter);
        if (defaultResult is not TArgument defaultArgumentResult)
            throw new InvalidArgumentTypeException(defaultExpression.Location, GetArgumentTypes(index), defaultResult);

        return defaultArgumentResult;
    }

    protected TArgument GetArgument<TArgument>(int index, IExpressionEvaluator interpreter, IReadOnlyList<ExpressionNode> arguments) where TArgument : ExpressionResult
    {
        if (arguments.Count < index + 1)
            return GetDefaultArgumentValue<TArgument>(index, interpreter);

        var argumentResult = arguments[index].EvaluateUsing(interpreter);
        if (argumentResult is not TArgument typedResult)
            throw new InvalidArgumentTypeException(arguments[index].Location, GetArgumentTypes(index), argumentResult);

        return typedResult;
    }
}

public abstract class GenericOneParameterFunction : GenericParameterlessFunction
{
    protected IReadOnlyList<ResultType> Argument1Types => NativeParameters.Single().Types;
    protected ExpressionNode? Argument1Default => NativeParameters.Single().DefaultValue;

    protected override int GetRequiredCount() => Argument1Default is null ? 1 : 0;

    protected override ExpressionNode GetDefaultExpression(int index) => Argument1Default ?? throw new InvalidOperationException();

    protected override IReadOnlyList<ResultType> GetArgumentTypes(int index) => Argument1Types;
}

public abstract class GenericTwoParameterFunction : GenericOneParameterFunction
{
    protected IReadOnlyList<ResultType> Argument2Types => NativeParameters.ElementAt(1).Types;
    protected ExpressionNode? Argument2Default => NativeParameters.ElementAt(1).DefaultValue;

    protected override int GetRequiredCount()
    {
        var required = 0;

        if (Argument1Default is null)
            required++;

        if (Argument2Default is null)
            required++;

        return required;
    }

    protected override ExpressionNode GetDefaultExpression(int index)
    {
        return index switch
        {
            0 => Argument1Default ?? throw new InvalidOperationException(),
            1 => Argument2Default ?? throw new InvalidOperationException(),
            _ => throw new ArgumentOutOfRangeException(nameof(index)),
        };
    }

    protected override IReadOnlyList<ResultType> GetArgumentTypes(int index)
    {
        return index switch
        {
            0 => Argument1Types,
            1 => Argument2Types,
            _ => throw new ArgumentOutOfRangeException(nameof(index)),
        };
    }
}

public abstract class GenericThreeParameterFunction : GenericTwoParameterFunction
{
    protected IReadOnlyList<ResultType> Argument3Types => NativeParameters.ElementAt(2).Types;
    protected ExpressionNode? Argument3Default => NativeParameters.ElementAt(2).DefaultValue;

    protected override int GetRequiredCount()
    {
        var required = 0;

        if (Argument1Default is null)
            required++;

        if (Argument2Default is null)
            required++;

        if (Argument3Default is null)
            required++;

        return required;
    }

    protected override ExpressionNode GetDefaultExpression(int index)
    {
        return index switch
        {
            0 => Argument1Default ?? throw new InvalidOperationException(),
            1 => Argument2Default ?? throw new InvalidOperationException(),
            2 => Argument3Default ?? throw new InvalidOperationException(),
            _ => throw new ArgumentOutOfRangeException(nameof(index)),
        };
    }

    protected override IReadOnlyList<ResultType> GetArgumentTypes(int index)
    {
        return index switch
        {
            0 => Argument1Types,
            1 => Argument2Types,
            2 => Argument3Types,
            _ => throw new ArgumentOutOfRangeException(nameof(index)),
        };
    }
}