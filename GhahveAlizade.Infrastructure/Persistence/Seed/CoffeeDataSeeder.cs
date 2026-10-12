using GhahveAlizade.Domain.Coffee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Persistence.Seed;

public class CoffeeDataSeeder
{
	private readonly AppDbContext _context;
	private readonly ILogger<CoffeeDataSeeder> _logger;

	public CoffeeDataSeeder(AppDbContext context, ILogger<CoffeeDataSeeder> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		await SeedOriginsAsync(cancellationToken);
		await SeedProcessesAsync(cancellationToken);
		await SeedRoastProfilesAsync(cancellationToken);
		await SeedFlavorNotesAsync(cancellationToken);

		_logger.LogInformation("Coffee data seeded successfully.");
	}

	private async Task SeedOriginsAsync(CancellationToken ct)
	{
		if (await _context.CoffeeOrigins.AnyAsync(ct)) return;

		var origins = new List<CoffeeOrigin>
		{
			new() { Name = "اتیوپی", Slug = "ethiopia", CountryCode = "ET", Description = "خاستگاه اصلی قهوه عربیکا با طعم‌های میوه‌ای و گل‌دار", DisplayOrder = 1 },
			new() { Name = "کلمبیا", Slug = "colombia", CountryCode = "CO", Description = "تعادل عالی بین اسیدیته و بدنه", DisplayOrder = 2 },
			new() { Name = "برزیل", Slug = "brazil", CountryCode = "BR", Description = "بزرگترین تولیدکننده قهوه با طعم‌های آجیلی و شکلاتی", DisplayOrder = 3 },
			new() { Name = "کنیا", Slug = "kenya", CountryCode = "KE", Description = "اسیدیته بالا و طعم‌های توت‌مانند", DisplayOrder = 4 },
			new() { Name = "گواتمالا", Slug = "guatemala", CountryCode = "GT", Description = "طعم شکلاتی و ادویه‌ای", DisplayOrder = 5 },
			new() { Name = "کاستاریکا", Slug = "costa-rica", CountryCode = "CR", Description = "طعم‌های شیرین و تمیز", DisplayOrder = 6 },
			new() { Name = "پاناما", Slug = "panama", CountryCode = "PA", Description = "خاستگاه قهوه‌های لوکس گیشا", DisplayOrder = 7 },
			new() { Name = "اندونزی", Slug = "indonesia", CountryCode = "ID", Description = "بدنه سنگین و طعم خاکی", DisplayOrder = 8 },
			new() { Name = "رواندا", Slug = "rwanda", CountryCode = "RW", Description = "طعم‌های میوه‌ای و گلی", DisplayOrder = 9 },
			new() { Name = "هندوراس", Slug = "honduras", CountryCode = "HN", Description = "شیرینی کاراملی و میوه‌ای", DisplayOrder = 10 },
			new() { Name = "السالوادور", Slug = "el-salvador", CountryCode = "SV", Description = "شیرینی و تعادل", DisplayOrder = 11 },
			new() { Name = "پرو", Slug = "peru", CountryCode = "PE", Description = "قهوه‌های ارگانیک با طعم ملایم", DisplayOrder = 12 },
			new() { Name = "یمن", Slug = "yemen", CountryCode = "YE", Description = "خاستگاه تاریخی قهوه موکا", DisplayOrder = 13 },
			new() { Name = "ایران", Slug = "iran", CountryCode = "IR", Description = "قهوه‌های محلی ایران", DisplayOrder = 14 }
		};

		await _context.CoffeeOrigins.AddRangeAsync(origins, ct);
		await _context.SaveChangesAsync(ct);
		_logger.LogInformation("Seeded {Count} coffee origins.", origins.Count);
	}

	private async Task SeedProcessesAsync(CancellationToken ct)
	{
		if (await _context.CoffeeProcesses.AnyAsync(ct)) return;

		var processes = new List<CoffeeProcess>
		{
			new() { Name = "شسته (Washed)", Slug = "washed", Description = "طعم تمیز و شفاف با اسیدیته بالا", DisplayOrder = 1 },
			new() { Name = "طبیعی (Natural)", Slug = "natural", Description = "طعم میوه‌ای شدید و شیرین", DisplayOrder = 2 },
			new() { Name = "هانی (Honey)", Slug = "honey", Description = "تعادل بین شسته و طبیعی، شیرینی عسلی", DisplayOrder = 3 },
			new() { Name = "نیمه‌شسته (Semi-Washed)", Slug = "semi-washed", Description = "بدنه سنگین‌تر از شسته", DisplayOrder = 4 },
			new() { Name = "آنائروبیک (Anaerobic)", Slug = "anaerobic", Description = "تخمیر بدون اکسیژن، طعم‌های پیچیده", DisplayOrder = 5 },
			new() { Name = "کربونیک (Carbonic Maceration)", Slug = "carbonic", Description = "تخمیر با CO2، طعم‌های وینی", DisplayOrder = 6 },
			new() { Name = "گانیشا (Giling Basah)", Slug = "giling-basah", Description = "روش اندونزیایی با بدنه سنگین", DisplayOrder = 7 }
		};

		await _context.CoffeeProcesses.AddRangeAsync(processes, ct);
		await _context.SaveChangesAsync(ct);
		_logger.LogInformation("Seeded {Count} coffee processes.", processes.Count);
	}

