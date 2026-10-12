using GhahveAlizade.Application.Features.Wholesale.Dtos;
using GhahveAlizade.Domain.Enums;
using GhahveAlizade.Domain.Wholesale;
using Mapster;

namespace GhahveAlizade.Application.Features.Wholesale;

public class WholesaleMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<WholesaleRequest, WholesaleRequestDto>()
			.Map(dest => dest.StatusName, src => GetStatusName(src.Status));

		config.NewConfig<WholesaleRequest, WholesaleRequestListItemDto>()
			.Map(dest => dest.StatusName, src => GetStatusName(src.Status));
	}

	private static string GetStatusName(WholesaleRequestStatus s) => s switch
	{
		WholesaleRequestStatus.Pending => "در انتظار",
		WholesaleRequestStatus.UnderReview => "در حال بررسی",
		WholesaleRequestStatus.Approved => "تایید شده",
		WholesaleRequestStatus.Rejected => "رد شده",
		WholesaleRequestStatus.Suspended => "معلق",
		_ => s.ToString()
	};
}