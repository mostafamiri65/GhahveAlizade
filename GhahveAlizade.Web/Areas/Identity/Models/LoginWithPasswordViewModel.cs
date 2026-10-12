using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Identity.Models;

public class LoginWithPasswordViewModel
{
	[Required(ErrorMessage = "نام کاربری یا ایمیل الزامی است.")]
	[Display(Name = "نام کاربری یا ایمیل")]
	public string UsernameOrEmail { get; set; } = string.Empty;

	[Required(ErrorMessage = "رمز عبور الزامی است.")]
	[DataType(DataType.Password)]
	[Display(Name = "رمز عبور")]
	public string Password { get; set; } = string.Empty;

	[Display(Name = "مرا به خاطر بسپار")]
	public bool RememberMe { get; set; } = true;

	public string? ReturnUrl { get; set; }
}