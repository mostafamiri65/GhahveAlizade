using System.ComponentModel.DataAnnotations;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Web.Areas.Admin.Models.Attributes;

public class AttributeFormViewModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "نام ویژگی الزامی است.")]
    [StringLength(200)]
    [Display(Name = "نام ویژگی")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    [Display(Name = "نامک (Slug)")]
    public string? Slug { get; set; }

    [Required(ErrorMessage = "نوع ویژگی الزامی است.")]
    [Display(Name = "نوع")]
    public AttributeType Type { get; set; } = AttributeType.Select;

    [StringLength(50)]
    [Display(Name = "واحد")]
    public string? Unit { get; set; }

    [Display(Name = "قابل فیلتر (نمایش در فیلترها)")]
    public bool IsFilterable { get; set; } = true;

    [Display(Name = "اجباری در محصول")]
    public bool IsRequired { get; set; }

    [Display(Name = "استفاده در واریانت‌ها")]
    public bool IsVariant { get; set; }

    [Range(0, int.MaxValue)]
    [Display(Name = "ترتیب نمایش")]
    public int DisplayOrder { get; set; }

    [Display(Name = "مقادیر")]
    public List<AttributeValueViewModel> Values { get; set; } = new();
}

public class AttributeValueViewModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "مقدار الزامی است.")]
    [StringLength(500)]
    public string? Value { get; set; }

    [StringLength(500)]
    [Display(Name = "نمایش (اختیاری)")]
    public string? DisplayValue { get; set; }

    [StringLength(20)]
    [Display(Name = "کد رنگ")]
    public string? ColorCode { get; set; }

    [Display(Name = "ترتیب")]
    public int DisplayOrder { get; set; }
}