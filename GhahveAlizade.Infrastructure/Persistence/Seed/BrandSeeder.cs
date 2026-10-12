using GhahveAlizade.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Persistence.Seed;

/// <summary>برندهای دمو فروشگاه. Idempotent.</summary>
public class BrandSeeder
{
	private readonly AppDbContext _context;
	private readonly ILogger<BrandSeeder> _logger;

	public BrandSeeder(AppDbContext context, ILogger<BrandSeeder> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		if (await _context.Brands.IgnoreQueryFilters().AnyAsync(cancellationToken)) return;

		var brands = new List<Brand>
		{
			new() { Name = "قهوه علیزاده", Slug = "ghahve-alizade", Description = "برند اختصاصی رستری قهوه علیزاده", CountryOfOrigin = "ایران", IsActive = true, IsFeatured = true, DisplayOrder = 1 },
		};

		await _context.Brands.AddRangeAsync(brands, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);
		_logger.LogInformation("Seeded {Count} demo brands.", brands.Count);
	}
}