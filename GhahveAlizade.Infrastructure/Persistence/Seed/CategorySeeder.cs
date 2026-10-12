using GhahveAlizade.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Persistence.Seed;

public class CategorySeeder
{
	private readonly AppDbContext _context;
	private readonly ILogger<CategorySeeder> _logger;

	public CategorySeeder(AppDbContext context, ILogger<CategorySeeder> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		if (await _context.Categories.AnyAsync(cancellationToken)) return;

		// والدها
		var coffee = new Category
		{
			Name = "قهوه",
			Slug = "coffee",
			Description = "انواع قهوه تازه رست شده",
			IsActive = true,
			IsFeatured = true,
			DisplayOrder = 1,
			MetaTitle = "خرید قهوه تازه رست شده | قهوه علیزاده",
			MetaDescription = "خرید آنلاین قهوه تازه رست شده، دانه قهوه اسپرسو، قهوه ترک و فرانسه با ارسال سریع"
		};

		var equipment = new Category
		{
			Name = "تجهیزات",
			Slug = "equipment",
			Description = "تجهیزات و لوازم دم‌آوری قهوه",
			IsActive = true,
			DisplayOrder = 2
		};

		var accessories = new Category
		{
			Name = "لوازم جانبی",
			Slug = "accessories",
			Description = "لوازم جانبی قهوه",
			IsActive = true,
			DisplayOrder = 3
		};

		await _context.Categories.AddRangeAsync(new[] { coffee, equipment, accessories }, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		// زیردسته‌های قهوه
		var coffeeSubs = new List<Category>
		{
			new() { Name = "دانه قهوه", Slug = "coffee-beans", ParentId = coffee.Id, DisplayOrder = 1, IsActive = true },
			new() { Name = "قهوه آسیاب شده", Slug = "ground-coffee", ParentId = coffee.Id, DisplayOrder = 2, IsActive = true },
			new() { Name = "کپسول قهوه", Slug = "coffee-capsules", ParentId = coffee.Id, DisplayOrder = 3, IsActive = true },
			new() { Name = "قهوه ترک", Slug = "turkish-coffee", ParentId = coffee.Id, DisplayOrder = 4, IsActive = true },
			new() { Name = "قهوه فرانسه", Slug = "french-coffee", ParentId = coffee.Id, DisplayOrder = 5, IsActive = true },
			new() { Name = "قهوه اسپرسو", Slug = "espresso-coffee", ParentId = coffee.Id, DisplayOrder = 6, IsActive = true },
			new() { Name = "قهوه سرد", Slug = "cold-brew", ParentId = coffee.Id, DisplayOrder = 7, IsActive = true }
		};

		// زیردسته‌های تجهیزات
		var equipmentSubs = new List<Category>
		{
			new() { Name = "دستگاه اسپرسو", Slug = "espresso-machines", ParentId = equipment.Id, DisplayOrder = 1, IsActive = true },
			new() { Name = "آسیاب قهوه", Slug = "coffee-grinders", ParentId = equipment.Id, DisplayOrder = 2, IsActive = true },
			new() { Name = "موکاپات", Slug = "moka-pots", ParentId = equipment.Id, DisplayOrder = 3, IsActive = true },
			new() { Name = "فرنچ پرس", Slug = "french-press", ParentId = equipment.Id, DisplayOrder = 4, IsActive = true },
			new() { Name = "V60 و کموکس", Slug = "pour-over", ParentId = equipment.Id, DisplayOrder = 5, IsActive = true },
			new() { Name = "ایروپرس", Slug = "aeropress", ParentId = equipment.Id, DisplayOrder = 6, IsActive = true }
		};

		await _context.Categories.AddRangeAsync(coffeeSubs, cancellationToken);
		await _context.Categories.AddRangeAsync(equipmentSubs, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		_logger.LogInformation("Seeded {Count} categories.", 3 + coffeeSubs.Count + equipmentSubs.Count);
	}
}