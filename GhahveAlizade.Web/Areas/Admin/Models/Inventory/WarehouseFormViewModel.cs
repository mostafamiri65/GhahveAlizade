using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Admin.Models.Inventory;

public class WarehouseFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "نام انبار الزامی است.")]
	[StringLength(200)]
	[Display(Name = "نام انبار")]
	public string Name { get; set; } = string.Empty;

	[Required(ErrorMessage = "کد انبار الزامی است.")]
	[StringLength(50)]
	[Display(Name = "کد انبار")]
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

	[StringLength(200)]
	[Display(Name = "نام مدیر")]
	public string? ManagerName { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[Display(Name = "انبار پیش‌فرض")]
	public bool IsDefault { get; set; }
}