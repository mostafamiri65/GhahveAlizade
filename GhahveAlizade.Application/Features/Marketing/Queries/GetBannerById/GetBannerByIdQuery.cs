using ErrorOr;
using GhahveAlizade.Application.Features.Marketing.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Marketing.Queries.GetBannerById;

public record GetBannerByIdQuery(long Id) : IRequest<ErrorOr<BannerDto>>;