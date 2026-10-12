using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Admin.Models.Coffee;

public class CoffeeProcessFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "نام فرآیند الزامی است.")]
	[StringLength(200)]
	[Display(Name = "نام فرآیند")]
	public string Name { get; set; } = string.Empty;

	[StringLength(200)]
	[Display(Name = "نامک")]
	public string? Slug { get; set; }

	[StringLength(2000)]
	[Display(Name = "توضیحات")]
	[DataType(DataType.MultilineText)]
	public string? Description { get; set; }

	[StringLength(1000)]
	[Display(Name = "تصویر")]
	public string? ImageUrl { get; set; }

	[StringLength(1000)]
	[Display(Name = "آیکون")]
	public string? IconUrl { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[Range(0, int.MaxValue)]
	[Display(Name = "ترتیب")]
	public int DisplayOrder { get; set; }
}