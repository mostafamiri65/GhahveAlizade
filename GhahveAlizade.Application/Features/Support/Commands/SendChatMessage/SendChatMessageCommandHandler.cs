using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Support.Dtos;
using GhahveAlizade.Domain.Support;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Commands.SendChatMessage;

public class SendChatMessageCommandHandler
	: IRequestHandler<SendChatMessageCommand, ErrorOr<ChatMessageDto>>
{
	private readonly IAppDbContext _context;

	public SendChatMessageCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<ChatMessageDto>> Handle(SendChatMessageCommand request, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(request.Body) || request.Body.Trim().Length == 0)
			return Error.Validation("Chat.EmptyMessage", "متن پیام خالی است.");
		if (request.Body.Length > 2000)
			return Error.Validation("Chat.TooLong", "پیام حداکثر ۲۰۰۰ کاراکتر است.");

		var senderIdStr = request.SenderUserId;

		var session = await _context.SupportChatSessions
			.Where(s => s.Id == request.SessionId && !s.IsDeleted
				&& (request.IsFromAdmin || s.UserId == senderIdStr)) // ضد IDOR
			.FirstOrDefaultAsync(ct);

		if (session is null)
			return Error.NotFound("Chat.SessionNotFound", "جلسه چت یافت نشد.");

		if (session.Status == ChatSessionStatus.Closed)
			return Error.Conflict("Chat.Closed", "این گفتگو بسته شده است.");

		string? senderName;
		if (request.IsFromAdmin)
		{
			senderName = await _context.Users
				.Where(u => u.Id == request.SenderUserId)
				.Select(u => (u.FirstName + " " + u.LastName).Trim())
				.FirstOrDefaultAsync(ct) ?? "پشتیبانی";

			// اگر ادمین برای اولین بار پیام می‌دهد، جلسه فعال می‌شود
			if (session.Status == ChatSessionStatus.Waiting)
			{
				session.Status = ChatSessionStatus.Active;
				session.AssignedToUserId = request.SenderUserId;
				session.StartedAtUtc = DateTime.UtcNow;
			}
			else if (session.AssignedToUserId is null)
			{
				session.AssignedToUserId = request.SenderUserId;
			}
		}
		else
		{
			senderName = await _context.Customers
				.Where(c => c.UserId == senderIdStr && !c.IsDeleted)
				.Select(c => (c.FirstName + " " + c.LastName).Trim())
				.FirstOrDefaultAsync(ct) ?? "مشتری";
		}

		var message = new ChatMessage
		{
			SessionId = session.Id,
			SenderUserId = request.SenderUserId,
			SenderName = senderName,
			IsFromAdmin = request.IsFromAdmin,
			Body = request.Body.Trim()
		};

		_context.ChatMessages.Add(message);
		session.UpdatedAtUtc = DateTime.UtcNow;
		await _context.SaveChangesAsync(ct);

		return new ChatMessageDto
		{
			Id = message.Id,
			SenderName = senderName,
			IsFromAdmin = message.IsFromAdmin,
			Body = message.Body,
			CreatedAtUtc = message.CreatedAtUtc
		};
	}
}
