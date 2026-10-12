using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Settings;

public class AppSetting : BaseEntity
{
	public string Key { get; set; } = string.Empty;
	public string? Value { get; set; }
	public string? Description { get; set; }
	public string? Group { get; set; } // General, Payment, Shipping, Seo, ...
	public string ValueType { get; set; } = "String"; // String, Int, Bool, Decimal, Json
	public bool IsPublic { get; set; } // قابل نمایش در سایت
	public int DisplayOrder { get; set; }
}