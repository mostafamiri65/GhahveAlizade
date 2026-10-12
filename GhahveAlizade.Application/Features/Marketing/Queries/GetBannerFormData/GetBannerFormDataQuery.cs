using ErrorOr;
using GhahveAlizade.Application.Features.Marketing.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Marketing.Queries.GetBannerFormData;

public record GetBannerFormDataQuery : IRequest<ErrorOr<BannerFormDataDto>>;
