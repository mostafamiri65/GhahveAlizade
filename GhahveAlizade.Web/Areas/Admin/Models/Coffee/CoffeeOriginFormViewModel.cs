using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Admin.Models.Coffee;

public class CoffeeOriginFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "نام خاستگاه الزامی است.")]
	[StringLength(200)]
	[Display(Name = "نام خاستگاه")]
	public string Name { get; set; } = string.Empty;

	[StringLength(200)]
	[Display(Name = "نامک")]
	public string? Slug { get; set; }

	[StringLength(2000)]
	[Display(Name = "توضیحات")]
	[DataType(DataType.MultilineText)]
	public string? Description { get; set; }

	[StringLength(5)]
	[Display(Name = "کد کشور")]
	public string? CountryCode { get; set; }

	[StringLength(1000)]
	[Display(Name = "پرچم")]
	public string? FlagUrl { get; set; }

	[StringLength(1000)]
	[Display(Name = "تصویر")]
	public string? ImageUrl { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[Range(0, int.MaxValue)]
	[Display(Name = "ترتیب")]
	public int DisplayOrder { get; set; }

	public List<CoffeeRegionViewModel> Regions { get; set; } = new();
}

public class CoffeeRegionViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "نام منطقه الزامی است.")]
	[StringLength(200)]
	public string? Name { get; set; }

	[StringLength(200)]
	public string? Slug { get; set; }

	[StringLength(2000)]
	[Display(Name = "توضیحات")]
	public string? Description { get; set; }

	[StringLength(100)]
	[Display(Name = "ارتفاع")]
	public string? Altitude { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	public int DisplayOrder { get; set; }
}