using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Leap.Client;

public class LeapApiCredentialManager
{
    private static string? defaultCredentialsPath;
    private static string DefaultCredentialsPath
    {
        get
        {
            if (defaultCredentialsPath != null)
                return defaultCredentialsPath;

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            return defaultCredentialsPath = Path.Combine(appData, "STEP", "credentials.json");
        }
    }

    private readonly IConfiguration configuration;

    public LeapApiCredentialManager(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    private string CredentialsPath
    {
        get
        {
            var path = configuration["CredentialsPath"];

            return path ?? DefaultCredentialsPath;
        }
    }

    private Credentials? cachedCredentials;

    public Credentials? TryReadCredentials()
    {
        if (cachedCredentials != null)
            return cachedCredentials;

        if (!CredentialsExist())
            return null;

        using var stream = File.OpenRead(CredentialsPath);

        return cachedCredentials = JsonSerializer.Deserialize<Credentials>(stream);
    }

    public void StoreCredentials(Credentials newCredentials, bool overwrite)
    {
        if (CredentialsExist() && !overwrite)
            throw new InvalidOperationException("Credentials already exist.");

        var dir = Path.GetDirectoryName(CredentialsPath);

        if (dir != null && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        using var stream = File.OpenWrite(CredentialsPath);

        JsonSerializer.SerializeAsync(stream, newCredentials);

        cachedCredentials = newCredentials;
    }

    public bool CredentialsExist()
    {
        return File.Exists(CredentialsPath);
    }

    public void DestroyCredentials()
    {
        if (!CredentialsExist())
            return;

        File.Delete(CredentialsPath);

        cachedCredentials = null;
    }
}