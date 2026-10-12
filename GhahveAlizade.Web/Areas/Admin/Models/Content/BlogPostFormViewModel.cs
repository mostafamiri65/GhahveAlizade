using System.ComponentModel.DataAnnotations;
using GhahveAlizade.Application.Features.Content.Dtos;

namespace GhahveAlizade.Web.Areas.Admin.Models.Content;

public class BlogPostFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "عنوان الزامی است.")]
	[StringLength(500)]
	[Display(Name = "عنوان")]
	public string Title { get; set; } = string.Empty;

	[StringLength(500)]
	[Display(Name = "نامک (Slug)")]
	public string? Slug { get; set; }

	[StringLength(2000)]
	[Display(Name = "خلاصه")]
	[DataType(DataType.MultilineText)]
	public string? Summary { get; set; }

	[Required(ErrorMessage = "محتوا الزامی است.")]
	[Display(Name = "محتوا")]
	public string Content { get; set; } = string.Empty;

	[StringLength(1000)]
	[Display(Name = "تصویر شاخص")]
	public string? FeaturedImageUrl { get; set; }

	[StringLength(500)]
	[Display(Name = "متن Alt تصویر")]
	public string? FeaturedImageAlt { get; set; }

	[Display(Name = "دسته‌بندی")]
	public long? CategoryId { get; set; }

	[StringLength(200)]
	[Display(Name = "نویسنده")]
	public string? AuthorName { get; set; }

	[Display(Name = "منتشر شده")]
	public bool IsPublished { get; set; }

	[Display(Name = "ویژه")]
	public bool IsFeatured { get; set; }

	[Display(Name = "اجازه نظرات")]
	public bool AllowComments { get; set; } = true;

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

	[StringLength(1000)]
	[Display(Name = "Canonical URL")]
	public string? CanonicalUrl { get; set; }

	[StringLength(1000)]
	[Display(Name = "تصویر OG")]
	public string? OgImage { get; set; }

	public List<long> TagIds { get; set; } = new();

	public BlogPostFormDataDto FormData { get; set; } = new();
}