namespace GhahveAlizade.Infrastructure.Seo;

public class SeoOptions
{
	public const string SectionName = "Seo";

	public string SiteBaseUrl { get; set; } = "https://ghahvealizade.ir";
	public string SiteName { get; set; } = "قهوه علیزاده";
	public string DefaultMetaTitle { get; set; } = "قهوه علیزاده | فروشگاه تخصصی قهوه";
	public string DefaultMetaDescription { get; set; } = "فروشگاه تخصصی قهوه علیزاده";
	public string DefaultOgImage { get; set; } = "/images/og-default.jpg";
	public string TwitterHandle { get; set; } = "@ghahvealizade";
	public string? GoogleSiteVerification { get; set; }
	public string? GoogleAnalyticsId { get; set; }
	public string? GoogleTagManagerId { get; set; }
	public string? SearchConsoleApiKey { get; set; }

	// سازمان
	public string OrganizationLegalName { get; set; } = "قهوه علیزاده";
	public string OrganizationLogo { get; set; } = "/images/logo.png";
	public string? OrganizationPhone { get; set; } = "+982112345678";
	public string? OrganizationEmail { get; set; } = "info@ghahvealizade.ir";
	public string? OrganizationAddress { get; set; } = "تهران، ...";
	public string? OrganizationCity { get; set; } = "تهران";
	public string? OrganizationCountry { get; set; } = "IR";

	// شبکه‌های اجتماعی
	public List<string> SocialProfiles { get; set; } = new();
}