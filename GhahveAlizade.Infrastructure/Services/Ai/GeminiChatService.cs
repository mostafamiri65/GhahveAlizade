using System.Net.Http.Json;
using System.Text.Json;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace GhahveAlizade.Infrastructure.Services.Ai;

public class GeminiChatService : IAiChatService
{
	private readonly HttpClient _http;
	private readonly GeminiOptions _options;

	private const string SystemPrompt = """
		شما «مشاور قهوه علیزاده» هستید؛ متخصص قهوه فروشگاه قهوه علیزاده.
		- از سلیقه مشتری بپرسید: طعم (تلخ/میوه‌ای/شکلاتی)، تلخی، اسیدیته، روش دم‌آوری، بودجه.
		- فقط از محصولات موجود در لیست پیشنهاد بدهید. حداکثر ۳ پیشنهاد با لینک.
		- ترکیب عربیکا/روبستا را توضیح دهید (مثلا ۷۰/۳۰) و بگویید هر دانه چه اثری در طعم دارد.
		- پاسخ فارسی، کوتاه (حداکثر ۶ خط)، دوستانه.
		""";

	public GeminiChatService(HttpClient http, IOptions<GeminiOptions> options)
	{
		_http = http;
		_options = options.Value;
	}

	public async Task<AiChatResult> GetAdviceAsync(
		IReadOnlyList<AiChatTurn> history,
		string catalogContext,
		string userMessage,
		CancellationToken ct = default)
	{
		if (string.IsNullOrWhiteSpace(_options.ApiKey))
			return new AiChatResult(false, "", "کلید Gemini تنظیم نشده (appsettings → Gemini:ApiKey).");

		var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_options.Model}:generateContent?key={_options.ApiKey}";

		var contents = new List<object>();
		foreach (var turn in history)
			contents.Add(new { role = turn.Role, parts = new[] { new { text = turn.Content } } });

		// پیام کاربر + کاتالوگ محصولات
		contents.Add(new
		{
			role = "user",
			parts = new[] { new { text = $"{userMessage}\n\n[لیست محصولات موجود برای پیشنهاد]:\n{catalogContext}" } }
		});

		var payload = new
		{
			system_instruction = new { parts = new[] { new { text = SystemPrompt } } },
			contents,
			generationConfig = new { temperature = 0.7, maxOutputTokens = 600 }
		};

		try
		{
			var response = await _http.PostAsJsonAsync(url, payload, ct);
			if (!response.IsSuccessStatusCode)
				return new AiChatResult(false, "", $"Gemini error {(int)response.StatusCode}");

			var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
			var text = json.GetProperty("candidates")[0]
				.GetProperty("content").GetProperty("parts")[0]
				.GetProperty("text").GetString();

			return new AiChatResult(true, text ?? "");
		}
		catch (Exception ex)
		{
			return new AiChatResult(false, "", ex.Message);
		}
	}
}
