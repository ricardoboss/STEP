using StepLang.Tooling.Highlighting;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace StepLang.CLI.Converter;

[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class ColorSchemeConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
	{
		return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
	}

	public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
	{
		if (value is string str)
		{
			return ColorScheme.ByName(str);
		}

		return base.ConvertFrom(context, culture, value);
	}
}
