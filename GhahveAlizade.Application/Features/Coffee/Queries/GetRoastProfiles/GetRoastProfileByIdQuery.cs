using ErrorOr;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetRoastProfiles;

public record GetRoastProfileByIdQuery(long Id) : IRequest<ErrorOr<RoastProfileDto>>;
