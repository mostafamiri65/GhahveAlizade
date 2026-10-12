using ErrorOr;
using GhahveAlizade.Application.Features.Marketing.Dtos;
using GhahveAlizade.Domain.Enums;
using MediatR;

namespace GhahveAlizade.Application.Features.Marketing.Queries.GetBannerFormData;

public class GetBannerFormDataQueryHandler
	: IRequestHandler<GetBannerFormDataQuery, ErrorOr<BannerFormDataDto>>
{
	public Task<ErrorOr<BannerFormDataDto>> Handle(
		GetBannerFormDataQuery request, CancellationToken ct)
	{
		var dto = new BannerFormDataDto
		{
			PlacementTypes = Enum.GetValues<BannerPlacementType>()
				.Select(p => new PlacementLookupItem
				{
					Value = (int)p,
					Name = BannerMappingConfig.GetPlacementName(p),
					Group = BannerMappingConfig.GetPlacementGroup(p)
				})
				.OrderBy(p => p.Group)
				.ThenBy(p => p.Value)
				.ToList()
		};

		return Task.FromResult<ErrorOr<BannerFormDataDto>>(dto);
	}
}