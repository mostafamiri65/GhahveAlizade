using ErrorOr;
using GhahveAlizade.Application.Features.Wholesale.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Wholesale.Queries.GetWholesaleRequestById;

public record GetWholesaleRequestByIdQuery(long Id) : IRequest<ErrorOr<WholesaleRequestDto>>;
