using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Reports.Dtos;
using GhahveAlizade.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetTopProducts;

public class GetTopProductsQueryHandler
	: IRequestHandler<GetTopProductsQuery, ErrorOr<List<TopProductDto>>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public GetTopProductsQueryHandler(IAppDbContext context, IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<List<TopProductDto>>> Handle(
		GetTopProductsQuery request, CancellationToken ct)
	{
		var startDate = _dateTime.UtcNow.Date.AddDays(-request.Days);

		var paidStatuses = new[]
		{
			OrderStatus.Paid, OrderStatus.Processing, OrderStatus.ReadyToShip,
			OrderStatus.Shipped, OrderStatus.Delivered
		};

		var result = await _context.OrderItems
			.Where(oi => !oi.IsDeleted &&
						oi.Order.CreatedAtUtc >= startDate &&
						!oi.Order.IsDeleted &&
						paidStatuses.Contains(oi.Order.Status))
			.GroupBy(oi => new { oi.ProductId, oi.ProductName })
			.Select(g => new TopProductDto
			{
				ProductId = g.Key.ProductId,
				ProductName = g.Key.ProductName,
				SoldCount = g.Sum(x => x.Quantity),
				Revenue = g.Sum(x => x.TotalPrice)
			})
			.OrderByDescending(x => x.SoldCount)
			.Take(request.Count)
			.ToListAsync(ct);

		// گرفتن تصویر
		var productIds = result.Select(r => r.ProductId).ToList();
		var images = await _context.ProductImages
			.Where(i => productIds.Contains(i.ProductId) && !i.IsDeleted && i.IsPrimary)
			.Select(i => new { i.ProductId, i.ImageUrl })
			.ToDictionaryAsync(i => i.ProductId, i => i.ImageUrl, ct);

		foreach (var p in result)
		{
			if (images.TryGetValue(p.ProductId, out var url))
				p.ImageUrl = url;
		}

		return result;
	}
}