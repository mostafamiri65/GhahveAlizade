using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Support.Dtos;
using GhahveAlizade.Domain.Support;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Queries.GetMyChatHistory;

public class GetMyChatHistoryQueryHandler
	: IRequestHandler<GetMyChatHistoryQuery, ErrorOr<ChatHistoryDto>>
{
	private readonly IAppDbContext _context;

	public GetMyChatHistoryQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<ChatHistoryDto>> Handle(GetMyChatHistoryQuery request, CancellationToken ct)
	{
		var userIdStr = request.UserId;

		var session = await _context.SupportChatSessions
			.AsNoTracking()
			.Where(s => s.Id == request.SessionId && !s.IsDeleted && s.UserId == userIdStr) // ضد IDOR
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

		// علامت‌گذاری پیام‌های ادمین به‌عنوان خوانده‌شده
		var unread = await _context.ChatMessages
			.Where(m => m.SessionId == session.Id && m.IsFromAdmin && m.ReadAtUtc == null && !m.IsDeleted)
			.ToListAsync(ct);
		foreach (var m in unread)
			m.ReadAtUtc = DateTime.UtcNow;
		if (unread.Count > 0)
			await _context.SaveChangesAsync(ct);

		string? name = session.GuestName;
		if (name is null && session.CustomerId.HasValue)
		{
			name = await _context.Customers
				.Where(c => c.Id == session.CustomerId)
				.Select(c => (c.FirstName + " " + c.LastName).Trim())
				.FirstOrDefaultAsync(ct);
		}

		return new ChatHistoryDto
		{
			SessionId = session.Id,
			Status = session.Status,
			CustomerName = name,
			GuestName = session.GuestName,
			Messages = messages
		};
	}
}