	private async Task SeedRoastProfilesAsync(CancellationToken ct)
	{
		if (await _context.RoastProfiles.AnyAsync(ct)) return;

		var roasts = new List<RoastProfile>
		{
			new() { Name = "روشن (Light)", Slug = "light", Description = "اسیدیته بالا، حفظ طعم اصلی دانه", RoastLevel = 2, ColorCode = "#C8956D", DisplayOrder = 1 },
			new() { Name = "روشن-متوسط (Light-Medium)", Slug = "light-medium", Description = "تعادل بین اسیدیته و شیرینی", RoastLevel = 4, ColorCode = "#B0754C", DisplayOrder = 2 },
			new() { Name = "متوسط (Medium)", Slug = "medium", Description = "محبوب‌ترین رست، تعادل کامل", RoastLevel = 5, ColorCode = "#8B5A2B", DisplayOrder = 3 },
			new() { Name = "متوسط-تیره (Medium-Dark)", Slug = "medium-dark", Description = "شیرینی کاراملی، بدنه سنگین", RoastLevel = 7, ColorCode = "#6B3F1F", DisplayOrder = 4 },
			new() { Name = "تیره (Dark)", Slug = "dark", Description = "طعم دودی و تلخ، مناسب اسپرسو", RoastLevel = 9, ColorCode = "#3D2418", DisplayOrder = 5 },
			new() { Name = "اسپرسو (Espresso)", Slug = "espresso", Description = "مخصوص دستگاه اسپرسو", RoastLevel = 6, ColorCode = "#5C3A21", DisplayOrder = 6 }
		};

		await _context.RoastProfiles.AddRangeAsync(roasts, ct);
		await _context.SaveChangesAsync(ct);
		_logger.LogInformation("Seeded {Count} roast profiles.", roasts.Count);
	}

	private async Task SeedFlavorNotesAsync(CancellationToken ct)
	{
		if (await _context.FlavorNotes.AnyAsync(ct)) return;

		var notes = new List<FlavorNote>
		{
            // شکلاتی
            new() { Name = "شکلات تلخ", Slug = "dark-chocolate", Category = "Chocolate", ColorCode = "#3E2723", DisplayOrder = 1 },
			new() { Name = "کاکائو", Slug = "cocoa", Category = "Chocolate", ColorCode = "#4E342E", DisplayOrder = 2 },
			new() { Name = "شکلات شیری", Slug = "milk-chocolate", Category = "Chocolate", ColorCode = "#6D4C41", DisplayOrder = 3 },
			new() { Name = "کارامel", Slug = "caramel", Category = "Sweet", ColorCode = "#D4A574", DisplayOrder = 4 },

            // میوه‌ای
            new() { Name = "توت‌فرنگی", Slug = "strawberry", Category = "Fruity", ColorCode = "#E57373", DisplayOrder = 5 },
			new() { Name = "بلوبری", Slug = "blueberry", Category = "Fruity", ColorCode = "#5C6BC0", DisplayOrder = 6 },
			new() { Name = "گیلاس", Slug = "cherry", Category = "Fruity", ColorCode = "#C62828", DisplayOrder = 7 },
			new() { Name = "لیمو", Slug = "lemon", Category = "Citrus", ColorCode = "#FDD835", DisplayOrder = 8 },
			new() { Name = "پرتقال", Slug = "orange", Category = "Citrus", ColorCode = "#FB8C00", DisplayOrder = 9 },
			new() { Name = "گریپ‌فروت", Slug = "grapefruit", Category = "Citrus", ColorCode = "#EF6C00", DisplayOrder = 10 },
			new() { Name = "هلو", Slug = "peach", Category = "Fruity", ColorCode = "#FFAB91", DisplayOrder = 11 },
			new() { Name = "زردآلو", Slug = "apricot", Category = "Fruity", ColorCode = "#FFCC80", DisplayOrder = 12 },

            // آجیلی
            new() { Name = "بادام", Slug = "almond", Category = "Nutty", ColorCode = "#D7CCC8", DisplayOrder = 13 },
			new() { Name = "فندق", Slug = "hazelnut", Category = "Nutty", ColorCode = "#A1887F", DisplayOrder = 14 },
			new() { Name = "گردو", Slug = "walnut", Category = "Nutty", ColorCode = "#6D4C41", DisplayOrder = 15 },

            // گلی
            new() { Name = "یاس", Slug = "jasmine", Category = "Floral", ColorCode = "#F8BBD0", DisplayOrder = 16 },
			new() { Name = "گل رز", Slug = "rose", Category = "Floral", ColorCode = "#F06292", DisplayOrder = 17 },
			new() { Name = "اسطوخودوس", Slug = "lavender", Category = "Floral", ColorCode = "#9575CD", DisplayOrder = 18 },

            // ادویه‌ای
            new() { Name = "دارچین", Slug = "cinnamon", Category = "Spicy", ColorCode = "#A1887F", DisplayOrder = 19 },
			new() { Name = "هل", Slug = "cardamom", Category = "Spicy", ColorCode = "#81C784", DisplayOrder = 20 },
			new() { Name = "جوز هندی", Slug = "nutmeg", Category = "Spicy", ColorCode = "#8D6E63", DisplayOrder = 21 },

            // سایر
            new() { Name = "عسل", Slug = "honey", Category = "Sweet", ColorCode = "#FFC107", DisplayOrder = 22 },
			new() { Name = "شکر قهوه‌ای", Slug = "brown-sugar", Category = "Sweet", ColorCode = "#BCAAA4", DisplayOrder = 23 },
			new() { Name = "وانیل", Slug = "vanilla", Category = "Sweet", ColorCode = "#FFECB3", DisplayOrder = 24 },
			new() { Name = "تنباکو", Slug = "tobacco", Category = "Other", ColorCode = "#795548", DisplayOrder = 25 },
			new() { Name = "چرم", Slug = "leather", Category = "Other", ColorCode = "#5D4037", DisplayOrder = 26 }
		};

		await _context.FlavorNotes.AddRangeAsync(notes, ct);
		await _context.SaveChangesAsync(ct);
		_logger.LogInformation("Seeded {Count} flavor notes.", notes.Count);
	}
}