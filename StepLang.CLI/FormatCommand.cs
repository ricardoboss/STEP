using System.Drawing;
using Pastel;
using StepLang.Tokenizing;

namespace StepLang.CLI;

internal static class FormatCommand
{
    private static VerbosityWriter stdout = new(Console.Out, Verbosity.Normal);
    private static VerbosityWriter stderr = new(Console.Error, Verbosity.Normal);

    public static async Task<int> Invoke(string[] filesOrDirs, bool setExitCode, bool dryRun, Verbosity verbosity)
    {
        stdout = new(Console.Out, verbosity);
        stderr = new(Console.Error, verbosity);

        if (dryRun)
            await stdout.Normal("Dry run mode enabled. No files will be modified.");

        if (filesOrDirs.Length == 0)
            filesOrDirs = new [] { "." };

        var changes = 0;
        var files = 0;
        foreach (var s in filesOrDirs)
        {
            var (c, f) = await Format(s, dryRun);
            changes += c;
            files += f;
        }

        await stdout.Normal($"{(dryRun ? "Would have applied" : "Applied")} {changes} change(s) to {files} file(s).");

        if (setExitCode)
            return changes > 0 ? 1 : 0;

        return 0;
    }

    private static async Task<(int changes, int files)> Format(string fileOrDir, bool dryRun)
    {
        await stdout.Normal($"Formatting '{fileOrDir.Pastel(Color.Aqua)}'...");

        var changes = 0;
        var files = 0;
        if (File.Exists(fileOrDir))
        {
            changes += await FormatFile(new(fileOrDir), dryRun);
            files++;
        }
        else if (Directory.Exists(fileOrDir))
        {
            var stepFilesEnumerable = Directory.EnumerateFiles(fileOrDir, "*.step", new EnumerationOptions
            {
                RecurseSubdirectories = true,
                IgnoreInaccessible = true,
            });

            changes += await stepFilesEnumerable.ToAsyncEnumerable().AggregateAwaitAsync(
                changes,
                async (current, file) =>
                {
                    var fileChanges = await FormatFile(new(file), dryRun);
                    if (fileChanges > 0)
                        files++;
                    return current + fileChanges;
                });
        }
        else
        {
            await stderr.Normal($"The path '{fileOrDir.Pastel(Color.Aqua)}' is not a file or directory.");
        }

        return (changes, files);
    }

    private static async Task<int> FormatFile(FileInfo info, bool dryRun)
    {
        await stdout.Verbose("Formatting file: " + info.FullName);

        List<Token> tokens;
        try
        {
            var fileText = await File.ReadAllTextAsync(info.FullName);
            var tokenizer = new Tokenizer();
            tokenizer.UpdateFile(info);
            tokenizer.Add(fileText);
            tokens = tokenizer.Tokenize().ToList();
        }
        catch (Exception e)
        {
            await stderr.Quiet($"Failed to format file: {info.FullName.Pastel(Color.Aqua)}");
            await stderr.Quiet(e.ToString());

            return 0;
        }

        // var parser = new StatementParser();
        // parser.Add(tokens);
        // var statements = await parser.ParseAsync().ToListAsync();

        await stdout.Verbose($"Found {tokens.Count} tokens.");

        var changes = 0;
        var formatted = Format(tokens, () => changes++).ToList();
        if (!formatted.SequenceEqual(tokens))
        {
            if (!dryRun)
            {
                await File.WriteAllTextAsync(info.FullName, string.Join("", Render(formatted)));
                await stdout.Verbose($"Wrote {changes} change(s) to file.");
            }
            else
            {
                await stdout.Verbose($"Would have written {changes} change(s) to file.");
            }
        }
        else
        {
            await stdout.Verbose("No changes to file.");
        }

        return changes;
    }

    private static IEnumerable<string> Render(IEnumerable<Token> tokens)
    {
        return tokens.Select(token => token.Type switch
        {
            TokenType.NewLine => Environment.NewLine,
            TokenType.LiteralString => $"\"{token.Value}\"",
            _ => token.Value,
        });
    }

    private static IEnumerable<Token> Format(IEnumerable<Token> tokens, Action notifyChanged)
    {
        using var enumerator = tokens.GetEnumerator();

        enumerator.Reset();
        if (!enumerator.MoveNext())
            yield break;

        // clear leading whitespace
        DropLeadingWhitespace(enumerator, notifyChanged);

        // order imports
        foreach (var token in FormatImports(enumerator))
            yield return token;

        // yield the rest of the file
        yield return enumerator.Current;
        while (enumerator.MoveNext())
            yield return enumerator.Current;
    }

    private static void DropLeadingWhitespace(IEnumerator<Token> enumerator, Action notifyChanged)
    {
        while (enumerator.Current.Type is TokenType.Whitespace or TokenType.NewLine)
        {
            notifyChanged();
            if (!enumerator.MoveNext())
                return;
        }
    }

    private static IEnumerable<Token> FormatImports(IEnumerator<Token> enumerator)
    {
        var imports = new List<List<Token>>();
        while (enumerator.Current.Type == TokenType.ImportKeyword)
        {
            var import = new List<Token>();

            do
            {
                import.Add(enumerator.Current);
            } while (enumerator.MoveNext() && !import.Any(t => t.Type is TokenType.NewLine));

            imports.Add(import);
        }

        foreach (var import in imports.OrderBy(i => i[1].Value))
        {
            foreach (var token in import)
                yield return token;

            yield return new(TokenType.NewLine, Environment.NewLine, null);
        }

        if (imports.Count > 0 && enumerator.Current.Type != TokenType.NewLine)
            yield return new(TokenType.NewLine, Environment.NewLine, null);
    }
}