using ErrorOr;
using GhahveAlizade.Application.Features.Support.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Queries.GetMyTicketById;

public record GetMyTicketByIdQuery(long UserId, long TicketId)
	: IRequest<ErrorOr<TicketDetailDto>>;
