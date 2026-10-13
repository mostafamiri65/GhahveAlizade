namespace GhahveAlizade.Infrastructure.Services.Ai;

/// <summary>
/// تنظیمات سرویس AI سازگار با فرمت OpenAI.
/// پیش‌فرض: OpenRouter (قابل استفاده از ایران، مدل‌های رایگان مثل llama-3.3-70b:free).
/// با Groq / Together / هر سرویس OpenAI-compatible هم کار می‌کند.
/// </summary>
public class OpenAiCompatibleOptions
{
	public const string SectionName = "Ai";
	public string ApiKey { get; set; } = "";
	public string BaseUrl { get; set; } = "https://openrouter.ai/api/v1";
	//public string Model { get; set; } = "meta-llama/llama-3.3-70b-instruct:free";
	public string Model { get; set; } = "deepseek/deepseek-chat-v3-0324";

	public int MaxOutputTokens { get; set; } = 700;
	public double Temperature { get; set; } = 0.7;
}
