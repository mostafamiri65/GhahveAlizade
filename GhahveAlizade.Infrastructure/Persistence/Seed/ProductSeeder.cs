using GhahveAlizade.Domain.Catalog;
using GhahveAlizade.Domain.Coffee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Persistence.Seed;

/// <summary>
/// دیتای دمو فروشگاه: برندها، محصولات قهوه با واریانت/تصویر اختصاصی.
/// Idempotent — فقط اگر هیچ محصولی نباشد اجرا می‌شود.
/// </summary>
public class ProductSeeder
{
	private readonly AppDbContext _context;
	private readonly ILogger<ProductSeeder> _logger;

	public ProductSeeder(AppDbContext context, ILogger<ProductSeeder> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		if (await _context.Products.IgnoreQueryFilters().AnyAsync(cancellationToken))
		{
			_logger.LogInformation("Products already exist; skipping product seed.");
			return;
		}

		var category = await _context.Categories.IgnoreQueryFilters()
			.OrderBy(c => c.DisplayOrder).FirstOrDefaultAsync(cancellationToken);
		var brand = await _context.Brands.IgnoreQueryFilters()
			.OrderBy(b => b.DisplayOrder).FirstOrDefaultAsync(cancellationToken);
		var origins = await _context.CoffeeOrigins.IgnoreQueryFilters().ToListAsync(cancellationToken);
		var processes = await _context.CoffeeProcesses.IgnoreQueryFilters().ToListAsync(cancellationToken);
		var roasts = await _context.RoastProfiles.IgnoreQueryFilters().ToListAsync(cancellationToken);
		var flavorNotes = await _context.FlavorNotes.IgnoreQueryFilters().ToListAsync(cancellationToken);

		var products = new List<Product>
		{
			new()
			{
				Name = "قهوه اتیوپی ییرگاچف", Slug = "ethiopia-yirgacheffe",
				ShortDescription = "اسیدیته روشن، عطر یاس و طعم بلوبری — یکی از بهترین عربیکاهای دنیا.",
				Description = "<p>قهوه <strong>اتیوپی ییرگاچف</strong> از ارتفاعات ۱۹۰۰ متری جنوب اتیوپی می‌آید. با پروسس شسته، طعمی تمیز و شفاف با عطر یاس، طعم بلوبری و شیرینی لیمو دارد. انتخاب اول عاشقان قهوه فیلتر و پورآور.</p><p><strong>پیشنهاد دم‌آوری:</strong> V60 با نسبت ۱ به ۱۶ و آب ۹۳ درجه.</p>",
				Sku = "GA-YIRG-250", HasVariants = true,
				CategoryId = category?.Id, BrandId = brand?.Id,
				CoffeeOriginId = FindByName(origins, "اتیوپی")?.Id, CoffeeProcessId = FindByName(processes, "شسته")?.Id, RoastProfileId = FindByName(roasts, "روشن")?.Id,
				IsActive = true, IsPublished = true, IsFeatured = true, IsNew = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-2),
				MetaTitle = "خرید قهوه اتیوپی ییرگاچف | قهوه علیزاده",
				MetaDescription = "قهوه اتیوپی ییرگاچف تک‌خاستگاه با عطر یاس و طعم بلوبری، تازه رست‌شده.",
				Weight = 0.25m, ViewCount = 480, SoldCount = 95, AverageRating = 4.8m, ReviewCount = 24,
			},
			new()
			{
				Name = "قهوه کلمبیا هوئلا", Slug = "colombia-huila",
				ShortDescription = "تعادل کامل؛ کارامل، فندق و اسیدیته ملایم مرکبات.",
				Description = "<p>از منطقه معروف <strong>هوئلا</strong> کلمبیا؛ قهوه‌ای با بدنه متوسط، شیرینی کارامل، فندق و نوک اسیدیته پرتقالی. برای هر روش دم‌آوری جواب می‌دهد.</p>",
				Sku = "GA-HUIL-250", HasVariants = true,
				CategoryId = category?.Id, BrandId = brand?.Id,
				CoffeeOriginId = FindByName(origins, "کلمبیا")?.Id, CoffeeProcessId = FindByName(processes, "شسته")?.Id, RoastProfileId = FindByName(roasts, "متوسط")?.Id,
				IsActive = true, IsPublished = true, IsFeatured = true, IsBestSeller = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-6),
				MetaTitle = "خرید قهوه کلمبیا هوئلا | قهوه علیزاده",
				MetaDescription = "قهوه کلمبیا هوئلا با تعادل عالی، طعم کارامل و فندق.",
				Weight = 0.25m, ViewCount = 720, SoldCount = 180, AverageRating = 4.6m, ReviewCount = 41,
			},
			new()
			{
				Name = "بلند اسپرسو علیزاده", Slug = "alizade-espresso-blend",
				ShortDescription = "بلند خورده برزیل و هندوراس؛ شکلات تلخ و کرمای غنی.",
				Description = "<p>بلند اختصاصی ما برای <strong>اسپرسو و کاپوچینو</strong>؛ ترکیب عربیکای برزیل و روبوستای هندوراس با رُست متوسط-تیره. کرمای غنی، شکلات تلخ و ماندگاری عالی.</p><p><strong>دستور اسپرسو:</strong> ۱۸ گرم، خروجی ۳۶ گرم در ۲۸ ثانیه.</p>",
				Sku = "GA-ESP-500", HasVariants = true,
				CategoryId = category?.Id, BrandId = brand?.Id,
				CoffeeOriginId = FindByName(origins, "برزیل")?.Id, CoffeeProcessId = FindByName(processes, "طبیعی")?.Id, RoastProfileId = FindByName(roasts, "متوسط-تیره")?.Id,
				IsActive = true, IsPublished = true, IsBestSeller = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-30),
				MetaTitle = "خرید بلند اسپرسو | قهوه علیزاده",
				MetaDescription = "بلند اسپرسو اختصاصی با کرمای غنی و طعم شکلات تلخ.",
				Weight = 0.5m, ViewCount = 1100, SoldCount = 320, AverageRating = 4.9m, ReviewCount = 67,
			},
			new()
			{
				Name = "قهوه کنیا AA", Slug = "kenya-aa",
				ShortDescription = "اسیدیته جسورانه، طعم گیلاس و گریپ‌فروت.",
				Description = "<p><strong>کنیا AA</strong> درجه‌بندی بالاترین کیفیت کنیایی است؛ اسیدیته جسورانه و آبدار با طعم گیلاس، گریپ‌فروت و نکته‌های شیرین. برای قهوه‌شناس‌هایی که اسیدیته دوست دارند.</p>",
				Sku = "GA-KEN-250", HasVariants = true,
				CategoryId = category?.Id, BrandId = brand?.Id,
				CoffeeOriginId = FindByName(origins, "کنیا")?.Id, CoffeeProcessId = FindByName(processes, "شسته")?.Id, RoastProfileId = FindByName(roasts, "روشن")?.Id,
				IsActive = true, IsPublished = true, IsNew = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-1),
				MetaTitle = "خرید قهوه کنیا AA | قهوه علیزاده",
				MetaDescription = "قهوه کنیا AA با اسیدیته بالا و طعم گیلاس و گریپ‌فروت.",
				Weight = 0.25m, ViewCount = 310, SoldCount = 58, AverageRating = 4.7m, ReviewCount = 19,
			},
			new()
			{
				Name = "قهوه برزیل سانتوس", Slug = "brazil-santos",
				ShortDescription = "بدنه سنگین، شکلات شیری و بادام — مناسب موکاپات.",
				Description = "<p>قهوه <strong>برزیل سانتوس</strong> با پروسس طبیعی؛ بدنه سنگین، شکلات شیری، بادام و شیرینی کم. انتخابی عالی برای موکاپات و قهوه‌های شیرین.</p>",
				Sku = "GA-BRZ-250", HasVariants = true,
				CategoryId = category?.Id, BrandId = brand?.Id,
				CoffeeOriginId = FindByName(origins, "برزیل")?.Id, CoffeeProcessId = FindByName(processes, "طبیعی")?.Id, RoastProfileId = FindByName(roasts, "متوسط")?.Id,
				IsActive = true, IsPublished = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-45),
				MetaTitle = "خرید قهوه برزیل سانتوس | قهوه علیزاده",
				MetaDescription = "قهوه برزیل سانتوس طبیعی با بدنه سنگین و شکلات شیری.",
				Weight = 0.25m, ViewCount = 560, SoldCount = 140, AverageRating = 4.4m, ReviewCount = 33,
			},
			new()
			{
				Name = "قهوه گواتمالا آنتیگوا", Slug = "guatemala-antigua",
				ShortDescription = "شکلات تلخ، ادویه و نکته دودی ملایم.",
				Description = "<p>از دره آتشفشانی <strong>آنتیگوا</strong>؛ شکلات تلخ، دارچین و نکته دودی ملایم با بدنه کرمی. مناسب قهوه‌های تلخ‌پسند.</p>",
				Sku = "GA-GUA-250", HasVariants = true,
				CategoryId = category?.Id, BrandId = brand?.Id,
				CoffeeOriginId = FindByName(origins, "گواتمالا")?.Id, CoffeeProcessId = FindByName(processes, "شسته")?.Id, RoastProfileId = FindByName(roasts, "تیره")?.Id,
				IsActive = true, IsPublished = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-20),
				MetaTitle = "خرید قهوه گواتمالا آنتیگوا | قهوه علیزاده",
				MetaDescription = "قهوه گواتمالا آنتیگوا با شکلات تلخ و نکته دودی.",
				Weight = 0.25m, ViewCount = 420, SoldCount = 88, AverageRating = 4.5m, ReviewCount = 27,
			},
			new()
			{
				Name = "قهوه پاناما گیشا", Slug = "panama-geisha",
				ShortDescription = "لوکس‌ترین قهوه دنیا؛ گل رز، هلو و لیمو — بچ محدود.",
				Description = "<p><strong>پاناما گیشا</strong> افسانه دنیای قهوه؛ عطر گل رز، طعم هلو و لیمو با شیرینی عسلی. بچ محدود — ویژه مجموعه‌داران و قهوه‌شناس‌ها.</p>",
				Sku = "GA-GEI-100", HasVariants = false, BasePrice = 1_950_000m, CompareAtPrice = 2_300_000m,
				CategoryId = category?.Id, BrandId = brand?.Id,
				CoffeeOriginId = FindByName(origins, "پاناما")?.Id, CoffeeProcessId = FindByName(processes, "شسته")?.Id, RoastProfileId = FindByName(roasts, "روشن")?.Id,
				IsActive = true, IsPublished = true, IsFeatured = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-3),
				MetaTitle = "خرید قهوه پاناما گیشا | قهوه علیزاده",
				MetaDescription = "قهوه لوکس پاناما گیشا با عطر گل رز و طعم هلو.",
				Weight = 0.1m, ViewCount = 1900, SoldCount = 15, AverageRating = 5.0m, ReviewCount = 9,
			},
			new()
			{
				Name = "قهوه هانی پروسس کاستاریکا", Slug = "costa-rica-honey",
				ShortDescription = "شیرینی عسلی، زردآلو و بدنه کرمی.",
				Description = "<p>پروسس <strong>هانی</strong> کاستاریکا؛ بین شسته و طبیعی — شیرینی عسلی، زردآلو و بدنه کرمی با اسیدیته متعادل.</p>",
				Sku = "GA-CRH-250", HasVariants = true,
				CategoryId = category?.Id, BrandId = brand?.Id,
				CoffeeOriginId = FindByName(origins, "کاستاریکا")?.Id, CoffeeProcessId = FindByName(processes, "هانی")?.Id, RoastProfileId = FindByName(roasts, "روشن-متوسط")?.Id,
				IsActive = true, IsPublished = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-10),
				MetaTitle = "خرید قهوه کاستاریکا هانی | قهوه علیزاده",
				MetaDescription = "قهوه کاستاریکا با پروسس هانی و شیرینی عسلی.",
				Weight = 0.25m, ViewCount = 290, SoldCount = 61, AverageRating = 4.6m, ReviewCount = 15,
			},
		};

		// واریانت‌ها
		foreach (var p in products.Where(x => x.HasVariants))
		{
			p.Variants =
			[
				new() { Name = "۲۵۰ گرم", Price = p.Sku!.Contains("GEI") ? 1_950_000m : p.Sku.Contains("ESP") ? 690_000m : 420_000m, Weight = 0.25m, IsDefault = true, DisplayOrder = 1, IsActive = true },
				new() { Name = "۵۰۰ گرم", Price = p.Sku.Contains("GEI") ? 3_700_000m : p.Sku.Contains("ESP") ? 1_290_000m : 790_000m, Weight = 0.5m, DisplayOrder = 2, IsActive = true },
				new() { Name = "۱ کیلوگرم", Price = p.Sku.Contains("ESP") ? 2_390_000m : 1_490_000m, Weight = 1m, DisplayOrder = 3, IsActive = true },
			];
		}

		// تصاویر اختصاصی هر محصول (Unsplash CDN)
		var imageMap = new Dictionary<string, (string Main, string Alt)>
		{
			["ethiopia-yirgacheffe"] = (
				"https://images.unsplash.com/photo-1611854779393-1b2da9d400fe?auto=format&fit=crop&w=900&q=75",
				"https://images.unsplash.com/photo-1610632380989-680f404325ff?auto=format&fit=crop&w=900&q=75"),
			["colombia-huila"] = (
				"https://images.unsplash.com/photo-1610632380989-680f404325ff?auto=format&fit=crop&w=900&q=75",
				"https://images.unsplash.com/photo-1580933073521-dc49ac0d4e6a?auto=format&fit=crop&w=900&q=75"),
			["alizade-espresso-blend"] = (
				"https://images.unsplash.com/photo-1510707577719-ae7c14805e3a?auto=format&fit=crop&w=900&q=75",
				"https://images.unsplash.com/photo-1585494156145-1c70a4edc1d0?auto=format&fit=crop&w=900&q=75"),
			["kenya-aa"] = (
				"https://images.unsplash.com/photo-1521302200778-33500795e126?auto=format&fit=crop&w=900&q=75",
				"https://images.unsplash.com/photo-1498804103079-e6fe530c5842?auto=format&fit=crop&w=900&q=75"),
			["brazil-santos"] = (
				"https://images.unsplash.com/photo-1524350876685-274059332603?auto=format&fit=crop&w=900&q=75",
				"https://images.unsplash.com/photo-1511920170033-f8396924c348?auto=format&fit=crop&w=900&q=75"),
			["guatemala-antigua"] = (
				"https://images.unsplash.com/photo-1447933601403-0c6688de566e?auto=format&fit=crop&w=900&q=75",
				"https://images.unsplash.com/photo-1512568400610-62da28bc8a13?auto=format&fit=crop&w=900&q=75"),
			["panama-geisha"] = (
				"https://images.unsplash.com/photo-1507133750040-4a8f57021571?auto=format&fit=crop&w=900&q=75",
				"https://images.unsplash.com/photo-1461023058943-07fcbe16d735?auto=format&fit=crop&w=900&q=75"),
			["costa-rica-honey"] = (
				"https://images.unsplash.com/photo-1514432324607-a61d498e228e?auto=format&fit=crop&w=900&q=75",
				"https://images.unsplash.com/photo-1442512595331-e89e73853f31?auto=format&fit=crop&w=900&q=75"),
		};

		foreach (var p in products)
		{
			var (main, alt) = imageMap.TryGetValue(p.Slug, out var imgs)
				? imgs
				: ("https://images.unsplash.com/photo-1559056199-641a0ac8b55e?auto=format&fit=crop&w=900&q=75",
				   "https://images.unsplash.com/photo-1447933601403-0c6688de566e?auto=format&fit=crop&w=900&q=75");

			p.Images =
			[
				new() { ImageUrl = main, AltText = p.Name, IsPrimary = true, DisplayOrder = 1 },
				new() { ImageUrl = alt, AltText = $"{p.Name} - نمای دوم", DisplayOrder = 2 },
			];

			// نکته‌های طعمی
			if (FindFlavorNote(flavorNotes, "jasmine") is { } n1) p.FlavorNotes.Add(n1);
			if (FindFlavorNote(flavorNotes, "blueberry") is { } n2) p.FlavorNotes.Add(n2);
			if (p.Slug.Contains("colombia") && FindFlavorNote(flavorNotes, "caramel") is { } n3) p.FlavorNotes.Add(n3);
			if (p.Slug.Contains("espresso") && FindFlavorNote(flavorNotes, "dark-chocolate") is { } n4) p.FlavorNotes.Add(n4);
			if (p.Slug.Contains("kenya") && FindFlavorNote(flavorNotes, "cherry") is { } n5) p.FlavorNotes.Add(n5);
			if (p.Slug.Contains("brazil") && FindFlavorNote(flavorNotes, "milk-chocolate") is { } n6) p.FlavorNotes.Add(n6);
			if (p.Slug.Contains("geisha") && FindFlavorNote(flavorNotes, "rose") is { } n7) p.FlavorNotes.Add(n7);
			if (p.Slug.Contains("honey") && FindFlavorNote(flavorNotes, "honey") is { } n8) p.FlavorNotes.Add(n8);
		}

		await _context.Products.AddRangeAsync(products, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		_logger.LogInformation("Seeded {Count} demo products.", products.Count);
	}

	private static T? FindByName<T>(IEnumerable<T> items, string keyword) where T : class =>
		items.FirstOrDefault(i => i.GetType().GetProperty("Name")?.GetValue(i) is string name
			&& name.Contains(keyword, StringComparison.OrdinalIgnoreCase));

	private static FlavorNote? FindFlavorNote(IEnumerable<FlavorNote> notes, string slug) =>
		notes.FirstOrDefault(n => n.Slug == slug);
}