using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.AiAdvisor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.AiAdvisor.Commands.SendAiMessage;

public class SendAiMessageCommandHandler
	: IRequestHandler<SendAiMessageCommand, ErrorOr<SendAiMessageResponse>>
{
	private readonly IAiChatService _ai;
	private readonly IAppDbContext _context;
	private readonly ISender _mediator;

	private const string SystemPrompt = """
		شما «مشاور قهوه علیزاده» هستید؛ متخصص قهوه برای فروشگاه اینترنتی قهوه علیزاده.
		وظایف شما:
		1. از سلیقه مشتری بپرسید: طعم‌های مورد علاقه (تلخ/میوه‌ای/شکلاتی/خامه‌ای)، میزان تلخی، اسیدیته، روش دم‌آوری (اسپرسو/دمی/ترک/فرنچ‌پرس/کپسول)، بودجه.
		2. بر اساس پاسخ‌ها، فقط از بین «محصولات موجود» (که در هر پیام برایتان ارسال می‌شود) پیشنهاد بدهید. هرگز محصولی که در لیست نیست را معرفی نکنید.
		3. درباره ترکیب توضیح دهید: مثلا «این بلند ۷۰٪ عربیکا و ۳۰٪ روبستا است؛ روبستا کرِما و بدنه بیشتر می‌دهد، عربیکا عطر و اسیدیته».
		4. پاسخ‌ها کوتاه (حداکثر ۶-۷ خط)، دوستانه و فارسی باشند. حداکثر ۳ پیشنهاد محصول با لینک بدهید.
		5. اگر سوال خارج از حوزه قهوه بود، مؤدبانه به موضوع برگردید.
		""";

	public SendAiMessageCommandHandler(IAiChatService ai, IAppDbContext context, ISender mediator)
	{
		_ai = ai;
		_context = context;
		_mediator = mediator;
	}

	public async Task<ErrorOr<SendAiMessageResponse>> Handle(SendAiMessageCommand request, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(request.Message) || request.Message.Length > 2000)
			return Error.Validation("Ai.InvalidMessage", "پیام نامعتبر است.");

		var userIdStr = request.UserId.ToString();

		var session = await _context.AiAdvisorSessions
			.Where(s => s.UserId == userIdStr && !s.IsClosed && !s.IsDeleted)
			.OrderByDescending(s => s.CreatedAtUtc)
			.FirstOrDefaultAsync(ct);

		if (session is null)
		{
			session = new AiAdvisorSession { UserId = userIdStr };
			_context.AiAdvisorSessions.Add(session);
			await _context.SaveChangesAsync(ct);
		}

		// فقط ۱۰ پیام آخر برای صرفه‌جویی توکن
		var history = await _context.AiAdvisorMessages
			.AsNoTracking()
			.Where(m => m.SessionId == session.Id && !m.IsDeleted)
			.OrderBy(m => m.CreatedAtUtc)
			.ToListAsync(ct);

		var turns = history
			.TakeLast(10)
			.Select(m => new AiChatTurn(m.Role, m.Content))
			.ToList();

		var catalogResult = await _mediator.Send(
			new Queries.GetCatalogContext.GetCatalogContextQuery(), ct);

		var catalog = catalogResult.IsError ? "لیست محصولات در دسترس نیست." : catalogResult.Value;

		var aiResult = await _ai.GetAdviceAsync(turns, catalog, request.Message.Trim(), ct);
		if (!aiResult.Success)
			return Error.Failure("Ai.ServiceError",
				aiResult.Error ?? "سرویس هوش مصنوعی در دسترس نیست. بعداً تلاش کنید.");

		_context.AiAdvisorMessages.Add(new AiAdvisorMessage
		{
			SessionId = session.Id,
			Role = "user",
			Content = request.Message.Trim()
		});
		var aiMsg = new AiAdvisorMessage
		{
			SessionId = session.Id,
			Role = "model",
			Content = aiResult.Reply
		};
		_context.AiAdvisorMessages.Add(aiMsg);
		await _context.SaveChangesAsync(ct);

		return new SendAiMessageResponse(aiResult.Reply, aiMsg.Id);
	}
}
