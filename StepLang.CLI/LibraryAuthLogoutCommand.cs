namespace StepLang.CLI;

public static class LibraryAuthLogoutCommand
{
    public static async Task<int> Invoke()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var credentialsPath = new FileInfo(Path.Combine(appData, "STEP", "credentials.json"));

        if (credentialsPath.Exists)
            credentialsPath.Delete();

        await Console.Out.WriteLineAsync("Credentials deleted.");

        return 0;
    }
}