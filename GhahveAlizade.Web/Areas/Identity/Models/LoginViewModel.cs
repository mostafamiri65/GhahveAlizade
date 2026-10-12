using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Identity.Models;

public class LoginViewModel
{
	[Required(ErrorMessage = "شماره موبایل الزامی است.")]
	[RegularExpression(@"^09[0-9]{9}$",
		ErrorMessage = "شماره موبایل باید با 09 شروع شده و 11 رقم باشد.")]
	[Display(Name = "شماره موبایل")]
	public string PhoneNumber { get; set; } = string.Empty;

	public string? ReturnUrl { get; set; }
}