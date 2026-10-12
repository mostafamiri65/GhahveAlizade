using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Admin.Models.Content;

public class BlogTagFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "نام برچسب الزامی است.")]
	[StringLength(200)]
	[Display(Name = "نام")]
	public string Name { get; set; } = string.Empty;

	[StringLength(200)]
	[Display(Name = "نامک")]
	public string? Slug { get; set; }

	[StringLength(1000)]
	[Display(Name = "توضیحات")]
	[DataType(DataType.MultilineText)]
	public string? Description { get; set; }
}