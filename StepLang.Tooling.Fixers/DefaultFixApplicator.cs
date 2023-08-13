namespace StepLang.Formatters;

public class DefaultFixApplicator : BaseFixApplicator
{
    public event EventHandler<FixResult>? Failure;

    public event EventHandler<FixResult>? BeforeApplyFix;

    public event EventHandler<FixResult>? AfterApplyFix;

    protected virtual void OnFailure(FixResult result) => Failure?.Invoke(this, result);

    protected virtual void OnBeforeApplyFix(FixResult result) => BeforeApplyFix?.Invoke(this, result);

    protected virtual void OnAfterApplyFix(FixResult result) => AfterApplyFix?.Invoke(this, result);

    public bool ThrowOnFailure { get; set; } = true;

    public bool DryRun { get; set; } = false;

    public override async Task ApplyFixesAsync(IFixer fixer, FileInfo file, CancellationToken cancellationToken = default)
    {
        switch (fixer)
        {
            case IStringFixer stringFixer:
                await ApplyStringFixer(stringFixer, file, cancellationToken);
                break;
            case IFileFixer fileFixer:
                await ApplyFileFixer(file, fileFixer, cancellationToken);
                break;
        }
    }

    private async Task ApplyFileFixer(FileInfo file, IFileFixer fileFixer, CancellationToken cancellationToken)
    {
        var fileFixResult = await fileFixer.FixAsync(file, cancellationToken);
        if (!fileFixResult.Success)
        {
            if (ThrowOnFailure)
                throw new FixerException(fileFixer, fileFixResult.Message);

            OnFailure(fileFixResult);

            return;
        }

        OnBeforeApplyFix(fileFixResult);

        if (!DryRun)
            File.Copy(fileFixResult.FixedFile!.FullName, file.FullName, true);

        OnAfterApplyFix(fileFixResult);
    }

    private async Task ApplyStringFixer(IStringFixer stringFixer, FileSystemInfo file, CancellationToken cancellationToken)
    {
        var fileContents = await File.ReadAllTextAsync(file.FullName, cancellationToken);

        var stringFixResult = await stringFixer.FixAsync(fileContents, cancellationToken);
        if (!stringFixResult.Success)
        {
            if (ThrowOnFailure)
                throw new FixerException(stringFixer, stringFixResult.Message);

            OnFailure(stringFixResult);

            return;
        }

        OnBeforeApplyFix(stringFixResult);

        if (!DryRun)
            await File.WriteAllTextAsync(file.FullName, stringFixResult.FixedString, cancellationToken);

        OnAfterApplyFix(stringFixResult);
    }
}