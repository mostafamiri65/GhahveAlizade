using GhahveAlizade.Domain.Catalog;
using GhahveAlizade.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Persistence.Seed;

public class AttributeSeeder
{
	private readonly AppDbContext _context;
	private readonly ILogger<AttributeSeeder> _logger;

	public AttributeSeeder(AppDbContext context, ILogger<AttributeSeeder> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		if (await _context.Attributes.AnyAsync(cancellationToken)) return;

		var attributes = new List<Domain.Catalog.Attribute>
		{
			new()
			{
				Name = "وزن",
				Slug = "weight",
				Type = AttributeType.Select,
				Unit = "گرم",
				IsFilterable = true,
				IsRequired = true,
				IsVariant = true,
				DisplayOrder = 1,
				Values = new List<AttributeValue>
				{
					new() { Value = "100", DisplayValue = "۱۰۰ گرم", DisplayOrder = 1 },
					new() { Value = "250", DisplayValue = "۲۵۰ گرم", DisplayOrder = 2 },
					new() { Value = "500", DisplayValue = "۵۰۰ گرم", DisplayOrder = 3 },
					new() { Value = "1000", DisplayValue = "۱ کیلوگرم", DisplayOrder = 4 }
				}
			},
			new()
			{
				Name = "آسیاب",
				Slug = "grind",
				Type = AttributeType.Select,
				IsFilterable = true,
				IsRequired = false,
				IsVariant = true,
				DisplayOrder = 2,
				Values = new List<AttributeValue>
				{
					new() { Value = "whole-bean", DisplayValue = "دانه کامل", DisplayOrder = 1 },
					new() { Value = "espresso", DisplayValue = "اسپرسو", DisplayOrder = 2 },
					new() { Value = "moka", DisplayValue = "موکاپات", DisplayOrder = 3 },
					new() { Value = "french-press", DisplayValue = "فرنچ پرس", DisplayOrder = 4 },
					new() { Value = "v60", DisplayValue = "V60", DisplayOrder = 5 },
					new() { Value = "turkish", DisplayValue = "ترک", DisplayOrder = 6 }
				}
			},
			new()
			{
				Name = "بسته‌بندی",
				Slug = "packaging",
				Type = AttributeType.Select,
				IsFilterable = true,
				IsVariant = true,
				DisplayOrder = 3,
				Values = new List<AttributeValue>
				{
					new() { Value = "valve-bag", DisplayValue = "پاکت سوپاپ‌دار", DisplayOrder = 1 },
					new() { Value = "tin", DisplayValue = "قوطی", DisplayOrder = 2 },
					new() { Value = "capsule", DisplayValue = "کپسول", DisplayOrder = 3 }
				}
			},
			new()
			{
				Name = "کافئین",
				Slug = "caffeine",
				Type = AttributeType.Bool,
				IsFilterable = true,
				DisplayOrder = 4
			},
			new()
			{
				Name = "ارگانیک",
				Slug = "organic",
				Type = AttributeType.Bool,
				IsFilterable = true,
				DisplayOrder = 5
			},
			new()
			{
				Name = "ارتفاع کشت",
				Slug = "altitude",
				Type = AttributeType.Text,
				Unit = "متر",
				IsFilterable = false,
				DisplayOrder = 6
			},
			new()
			{
				Name = "اسیدیته",
				Slug = "acidity",
				Type = AttributeType.Select,
				IsFilterable = true,
				DisplayOrder = 7,
				Values = new List<AttributeValue>
				{
					new() { Value = "low", DisplayValue = "کم", DisplayOrder = 1 },
					new() { Value = "medium", DisplayValue = "متوسط", DisplayOrder = 2 },
					new() { Value = "high", DisplayValue = "زیاد", DisplayOrder = 3 }
				}
			},
			new()
			{
				Name = "بدنه",
				Slug = "body",
				Type = AttributeType.Select,
				IsFilterable = true,
				DisplayOrder = 8,
				Values = new List<AttributeValue>
				{
					new() { Value = "light", DisplayValue = "سبک", DisplayOrder = 1 },
					new() { Value = "medium", DisplayValue = "متوسط", DisplayOrder = 2 },
					new() { Value = "full", DisplayValue = "سنگین", DisplayOrder = 3 }
				}
			}
		};

		await _context.Attributes.AddRangeAsync(attributes, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		_logger.LogInformation("Seeded {Count} attributes with values.", attributes.Count);
	}
}