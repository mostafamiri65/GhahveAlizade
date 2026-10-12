using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Identity.Models;

public class VerifyOtpViewModel
{
	[Required]
	public string PhoneNumber { get; set; } = string.Empty;

	[Required(ErrorMessage = "کد تایید الزامی است.")]
	[StringLength(6, MinimumLength = 6, ErrorMessage = "کد تایید باید 6 رقم باشد.")]
	[RegularExpression(@"^[0-9]{6}$", ErrorMessage = "کد تایید فقط شامل اعداد است.")]
	[Display(Name = "کد تایید")]
	public string Code { get; set; } = string.Empty;

	public string? ReturnUrl { get; set; }

	public int RemainingSeconds { get; set; } = 180;
}