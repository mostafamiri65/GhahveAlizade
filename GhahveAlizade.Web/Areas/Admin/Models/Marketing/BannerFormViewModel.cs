using System.ComponentModel.DataAnnotations;
using GhahveAlizade.Application.Features.Marketing.Dtos;

namespace GhahveAlizade.Web.Areas.Admin.Models.Marketing;

public class BannerFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "عنوان الزامی است.")]
	[StringLength(300)]
	[Display(Name = "عنوان")]
	public string Title { get; set; } = string.Empty;

	[StringLength(500)]
	[Display(Name = "زیرعنوان")]
	public string? Subtitle { get; set; }

	[StringLength(2000)]
	[Display(Name = "توضیحات")]
	[DataType(DataType.MultilineText)]
	public string? Description { get; set; }

	[StringLength(1000)]
	[Display(Name = "آدرس تصویر (دسکتاپ) *")]
	public string ImageUrl { get; set; } = string.Empty;

	[StringLength(1000)]
	[Display(Name = "آدرس تصویر موبایل")]
	public string? MobileImageUrl { get; set; }

	[StringLength(500)]
	[Display(Name = "متن Alt (سئو)")]
	public string? AltText { get; set; }

	[StringLength(1000)]
	[Display(Name = "لینک")]
	public string? LinkUrl { get; set; }

	[StringLength(100)]
	[Display(Name = "متن دکمه")]
	public string? ButtonText { get; set; }

	[StringLength(20)]
	[Display(Name = "باز شدن در")]
	public string? Target { get; set; } = "_self";

	[Display(Name = "تاریخ شروع")]
	public DateTime? StartsAtUtc { get; set; }

	[Display(Name = "تاریخ پایان")]
	public DateTime? ExpiresAtUtc { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[Range(0, int.MaxValue)]
	[Display(Name = "ترتیب")]
	public int DisplayOrder { get; set; }

	public List<BannerPlacementViewModel> Placements { get; set; } = new();

	public BannerFormDataDto FormData { get; set; } = new();
}

public class BannerPlacementViewModel
{
	public long Id { get; set; }
	public int Placement { get; set; }
	public string? PlacementName { get; set; }
	public int DisplayOrder { get; set; }
	public bool IsActive { get; set; } = true;
}