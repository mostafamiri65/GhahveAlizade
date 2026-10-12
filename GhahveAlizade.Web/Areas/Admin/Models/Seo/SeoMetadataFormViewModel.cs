using System.ComponentModel.DataAnnotations;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Web.Areas.Admin.Models.Seo;

public class SeoMetadataFormViewModel
{
	public long Id { get; set; }

	[Required]
	[Display(Name = "نوع موجودیت")]
	public SeoEntityType EntityType { get; set; }

	[Required]
	[Display(Name = "شناسه موجودیت")]
	public long EntityId { get; set; }

	[Required]
	[StringLength(10)]
	[Display(Name = "زبان")]
	public string Culture { get; set; } = "fa";

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

	[StringLength(200)]
	[Display(Name = "عنوان OG")]
	public string? OgTitle { get; set; }

	[StringLength(500)]
	[Display(Name = "توضیحات OG")]
	public string? OgDescription { get; set; }

	[StringLength(1000)]
	[Display(Name = "تصویر OG")]
	public string? OgImage { get; set; }

	[StringLength(50)]
	[Display(Name = "نوع OG")]
	public string? OgType { get; set; }

	[StringLength(50)]
	[Display(Name = "Twitter Card")]
	public string? TwitterCard { get; set; }

	[StringLength(200)]
	[Display(Name = "عنوان Twitter")]
	public string? TwitterTitle { get; set; }

	[StringLength(500)]
	[Display(Name = "توضیحات Twitter")]
	public string? TwitterDescription { get; set; }

	[StringLength(1000)]
	[Display(Name = "تصویر Twitter")]
	public string? TwitterImage { get; set; }

	[Display(Name = "Robots")]
	public RobotsDirective Robots { get; set; } = RobotsDirective.IndexFollow;

	[Display(Name = "JSON-LD Schema")]
	public string? JsonLdSchema { get; set; }

	[StringLength(200)]
	[Display(Name = "کلمه کلیدی اصلی")]
	public string? FocusKeyword { get; set; }

	[Range(0, 100)]
	[Display(Name = "امتیاز سئو")]
	public int? SeoScore { get; set; }
}