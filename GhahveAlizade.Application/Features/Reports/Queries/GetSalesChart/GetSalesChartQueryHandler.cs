using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Reports.Dtos;
using GhahveAlizade.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetSalesChart;

public class GetSalesChartQueryHandler
	: IRequestHandler<GetSalesChartQuery, ErrorOr<List<SalesChartPointDto>>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public GetSalesChartQueryHandler(IAppDbContext context, IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<List<SalesChartPointDto>>> Handle(
		GetSalesChartQuery request, CancellationToken ct)
	{
		var now = _dateTime.UtcNow;
		var startDate = now.Date.AddDays(-request.Days + 1);

		var paidStatuses = new[]
		{
			OrderStatus.Paid, OrderStatus.Processing, OrderStatus.ReadyToShip,
			OrderStatus.Shipped, OrderStatus.Delivered
		};

		var rawData = await _context.Orders
			.Where(o => !o.IsDeleted &&
					   o.CreatedAtUtc >= startDate &&
					   paidStatuses.Contains(o.Status))
			.GroupBy(o => o.CreatedAtUtc.Date)
			.Select(g => new
			{
				Date = g.Key,
				Orders = g.Count(),
				Revenue = g.Sum(o => o.Total)
			})
			.ToListAsync(ct);

		// پر کردن روزهای خالی
		var result = new List<SalesChartPointDto>();
		for (var i = 0; i < request.Days; i++)
		{
			var date = startDate.AddDays(i);
			var data = rawData.FirstOrDefault(r => r.Date == date);
			result.Add(new SalesChartPointDto
			{
				Date = date.ToString("yyyy/MM/dd"),
				Orders = data?.Orders ?? 0,
				Revenue = data?.Revenue ?? 0
			});
		}

		return result;
	}
}