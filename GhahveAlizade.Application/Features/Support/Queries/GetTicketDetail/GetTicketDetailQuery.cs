using ErrorOr;
using GhahveAlizade.Application.Features.Support.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Queries.GetTicketDetail;

public record GetTicketDetailQuery(long TicketId) : IRequest<ErrorOr<TicketDetailWithMetaDto>>;
