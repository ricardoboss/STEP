using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StepLang;

public static class Telemetry
{
	public const string ActivityName = "StepLang";

	private static readonly ActivitySource Activity = new(ActivityName);

	internal static Activity? Profile(string context = "", [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "")
	{
		var className = Path.GetFileNameWithoutExtension(filePath);

		if (!string.IsNullOrEmpty(context))
			methodName = $"{methodName} ({context})";

		var spanName = $"{className}::{methodName}";

		return Activity.StartActivity(spanName);
	}
}
