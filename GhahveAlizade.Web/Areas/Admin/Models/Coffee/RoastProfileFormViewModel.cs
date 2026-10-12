using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Admin.Models.Coffee;

public class RoastProfileFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "نام پروفایل الزامی است.")]
	[StringLength(200)]
	[Display(Name = "نام پروفایل")]
	public string Name { get; set; } = string.Empty;

	[StringLength(200)]
	[Display(Name = "نامک")]
	public string? Slug { get; set; }

	[StringLength(2000)]
	[Display(Name = "توضیحات")]
	[DataType(DataType.MultilineText)]
	public string? Description { get; set; }

	[StringLength(20)]
	[Display(Name = "کد رنگ")]
	public string? ColorCode { get; set; }

	[Range(1, 10)]
	[Display(Name = "سطح رست (1-10)")]
	public int RoastLevel { get; set; } = 5;

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[Range(0, int.MaxValue)]
	[Display(Name = "ترتیب")]
	public int DisplayOrder { get; set; }
}