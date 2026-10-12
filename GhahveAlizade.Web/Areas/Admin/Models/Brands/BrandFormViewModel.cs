using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Admin.Models.Brands;

public class BrandFormViewModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "نام برند الزامی است.")]
    [StringLength(300)]
    [Display(Name = "نام برند")]
    public string Name { get; set; } = string.Empty;

    [StringLength(300)]
    [Display(Name = "نامک (Slug)")]
    public string? Slug { get; set; }

    [StringLength(2000)]
    [Display(Name = "توضیحات")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [StringLength(1000)]
    [Display(Name = "لوگو")]
    [Url]
    public string? LogoUrl { get; set; }

    [StringLength(500)]
    [Display(Name = "وب‌سایت")]
    [Url]
    public string? WebsiteUrl { get; set; }

    [StringLength(100)]
    [Display(Name = "کشور مبدأ")]
    public string? CountryOfOrigin { get; set; }

    [Range(0, int.MaxValue)]
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

    [StringLength(1000)]
    [Display(Name = "Canonical URL")]
    public string? CanonicalUrl { get; set; }

    [StringLength(1000)]
    [Display(Name = "تصویر OG")]
    public string? OgImage { get; set; }
}