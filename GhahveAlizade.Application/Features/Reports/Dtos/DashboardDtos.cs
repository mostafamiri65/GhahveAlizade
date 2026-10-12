namespace GhahveAlizade.Application.Features.Reports.Dtos;

public class DashboardStatsDto
{
	// امروز
	public int TodayOrders { get; set; }
	public decimal TodayRevenue { get; set; }

	// دیروز
	public int YesterdayOrders { get; set; }
	public decimal YesterdayRevenue { get; set; }

	// این ماه
	public int MonthOrders { get; set; }
	public decimal MonthRevenue { get; set; }

	// کل
	public int TotalOrders { get; set; }
	public decimal TotalRevenue { get; set; }
	public int TotalCustomers { get; set; }
	public int TotalProducts { get; set; }
	public int ActiveProducts { get; set; }

	// مقایسه‌ها
	public decimal RevenueGrowthPercent { get; set; }
	public decimal OrdersGrowthPercent { get; set; }
}

public class SalesChartPointDto
{
	public string Date { get; set; } = string.Empty;       // yyyy/MM/dd
	public int Orders { get; set; }
	public decimal Revenue { get; set; }
}

public class TopProductDto
{
	public long ProductId { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public string? ImageUrl { get; set; }
	public int SoldCount { get; set; }
	public decimal Revenue { get; set; }
}

public class TopCustomerDto
{
	public long CustomerId { get; set; }
	public string? FullName { get; set; }
	public string? PhoneNumber { get; set; }
	public int OrderCount { get; set; }
	public decimal TotalSpent { get; set; }
}

public class RecentOrderDto
{
	public long Id { get; set; }
	public string OrderNumber { get; set; } = string.Empty;
	public string? CustomerName { get; set; }
	public decimal Total { get; set; }
	public string StatusName { get; set; } = string.Empty;
	public string StatusClass { get; set; } = string.Empty;
	public DateTime CreatedAtUtc { get; set; }
}

public class LowStockAlertDto
{
	public long InventoryItemId { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public string? VariantName { get; set; }
	public string WarehouseName { get; set; } = string.Empty;
	public decimal AvailableQuantity { get; set; }
	public decimal ReorderLevel { get; set; }
}

public class SalesReportDto
{
	public decimal TotalRevenue { get; set; }
	public decimal TotalDiscount { get; set; }
	public decimal TotalShipping { get; set; }
	public decimal TotalTax { get; set; }
	public int TotalOrders { get; set; }
	public int TotalItems { get; set; }
	public decimal AverageOrderValue { get; set; }

	public List<SalesChartPointDto> Chart { get; set; } = new();
	public List<TopProductDto> TopProducts { get; set; } = new();
	public List<TopCustomerDto> TopCustomers { get; set; } = new();
	public Dictionary<string, int> OrdersByStatus { get; set; } = new();
	public Dictionary<string, decimal> RevenueByCategory { get; set; } = new();
}

public class InventoryReportDto
{
	public int TotalItems { get; set; }
	public decimal TotalValue { get; set; }
	public int LowStockCount { get; set; }
	public int OutOfStockCount { get; set; }
	public List<LowStockAlertDto> LowStockItems { get; set; } = new();
	public Dictionary<string, decimal> StockByWarehouse { get; set; } = new();
}