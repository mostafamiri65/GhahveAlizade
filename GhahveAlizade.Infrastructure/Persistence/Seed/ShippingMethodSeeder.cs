using GhahveAlizade.Domain.Shipping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Persistence.Seed;

public class ShippingMethodSeeder
{
	private readonly AppDbContext _context;
	private readonly ILogger<ShippingMethodSeeder> _logger;

	public ShippingMethodSeeder(AppDbContext context, ILogger<ShippingMethodSeeder> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		if (await _context.ShippingMethods.AnyAsync(cancellationToken)) return;

		var methods = new List<ShippingMethod>
		{
			new()
			{
				Name = "پست پیشتاز",
				Code = "post-peyshtaz",
				Description = "ارسال از طریق پست پیشتاز، ۳ تا ۵ روز کاری",
				Price = 45000,
				FreeShippingThreshold = 500000,
				EstimatedDeliveryDaysMin = 3,
				EstimatedDeliveryDaysMax = 5,
				IsActive = true,
				DisplayOrder = 1
			},
			new()
			{
				Name = "تیپاکس",
				Code = "tipax",
				Description = "ارسال سریع با تیپاکس، ۱ تا ۳ روز کاری",
				Price = 75000,
				FreeShippingThreshold = 1000000,
				EstimatedDeliveryDaysMin = 1,
				EstimatedDeliveryDaysMax = 3,
				IsActive = true,
				DisplayOrder = 2
			},
			new()
			{
				Name = "پیک تهران",
				Code = "tehran-courier",
				Description = "ارسال با پیک در تهران، همان روز",
				Price = 60000,
				EstimatedDeliveryDaysMin = 0,
				EstimatedDeliveryDaysMax = 1,
				IsActive = true,
				DisplayOrder = 3
			},
			new()
			{
				Name = "تحویل حضوری",
				Code = "in-store-pickup",
				Description = "تحویل حضوری از شعبه",
				Price = 0,
				EstimatedDeliveryDaysMin = 0,
				EstimatedDeliveryDaysMax = 1,
				IsActive = true,
				DisplayOrder = 4
			}
		};

		await _context.ShippingMethods.AddRangeAsync(methods, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		_logger.LogInformation("Seeded {Count} shipping methods.", methods.Count);
	}
}