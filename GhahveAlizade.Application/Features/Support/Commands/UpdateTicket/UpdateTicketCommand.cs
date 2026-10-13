using ErrorOr;
using GhahveAlizade.Domain.Support;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Commands.UpdateTicket;

public record UpdateTicketCommand(
	long AdminUserId,
	long TicketId,
	TicketStatus? Status = null,
	TicketPriority? Priority = null,
	long? AssignToUserId = null) : IRequest<ErrorOr<Success>>;
