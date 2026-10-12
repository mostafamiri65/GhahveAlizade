using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Identity.Models;

public class SetPasswordViewModel
{
	[Required(ErrorMessage = "رمز عبور جدید الزامی است.")]
	[StringLength(100, MinimumLength = 8,
		ErrorMessage = "رمز عبور باید حداقل 8 کاراکتر باشد.")]
	[DataType(DataType.Password)]
	[Display(Name = "رمز عبور جدید")]
	public string NewPassword { get; set; } = string.Empty;

	[Required(ErrorMessage = "تکرار رمز عبور الزامی است.")]
	[DataType(DataType.Password)]
	[Compare(nameof(NewPassword), ErrorMessage = "رمز عبور و تکرار آن یکسان نیستند.")]
	[Display(Name = "تکرار رمز عبور")]
	public string ConfirmPassword { get; set; } = string.Empty;
}