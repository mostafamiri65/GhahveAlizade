using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Support;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Commands.CloseChatSession;

public class CloseChatSessionCommandHandler : IRequestHandler<CloseChatSessionCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public CloseChatSessionCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(CloseChatSessionCommand request, CancellationToken ct)
	{
		var userIdStr = request.UserId;

		var session = await _context.SupportChatSessions
			.Where(s => s.Id == request.SessionId && !s.IsDeleted
				&& (request.IsFromAdmin || s.UserId == userIdStr))
			.FirstOrDefaultAsync(ct);

		if (session is null)
			return Error.NotFound("Chat.SessionNotFound", "جلسه چت یافت نشد.");

		session.Status = ChatSessionStatus.Closed;
		session.ClosedAtUtc = DateTime.UtcNow;
		session.ClosedByUserId = request.UserId;
		session.UpdatedAtUtc = DateTime.UtcNow;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
