namespace StepLang.Tooling.Formatting.Analyzers.Source;

public class FileFixerSource(FileInfo file) : IFixerSource
{
	public Uri Uri => new Uri(file.FullName);

	public FileInfo File => file;

	public async Task<string> GetSourceCodeAsync(CancellationToken cancellationToken = default)
	{
		return await System.IO.File.ReadAllTextAsync(file.FullName, cancellationToken);
	}
}
