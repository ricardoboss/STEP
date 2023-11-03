using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework;

public abstract class GenericFunction<T> : NativeFunction where T : ExpressionResult
{
    protected abstract ExpressionResult Invoke(Interpreter interpreter, T argument);

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        if (arguments.Count != 1)
            throw new InvalidArgumentCountException(1, arguments.Count);

        var argument = arguments.Single().EvaluateUsing(interpreter);
        if (argument is not T t)
            // TODO: improve error message and try to get location
            throw new InvalidArgumentTypeException(null, "Unexpected argument type");

        return Invoke(interpreter, t);
    }
}

public abstract class GenericFunction<T1, T2> : NativeFunction where T1 : ExpressionResult where T2 : ExpressionResult
{
    protected abstract ExpressionResult Invoke(Interpreter interpreter, T1 argument1, T2 argument2);

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        if (arguments.Count != 2)
            throw new InvalidArgumentCountException(2, arguments.Count);

        var evalArgs = arguments.EvaluateUsing(interpreter).ToList();

        if (evalArgs[0] is not T1 t1)
            throw new InvalidArgumentTypeException(null, "Unexpected argument type");

        if (evalArgs[1] is not T2 t2)
            throw new InvalidArgumentTypeException(null, "Unexpected argument type");

        return Invoke(interpreter, t1, t2);
    }
}

public abstract class GenericFunction<T1, T2, T3> : NativeFunction where T1 : ExpressionResult where T2 : ExpressionResult where T3 : ExpressionResult
{
    protected abstract ExpressionResult Invoke(Interpreter interpreter, T1 argument1, T2 argument2, T3 argument3);

    public override ExpressionResult Invoke(Interpreter interpreter, IReadOnlyList<ExpressionNode> arguments)
    {
        if (arguments.Count != 3)
            throw new InvalidArgumentCountException(3, arguments.Count);

        var evalArgs = arguments.EvaluateUsing(interpreter).ToList();

        if (evalArgs[0] is not T1 t1)
            throw new InvalidArgumentTypeException(null, "Unexpected argument type");

        if (evalArgs[1] is not T2 t2)
            throw new InvalidArgumentTypeException(null, "Unexpected argument type");

        if (evalArgs[2] is not T3 t3)
            throw new InvalidArgumentTypeException(null, "Unexpected argument type");

        return Invoke(interpreter, t1, t2, t3);
    }
}