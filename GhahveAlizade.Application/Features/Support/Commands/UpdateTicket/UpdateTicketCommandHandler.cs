using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Support;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Commands.UpdateTicket;

public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public UpdateTicketCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateTicketCommand request, CancellationToken ct)
	{
		var ticket = await _context.SupportTickets
			.Where(t => t.Id == request.TicketId && !t.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (ticket is null)
			return Error.NotFound("Ticket.NotFound", "تیکت یافت نشد.");

		if (request.Status.HasValue)
		{
			ticket.Status = request.Status.Value;
			if (request.Status.Value == TicketStatus.Resolved)
				ticket.ResolvedAtUtc = DateTime.UtcNow;
			if (request.Status.Value == TicketStatus.Closed)
				ticket.ClosedAtUtc = DateTime.UtcNow;
		}

		if (request.Priority.HasValue)
			ticket.Priority = request.Priority.Value;

		if (request.AssignToUserId.HasValue)
			ticket.AssignedToUserId = request.AssignToUserId;

		ticket.UpdatedAtUtc = DateTime.UtcNow;
		ticket.UpdatedBy = request.AdminUserId;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
