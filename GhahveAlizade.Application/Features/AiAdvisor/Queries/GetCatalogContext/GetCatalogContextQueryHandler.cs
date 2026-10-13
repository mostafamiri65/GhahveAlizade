using System.Text;
using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.AiAdvisor.Queries.GetCatalogContext;

/// <summary>
/// فهرست فشرده محصولات فعال را به‌صورت متن برای LLM تولید می‌کند.
/// هر ۵ دقیقه cache می‌شود تا از کوئری‌های تکراری جلوگیری شود.
/// </summary>
public class GetCatalogContextQueryHandler : IRequestHandler<GetCatalogContextQuery, ErrorOr<string>>
{
	private readonly IAppDbContext _context;
	private readonly ICacheService _cache;

	public GetCatalogContextQueryHandler(IAppDbContext context, ICacheService cache)
	{
		_context = context;
		_cache = cache;
	}

	public async Task<ErrorOr<string>> Handle(GetCatalogContextQuery request, CancellationToken ct)
	{
		var cached = await _cache.GetAsync<string>("ai:catalog-context", ct);
		if (!string.IsNullOrEmpty(cached)) return cached;

		var products = await _context.Products
			.AsNoTracking()
			.Where(p => p.IsActive && p.IsPublished && !p.IsDeleted)
			.Select(p => new
			{
				p.Id,
				p.Name,
				p.Slug,
				p.ShortDescription,
				p.BasePrice,
				Roast = p.RoastProfile != null ? p.RoastProfile.Name : null,
				Origin = p.CoffeeOrigin != null ? p.CoffeeOrigin.Name : null,
				Process = p.CoffeeProcess != null ? p.CoffeeProcess.Name : null,
				Flavors = p.FlavorNotes.Select(f => f.Name).ToList(),
				Rating = p.AverageRating
			})
			.Take(50)
			.ToListAsync(ct);

		var sb = new StringBuilder("محصولات موجود در فروشگاه قهوه علیزاده:\n");
		foreach (var p in products)
		{
			sb.AppendLine(
				$"- {p.Name} | قیمت: {p.BasePrice:N0} تومان | رست: {p.Roast ?? "-"} | " +
				$"خاستگاه: {p.Origin ?? "-"} | فرآیند: {p.Process ?? "-"} | " +
				$"طعم‌ها: {string.Join("، ", p.Flavors)} | امتیاز: {p.Rating:F1} | لینک: /products/{p.Slug}");
		}

		var result = sb.ToString();
		await _cache.SetAsync("ai:catalog-context", result, TimeSpan.FromMinutes(5), ct);
		return result;
	}
}
