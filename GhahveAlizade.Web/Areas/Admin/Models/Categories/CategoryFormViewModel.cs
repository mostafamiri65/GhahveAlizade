using System.ComponentModel.DataAnnotations;
using GhahveAlizade.Application.Features.Catalog.Categories.Queries.GetCategoriesTree;

namespace GhahveAlizade.Web.Areas.Admin.Models.Categories;

public class CategoryFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "نام دسته‌بندی الزامی است.")]
	[StringLength(300, ErrorMessage = "نام نمی‌تواند بیشتر از 300 کاراکتر باشد.")]
	[Display(Name = "نام دسته‌بندی")]
	public string Name { get; set; } = string.Empty;

	[StringLength(300)]
	[Display(Name = "نامک (Slug)")]
	public string? Slug { get; set; }

	[StringLength(2000)]
	[Display(Name = "توضیحات")]
	[DataType(DataType.MultilineText)]
	public string? Description { get; set; }

	[StringLength(1000)]
	[Display(Name = "تصویر")]
	[Url(ErrorMessage = "آدرس تصویر معتبر نیست.")]
	public string? ImageUrl { get; set; }

	[StringLength(1000)]
	[Display(Name = "آیکون")]
	public string? IconUrl { get; set; }

	[Display(Name = "دسته والد")]
	public long? ParentId { get; set; }

	[Range(0, int.MaxValue, ErrorMessage = "ترتیب نمایش نمی‌تواند منفی باشد.")]
	[Display(Name = "ترتیب نمایش")]
	public int DisplayOrder { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[Display(Name = "ویژه")]
	public bool IsFeatured { get; set; }

	// SEO
	[StringLength(200)]
	[Display(Name = "عنوان سئو")]
	public string? MetaTitle { get; set; }

	[StringLength(500)]
	[Display(Name = "توضیحات سئو")]
	[DataType(DataType.MultilineText)]
	public string? MetaDescription { get; set; }

	[StringLength(500)]
	[Display(Name = "کلمات کلیدی")]
	public string? MetaKeywords { get; set; }

	// لیست دسته‌ها برای Dropdown
	public List<CategoryTreeItemDto> Categories { get; set; } = new();
}