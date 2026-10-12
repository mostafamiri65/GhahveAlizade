using GhahveAlizade.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Persistence.Seed;

public class SettingSeeder
{
	private readonly AppDbContext _context;
	private readonly ILogger<SettingSeeder> _logger;

	public SettingSeeder(AppDbContext context, ILogger<SettingSeeder> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task SeedAsync(CancellationToken ct = default)
	{
		if (await _context.AppSettings.AnyAsync(ct)) return;

		var settings = new List<AppSetting>
		{
            // General
            new() { Key = "site.name", Value = "قهوه علیزاده", Description = "نام سایت", Group = "General", DisplayOrder = 1 },
			new() { Key = "site.tagline", Value = "فروشگاه تخصصی قهوه", Description = "شعار سایت", Group = "General", DisplayOrder = 2 },
			new() { Key = "site.logo", Value = "/images/logo.png", Description = "لوگو", Group = "General", DisplayOrder = 3 },
			new() { Key = "site.currency", Value = "IRT", Description = "واحد پول", Group = "General", DisplayOrder = 4 },

            // Contact
            new() { Key = "contact.phone", Value = "021-12345678", Description = "تلفن", Group = "Contact", DisplayOrder = 1 },
			new() { Key = "contact.email", Value = "info@ghahvealizade.ir", Description = "ایمیل", Group = "Contact", DisplayOrder = 2 },
			new() { Key = "contact.address", Value = "تهران، خیابان ولیعصر", Description = "آدرس", Group = "Contact", DisplayOrder = 3 },
			new() { Key = "contact.working_hours", Value = "شنبه تا پنج‌شنبه، ۹ صبح تا ۹ شب", Description = "ساعات کاری", Group = "Contact", DisplayOrder = 4 },

            // Social
            new() { Key = "social.instagram", Value = "", Description = "اینستاگرام", Group = "Social", DisplayOrder = 1 },
			new() { Key = "social.telegram", Value = "", Description = "تلگرام", Group = "Social", DisplayOrder = 2 },
			new() { Key = "social.whatsapp", Value = "", Description = "واتساپ", Group = "Social", DisplayOrder = 3 },

            // Shipping
            new() { Key = "shipping.free_threshold", Value = "500000", Description = "سقف ارسال رایگان (تومان)", Group = "Shipping", ValueType = "Decimal", DisplayOrder = 1 },

            // Payment
            new() { Key = "payment.zarinpal.merchant_id", Value = "", Description = "Merchant ID زرین‌پال", Group = "Payment", DisplayOrder = 1 },
			new() { Key = "payment.zarinpal.enabled", Value = "false", Description = "فعال بودن زرین‌پال", Group = "Payment", ValueType = "Bool", DisplayOrder = 2 },
			new() { Key = "payment.cod.enabled", Value = "true", Description = "پرداخت در محل", Group = "Payment", ValueType = "Bool", DisplayOrder = 3 },

            // SMS
            new() { Key = "sms.kavenegar.api_key", Value = "", Description = "API Key کاوه‌نگار", Group = "Sms", DisplayOrder = 1 },
			new() { Key = "sms.kavenegar.enabled", Value = "false", Description = "فعال بودن پیامک", Group = "Sms", ValueType = "Bool", DisplayOrder = 2 },

            // Email
            new() { Key = "email.smtp.host", Value = "", Description = "SMTP Host", Group = "Email", DisplayOrder = 1 },
			new() { Key = "email.smtp.port", Value = "587", Description = "SMTP Port", Group = "Email", ValueType = "Int", DisplayOrder = 2 },
			new() { Key = "email.smtp.username", Value = "", Description = "SMTP Username", Group = "Email", DisplayOrder = 3 },
			new() { Key = "email.smtp.password", Value = "", Description = "SMTP Password", Group = "Email", DisplayOrder = 4 },
		};

		await _context.AppSettings.AddRangeAsync(settings, ct);
		await _context.SaveChangesAsync(ct);

		_logger.LogInformation("Seeded {Count} settings.", settings.Count);
	}
}