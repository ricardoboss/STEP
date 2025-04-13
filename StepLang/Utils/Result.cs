namespace StepLang.Utils;

public abstract class Result<T>
{
	public static Result<T> Ok(T value) => new Ok<T>(value);

	public static Result<T> Err(Exception e) => new Err<T>(e);

	public abstract bool IsOk { get; }

	public bool IsErr => !IsOk;

	public abstract T Value { get; }

	public abstract Exception? Exception { get; }
}

public sealed class Ok<T>(T value) : Result<T>
{
	public override bool IsOk => true;

	public override T Value { get; } = value;

	public override Exception? Exception => null;
}

public sealed class Err<T>(Exception e) : Result<T>
{
	public override bool IsOk => false;

	public override T Value => throw new InvalidOperationException("Cannot get value of an error result");

	public override Exception Exception { get; } = e;
}
