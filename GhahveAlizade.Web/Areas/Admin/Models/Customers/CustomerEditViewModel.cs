using System.ComponentModel.DataAnnotations;
using GhahveAlizade.Application.Features.Customers.Dtos;

namespace GhahveAlizade.Web.Areas.Admin.Models.Customers;

public class CustomerEditViewModel
{
	public long Id { get; set; }

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
	public DateOnly? BirthDate { get; set; }

	[StringLength(20)]
	[Display(Name = "جنسیت")]
	public string? Gender { get; set; }

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[Display(Name = "تایید شده")]
	public bool IsVerified { get; set; }

	[Display(Name = "پذیرش بازاریابی")]
	public bool AcceptsMarketing { get; set; }

	[Display(Name = "گروه مشتری")]
	public long? CustomerGroupId { get; set; }

	public List<CustomerGroupDto> Groups { get; set; } = new();
}