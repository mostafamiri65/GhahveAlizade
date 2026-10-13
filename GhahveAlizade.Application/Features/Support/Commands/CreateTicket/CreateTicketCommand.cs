using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Commands.CreateTicket;

public record CreateTicketCommand(
	long UserId,
	string Subject,
	string Body,
	string Category = "General",
	long? RelatedOrderId = null) : IRequest<ErrorOr<CreateTicketResponse>>;

public record CreateTicketResponse(long TicketId, string TicketNumber);
