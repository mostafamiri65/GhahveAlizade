using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Admin.Models.Seo;

public class SeoLandingPageFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "عنوان الزامی است.")]
	[StringLength(500)]
	[Display(Name = "عنوان")]
	public string Title { get; set; } = string.Empty;

	[StringLength(500)]
	[Display(Name = "نامک")]
	public string? Slug { get; set; }

	[StringLength(500)]
	[Display(Name = "H1 صفحه")]
	public string? H1 { get; set; }

	[Display(Name = "محتوا")]
	public string? Content { get; set; }

	[StringLength(1000)]
	[Display(Name = "تصویر شاخص")]
	public string? FeaturedImageUrl { get; set; }

	[Display(Name = "فیلتر محصولات (JSON)")]
	public string? FilterCriteriaJson { get; set; }

	[Display(Name = "منتشر شده")]
	public bool IsPublished { get; set; }

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

	[Display(Name = "Schema.org JSON-LD")]
	public string? JsonLdSchema { get; set; }
}