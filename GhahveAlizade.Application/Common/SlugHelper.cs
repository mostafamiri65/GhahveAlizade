using System.Text.RegularExpressions;

namespace GhahveAlizade.Application.Common;

public static partial class SlugHelper
{
	public static string GenerateSlug(string input)
	{
		if (string.IsNullOrWhiteSpace(input)) return string.Empty;

		var normalized = NormalizePersian(input);
		var slug = GenerateSlugInternal(normalized);
		return slug;
	}

	private static string NormalizePersian(string input)
	{
		return input
			.Replace('ي', 'ی')
			.Replace('ك', 'ک')
			.Replace('ۀ', 'ه')
			.Replace('ة', 'ه')
			.Replace('\u200c', '-');
	}

	private static string GenerateSlugInternal(string input)
	{
		var str = input.ToLowerInvariant().Trim();
		str = NonWordCharsRegex().Replace(str, "-");
		str = MultiDashRegex().Replace(str, "-");
		str = str.Trim('-');

		if (str.Length > 300)
			str = str.Substring(0, 300).TrimEnd('-');

		return str;
	}

	[GeneratedRegex(@"[\s\-]+")]
	private static partial Regex MultiDashRegex();

	[GeneratedRegex(@"[^\p{L}\p{Nd}\-]+")]
	private static partial Regex NonWordCharsRegex();
}