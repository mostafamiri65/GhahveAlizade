namespace GhahveAlizade.Application.Common.Interfaces;

/// <summary>
/// سرویس چت هوش مصنوعی — Provider-agnostic (Gemini/OpenAI/...).
/// پیاده‌سازی رایگان: Google Gemini (gemini-2.0-flash — free tier).
/// </summary>
public interface IAiChatService
{
	/// <summary>پاسخ هوش مصنوعی برای پیام کاربر با توجه به تاریخچه</summary>
	Task<AiChatResult> GetAdviceAsync(
		IReadOnlyList<AiChatTurn> history,
		string catalogContext,
		string userMessage,
		CancellationToken ct = default);
}

public record AiChatTurn(string Role, string Content);

public record AiChatResult(bool Success, string Reply, string? Error = null);
