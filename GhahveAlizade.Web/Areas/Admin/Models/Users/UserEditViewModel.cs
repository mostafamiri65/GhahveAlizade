using System.ComponentModel.DataAnnotations;

namespace GhahveAlizade.Web.Areas.Admin.Models.Users;

public class UserEditViewModel
{
	public long Id { get; set; }

	public string UserName { get; set; } = string.Empty;

	[StringLength(200)]
	[Display(Name = "نام")]
	public string? FirstName { get; set; }

	[StringLength(200)]
	[Display(Name = "نام خانوادگی")]
	public string? LastName { get; set; }

	[EmailAddress(ErrorMessage = "ایمیل معتبر نیست.")]
	[StringLength(256)]
	[Display(Name = "ایمیل")]
	public string? Email { get; set; }

	[RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "شماره موبایل معتبر نیست.")]
	[Display(Name = "موبایل")]
	public string? PhoneNumber { get; set; }

	[StringLength(20)]
	[Display(Name = "کد ملی")]
	public string? NationalCode { get; set; }

	[Display(Name = "تاریخ تولد")]
	[DataType(DataType.Date)]
	public DateOnly? BirthDate { get; set; }

	[StringLength(20)]
	[Display(Name = "جنسیت")]
	public string? Gender { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;
}