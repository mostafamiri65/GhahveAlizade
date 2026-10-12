using System.ComponentModel.DataAnnotations;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Web.Areas.Admin.Models.Seo;

public class RedirectFormViewModel
{
	public long Id { get; set; }

	[Required(ErrorMessage = "مسیر مبدأ الزامی است.")]
	[StringLength(2000)]
	[Display(Name = "مسیر مبدأ")]
	public string FromPath { get; set; } = string.Empty;

	[Required(ErrorMessage = "مسیر مقصد الزامی است.")]
	[StringLength(2000)]
	[Display(Name = "مسیر مقصد")]
	public string ToPath { get; set; } = string.Empty;

	[Required]
	[Display(Name = "نوع ریدایرکت")]
	public RedirectType Type { get; set; } = RedirectType.Permanent301;

	[Display(Name = "فعال")]
	public bool IsActive { get; set; } = true;

	[StringLength(1000)]
	[Display(Name = "یادداشت")]
	public string? Notes { get; set; }
}