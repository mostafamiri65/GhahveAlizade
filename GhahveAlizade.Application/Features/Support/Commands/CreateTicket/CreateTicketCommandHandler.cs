using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Support;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Commands.CreateTicket;

public class CreateTicketCommandHandler
	: IRequestHandler<CreateTicketCommand, ErrorOr<CreateTicketResponse>>
{
	private readonly IAppDbContext _context;

	public CreateTicketCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<CreateTicketResponse>> Handle(CreateTicketCommand request, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(request.Subject) || request.Subject.Trim().Length < 5)
			return Error.Validation("Ticket.SubjectRequired", "موضوع تیکت باید حداقل ۵ کاراکتر باشد.");
		if (string.IsNullOrWhiteSpace(request.Body) || request.Body.Trim().Length < 10)
			return Error.Validation("Ticket.BodyRequired", "متن تیکت باید حداقل ۱۰ کاراکتر باشد.");

		var userIdStr = request.UserId.ToString();

		var customer = await _context.Customers
			.AsNoTracking()
			.Where(c => c.UserId == userIdStr && !c.IsDeleted)
			.Select(c => (long?)c.Id)
			.FirstOrDefaultAsync(ct);

		var now = DateTime.UtcNow;
		var ticketNumber = $"TK-{now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";

		var ticket = new SupportTicket
		{
			TicketNumber = ticketNumber,
			UserId = userIdStr,
			CustomerId = customer,
			Subject = request.Subject.Trim(),
			Body = request.Body.Trim(),
			Category = request.Category,
			Status = TicketStatus.Open,
			Priority = TicketPriority.Normal
		};

		ticket.Messages.Add(new TicketMessage
		{
			Body = request.Body.Trim(),
			SenderUserId = request.UserId,
			SenderName = "مشتری",
			IsFromAdmin = false
		});

		_context.SupportTickets.Add(ticket);
		await _context.SaveChangesAsync(ct);

		return new CreateTicketResponse(ticket.Id, ticket.TicketNumber);
	}
}
