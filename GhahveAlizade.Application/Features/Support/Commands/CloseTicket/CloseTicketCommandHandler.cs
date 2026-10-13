using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Support;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Commands.CloseTicket;

public class CloseTicketCommandHandler : IRequestHandler<CloseTicketCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public CloseTicketCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(CloseTicketCommand request, CancellationToken ct)
	{
		var userIdStr = request.UserId;

		var ticket = await _context.SupportTickets
			.Where(t => t.Id == request.TicketId && !t.IsDeleted
				&& (request.IsFromAdmin || t.UserId == userIdStr))
			.FirstOrDefaultAsync(ct);

		if (ticket is null)
			return Error.NotFound("Ticket.NotFound", "تیکت یافت نشد.");

		ticket.Status = TicketStatus.Closed;
		ticket.ClosedAtUtc = DateTime.UtcNow;
		ticket.UpdatedAtUtc = DateTime.UtcNow;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
