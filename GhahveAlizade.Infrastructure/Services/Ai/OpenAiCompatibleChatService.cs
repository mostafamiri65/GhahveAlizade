using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace GhahveAlizade.Infrastructure.Services.Ai;

public class OpenAiCompatibleChatService : IAiChatService
{
	private readonly HttpClient _http;
	private readonly OpenAiCompatibleOptions _options;

	private const string SystemPrompt = """
		شما «مشاور قهوه علیزاده» هستید؛ متخصص قهوه فروشگاه اینترنتی قهوه علیزاده (ghahvealizade.ir).
		وظایف شما:
		1. از سلیقه مشتری بپرسید: طعم‌های مورد علاقه (تلخ/میوه‌ای/شکلاتی/خامه‌ای)، میزان تلخی، اسیدیته، روش دم‌آوری (اسپرسو/دمی/ترک/فرنچ‌پرس/کپسول)، بودجه.
		2. فقط از بین «محصولات موجود» (که در پیام برایتان ارسال می‌شود) پیشنهاد بدهید. هرگز محصولی که در لیست نیست را معرفی نکنید.
		3. درباره ترکیب توضیح دهید: مثلا «این بلند ۷۰٪ عربیکا و ۳۰٪ روبستا است؛ روبستا کرِما و بدنه بیشتر می‌دهد، عربیکا عطر و اسیدیته».
		4. پاسخ‌ها کوتاه (حداکثر ۶-۷ خط)، دوستانه و فارسی باشند. حداکثر ۳ پیشنهاد محصول با لینک بدهید.
		5. اگر سوال خارج از حوزه قهوه بود، مؤدبانه به موضوع برگردید.
		""";

	public OpenAiCompatibleChatService(HttpClient http, IOptions<OpenAiCompatibleOptions> options)
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
			return new AiChatResult(false, "", "کلید سرویس AI تنظیم نشده (appsettings → Ai:ApiKey).");

		var url = $"{_options.BaseUrl.TrimEnd('/')}/chat/completions";

		var messages = new List<object> { new { role = "system", content = SystemPrompt } };
		foreach (var turn in history)
			messages.Add(new { role = turn.Role == "model" ? "assistant" : "user", content = turn.Content });

		// پیام کاربر + کاتالوگ محصولات
		messages.Add(new
		{
			role = "user",
			content = $"{userMessage}\n\n[لیست محصولات موجود برای پیشنهاد]:\n{catalogContext}"
		});

		var payload = new
		{
			model = _options.Model,
			messages,
			temperature = _options.Temperature,
			max_tokens = _options.MaxOutputTokens
		};

		using var request = new HttpRequestMessage(HttpMethod.Post, url)
		{
			Content = JsonContent.Create(payload)
		};
		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

		try
		{
			var response = await _http.SendAsync(request, ct);
			if (!response.IsSuccessStatusCode)
			{
				var body = await response.Content.ReadAsStringAsync(ct);
				return new AiChatResult(false, "", $"AI service error {(int)response.StatusCode}");
			}
			var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
			var text = json.GetProperty("choices")[0]
				.GetProperty("message")
				.GetProperty("content")
				.GetString();

			return new AiChatResult(true, text ?? "");
		}
		catch (Exception ex)
		{
			return new AiChatResult(false, "", ex.Message);
		}
	}
}
