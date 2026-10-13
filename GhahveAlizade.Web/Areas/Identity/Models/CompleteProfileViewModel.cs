using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Identity.Models;

/// <summary>
/// فرم تکمیل مشخصات بعد از تأیید OTP (کاربر جدید یا ناقص).
/// </summary>
public class CompleteProfileViewModel
{
	[Required(ErrorMessage = "نام الزامی است")]
	[StringLength(100)]
	[Display(Name = "نام")]
	public string FirstName { get; set; } = string.Empty;

	[Required(ErrorMessage = "نام خانوادگی الزامی است")]
	[StringLength(100)]
	[Display(Name = "نام خانوادگی")]
	public string LastName { get; set; } = string.Empty;

	[EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
	[Display(Name = "ایمیل (اختیاری)")]
	public string? Email { get; set; }

	[StringLength(10, MinimumLength = 10, ErrorMessage = "کد ملی باید ۱۰ رقم باشد")]
	[RegularExpression(@"^\d{10}$", ErrorMessage = "کد ملی باید ۱۰ رقم عددی باشد")]
	[Display(Name = "کد ملی (اختیاری)")]
	public string? NationalCode { get; set; }

	[Display(Name = "تاریخ تولد (اختیاری)")]
	public DateOnly? BirthDate { get; set; }

	[Display(Name = "جنسیت")]
	public string? Gender { get; set; }

	public string? ReturnUrl { get; set; }
}
