using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Admin.Models.Inventory;

public class BranchFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "نام شعبه الزامی است.")]
	[StringLength(200)]
	[Display(Name = "نام شعبه")]
	public string Name { get; set; } = string.Empty;

	[Required(ErrorMessage = "کد شعبه الزامی است.")]
	[StringLength(50)]
	[Display(Name = "کد شعبه")]
	public string Code { get; set; } = string.Empty;

	[StringLength(1000)]
	[Display(Name = "آدرس")]
	[DataType(DataType.MultilineText)]
	public string? Address { get; set; }

	[StringLength(100)]
	[Display(Name = "شهر")]
	public string? City { get; set; }

	[StringLength(100)]
	[Display(Name = "استان")]
	public string? Province { get; set; }

	[StringLength(20)]
	[Display(Name = "کد پستی")]
	public string? PostalCode { get; set; }

	[StringLength(20)]
	[Display(Name = "تلفن")]
	public string? Phone { get; set; }

	[Range(-90, 90, ErrorMessage = "عرض جغرافیایی باید بین -90 و 90 باشد.")]
	[Display(Name = "عرض جغرافیایی")]
	public decimal? Latitude { get; set; }

	[Range(-180, 180, ErrorMessage = "طول جغرافیایی باید بین -180 و 180 باشد.")]
	[Display(Name = "طول جغرافیایی")]
	public decimal? Longitude { get; set; }

	[StringLength(500)]
	[Display(Name = "ساعات کاری")]
	public string? WorkingHours { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[Display(Name = "شعبه پیش‌فرض")]
	public bool IsDefault { get; set; }

	[Range(0, int.MaxValue)]
	[Display(Name = "ترتیب نمایش")]
	public int DisplayOrder { get; set; }
}