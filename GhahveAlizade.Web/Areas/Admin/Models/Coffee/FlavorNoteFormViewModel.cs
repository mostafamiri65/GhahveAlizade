using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Admin.Models.Coffee;

public class FlavorNoteFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "نام نُت الزامی است.")]
	[StringLength(200)]
	[Display(Name = "نام نُت")]
	public string Name { get; set; } = string.Empty;

	[StringLength(200)]
	[Display(Name = "نامک")]
	public string? Slug { get; set; }

	[StringLength(100)]
	[Display(Name = "دسته (Chocolate, Fruity, ...)")]
	public string? Category { get; set; }

	[StringLength(1000)]
	[Display(Name = "آیکون")]
	public string? IconUrl { get; set; }

	[StringLength(20)]
	[Display(Name = "کد رنگ")]
	public string? ColorCode { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[Range(0, int.MaxValue)]
	[Display(Name = "ترتیب")]
	public int DisplayOrder { get; set; }
}