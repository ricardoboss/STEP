namespace StepLang.Tooling.Analysis.Analyzers.Source;

public interface IFixerSource
{
	Uri? Uri { get; }

	FileInfo? File { get; }

	Task<string> GetSourceCodeAsync(CancellationToken cancellationToken = default);
}
