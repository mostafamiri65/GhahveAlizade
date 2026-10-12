using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Support.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Queries.GetChatHistory;

public class GetChatHistoryQueryHandler
	: IRequestHandler<GetChatHistoryQuery, ErrorOr<ChatHistoryDto>>
{
	private readonly IAppDbContext _context;

	public GetChatHistoryQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<ChatHistoryDto>> Handle(GetChatHistoryQuery request, CancellationToken ct)
	{
		var session = await _context.SupportChatSessions
			.AsNoTracking()
			.Include(s => s.Customer)
			.Where(s => s.Id == request.SessionId && !s.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (session is null)
			return Error.NotFound("Chat.SessionNotFound", "جلسه چت یافت نشد.");

		var messagesQuery = _context.ChatMessages
			.AsNoTracking()
			.Where(m => m.SessionId == session.Id && !m.IsDeleted);

		if (request.AfterMessageId.HasValue)
			messagesQuery = messagesQuery.Where(m => m.Id > request.AfterMessageId.Value);

		var messages = await messagesQuery
			.OrderBy(m => m.CreatedAtUtc)
			.Take(200)
			.Select(m => new ChatMessageDto
			{
				Id = m.Id,
				SenderName = m.SenderName,
				IsFromAdmin = m.IsFromAdmin,
				Body = m.Body,
				CreatedAtUtc = m.CreatedAtUtc
			})
			.ToListAsync(ct);

		// علامت‌گذاری پیام‌های مشتری به‌عنوان خوانده‌شده
		var unread = await _context.ChatMessages
			.Where(m => m.SessionId == session.Id && !m.IsFromAdmin && m.ReadAtUtc == null && !m.IsDeleted)
			.ToListAsync(ct);
		foreach (var m in unread)
			m.ReadAtUtc = DateTime.UtcNow;
		if (unread.Count > 0)
			await _context.SaveChangesAsync(ct);

		return new ChatHistoryDto
		{
			SessionId = session.Id,
			Status = session.Status,
			CustomerName = session.Customer != null
				? (session.Customer.FirstName + " " + session.Customer.LastName).Trim()
				: session.GuestName,
			GuestName = session.GuestName,
			Messages = messages
		};
	}
}
