using GhahveAlizade.Application.Features.Promotion.Dtos;
using GhahveAlizade.Domain.Enums;
using GhahveAlizade.Domain.Promotion;
using Mapster;

namespace GhahveAlizade.Application.Features.Promotion;

public class CouponMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Coupon, CouponDto>()
			.Map(dest => dest.DiscountTypeName, src => GetDiscountTypeName(src.DiscountType));

		config.NewConfig<Coupon, CouponListItemDto>()
			.Map(dest => dest.DiscountTypeName, src => GetDiscountTypeName(src.DiscountType))
			.Map(dest => dest.Status, src => GetStatus(src));
	}

	private static string GetDiscountTypeName(DiscountType t) => t switch
	{
		DiscountType.Percentage => "درصدی",
		DiscountType.FixedAmount => "مبلغ ثابت",
		DiscountType.FreeShipping => "ارسال رایگان",
		DiscountType.BuyXGetY => "خرید X دریافت Y",
		_ => "نامشخص"
	};

	private static string GetStatus(Coupon c)
	{
		if (!c.IsActive) return "غیرفعال";
		var now = DateTime.UtcNow;
		if (c.StartsAtUtc.HasValue && c.StartsAtUtc > now) return "آینده";
		if (c.ExpiresAtUtc.HasValue && c.ExpiresAtUtc < now) return "منقضی";
		if (c.MaxUsageCount.HasValue && c.UsedCount >= c.MaxUsageCount.Value) return "اتمام";
		return "فعال";
	}
}