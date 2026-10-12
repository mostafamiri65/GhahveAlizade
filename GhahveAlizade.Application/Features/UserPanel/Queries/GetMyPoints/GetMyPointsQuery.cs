using ErrorOr;
using GhahveAlizade.Application.Features.UserPanel.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyPoints;

public record GetMyPointsQuery(long UserId) : IRequest<ErrorOr<MyPointDto>>;
