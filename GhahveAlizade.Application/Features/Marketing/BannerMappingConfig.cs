using GhahveAlizade.Application.Features.Marketing.Dtos;
using GhahveAlizade.Domain.Enums;
using GhahveAlizade.Domain.Marketing;
using Mapster;

namespace GhahveAlizade.Application.Features.Marketing;

public class BannerMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Banner, BannerDto>()
			.Map(dest => dest.Placements, src => src.Placements
				.Where(p => !p.IsDeleted)
				.OrderBy(p => p.DisplayOrder)
				.ToList());

		config.NewConfig<Banner, BannerListItemDto>()
			.Map(dest => dest.PlacementCount,
				src => src.Placements.Count(p => !p.IsDeleted && p.IsActive))
			.Map(dest => dest.PlacementNames,
				src => src.Placements
					.Where(p => !p.IsDeleted && p.IsActive)
					.Select(p => GetPlacementName(p.Placement))
					.ToList());

		config.NewConfig<BannerPlacement, BannerPlacementDto>()
			.Map(dest => dest.PlacementName, src => GetPlacementName(src.Placement));
	}

	public static string GetPlacementName(BannerPlacementType t) => t switch
	{
		BannerPlacementType.HomeHero => "هدر اصلی صفحه اصلی",
		BannerPlacementType.HomeMiddle => "میانه صفحه اصلی",
		BannerPlacementType.HomeBottom => "پایین صفحه اصلی",
		BannerPlacementType.SidebarTop => "سایدبار بالا",
		BannerPlacementType.SidebarBottom => "سایدبار پایین",
		BannerPlacementType.CategoryTop => "بالای دسته‌بندی",
		BannerPlacementType.ProductDetailSide => "کنار صفحه محصول",
		BannerPlacementType.CartPage => "صفحه سبد خرید",
		BannerPlacementType.CheckoutPage => "صفحه پرداخت",
		BannerPlacementType.BlogTop => "بالای وبلاگ",
		_ => t.ToString()
	};

	public static string GetPlacementGroup(BannerPlacementType t) => t switch
	{
		BannerPlacementType.HomeHero or BannerPlacementType.HomeMiddle or BannerPlacementType.HomeBottom => "صفحه اصلی",
		BannerPlacementType.SidebarTop or BannerPlacementType.SidebarBottom => "سایدبار",
		BannerPlacementType.CategoryTop => "دسته‌بندی",
		BannerPlacementType.ProductDetailSide => "محصول",
		BannerPlacementType.CartPage => "سبد خرید",
		BannerPlacementType.CheckoutPage => "پرداخت",
		BannerPlacementType.BlogTop => "وبلاگ",
		_ => "سایر"
	};
}