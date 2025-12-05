using Spectre.Console.Cli;

namespace StepLang.Tooling.CLI;

public class MultipleCommandInterceptor(params ICommandInterceptor[] interceptors) : ICommandInterceptor
{
	public void Intercept(CommandContext context, CommandSettings settings)
	{
		foreach (var interceptor in interceptors)
			interceptor.Intercept(context, settings);
	}

	public void InterceptResult(CommandContext context, CommandSettings settings, ref int result)
	{
		foreach (var interceptor in interceptors)
			interceptor.InterceptResult(context, settings, ref result);
	}
}
