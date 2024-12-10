using Spectre.Console.Cli;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace StepLang.CLI.Commands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
internal sealed class RunCommand : AsyncCommand<RunCommand.Settings>
{
	public sealed class Settings : HiddenGlobalCommandSettings
	{
		[CommandArgument(0, "<file>")]
		[Description("The path to a .step-file to run.")]
		public string File { get; init; } = null!;
	}

	public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
	{
		var scriptFile = new FileInfo(settings.File);

		var source = await CharacterSource.FromFileAsync(scriptFile);

		var tokenizer = new Tokenizer(source);
		var tokens = tokenizer.Tokenize();

		var parser = new Parser(tokens);
		var root = parser.ParseRoot();

		var interpreter = new Interpreter(Console.Out, Console.Error, Console.In);
		root.Accept(interpreter);

		return interpreter.ExitCode;
	}
}
