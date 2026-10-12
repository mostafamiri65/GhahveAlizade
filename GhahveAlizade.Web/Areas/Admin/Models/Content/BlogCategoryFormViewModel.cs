using System.ComponentModel.DataAnnotations;
using GhahveAlizade.Application.Features.Content.Dtos;

namespace GhahveAlizade.Web.Areas.Admin.Models.Content;

public class BlogCategoryFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "نام الزامی است.")]
	[StringLength(300)]
	[Display(Name = "نام")]
	public string Name { get; set; } = string.Empty;

	[StringLength(300)]
	[Display(Name = "نامک")]
	public string? Slug { get; set; }

	[StringLength(2000)]
	[Display(Name = "توضیحات")]
	[DataType(DataType.MultilineText)]
	public string? Description { get; set; }

	[StringLength(1000)]
	[Display(Name = "تصویر")]
	public string? ImageUrl { get; set; }

	[Display(Name = "دسته والد")]
	public long? ParentId { get; set; }

	[Range(0, int.MaxValue)]
	[Display(Name = "ترتیب")]
	public int DisplayOrder { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[StringLength(200)]
	[Display(Name = "عنوان سئو")]
	public string? MetaTitle { get; set; }

	[StringLength(500)]
	[Display(Name = "توضیحات سئو")]
	[DataType(DataType.MultilineText)]
	public string? MetaDescription { get; set; }

	[StringLength(1000)]
	[Display(Name = "Canonical URL")]
	public string? CanonicalUrl { get; set; }

	public List<BlogCategoryTreeItemDto> Categories { get; set; } = new();
}