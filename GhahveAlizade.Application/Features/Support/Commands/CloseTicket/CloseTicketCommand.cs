using ErrorOr;
using GhahveAlizade.Domain.Support;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Commands.CloseTicket;

public record CloseTicketCommand(
	long UserId,
	long TicketId,
	bool IsFromAdmin = false) : IRequest<ErrorOr<Success>>;
