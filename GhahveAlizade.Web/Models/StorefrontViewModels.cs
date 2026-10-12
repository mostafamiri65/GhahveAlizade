namespace GhahveAlizade.Web.Models;

/// <summary>فیلتر صفحه فروشگاه.</summary>
public class ProductFilterViewModel
{
	public int PageNumber { get; set; } = 1;
	public int? PageSize { get; set; } = 12;
	public string? Search { get; set; }
	public long? CategoryId { get; set; }
	public long? BrandId { get; set; }
	public decimal? MinPrice { get; set; }
	public decimal? MaxPrice { get; set; }
	public string? SortBy { get; set; } = "createdAt";
	public string? SortDir { get; set; } = "desc";
}

/// <summary>گزینه دراپ‌داون فیلتر.</summary>
public record CategoryOption(long Id, string Name);

/// <summary>مدل صفحه لیست محصولات.</summary>
public class ProductListViewModel
{
	public ProductFilterViewModel Filter { get; set; } = new();
	public GhahveAlizade.Application.Common.Models.PaginatedList<
		GhahveAlizade.Application.Features.Catalog.Products.Dtos.ProductListItemDto> Products { get; set; } = null!;
}

/// <summary>فیلتر صفحه وبلاگ.</summary>
public class BlogFilterViewModel
{
	public int PageNumber { get; set; } = 1;
	public int? PageSize { get; set; } = 9;
	public string? Search { get; set; }
	public long? CategoryId { get; set; }
}

/// <summary>مدل صفحه لیست وبلاگ.</summary>
public class BlogListViewModel
{
	public BlogFilterViewModel Filter { get; set; } = new();
	public GhahveAlizade.Application.Common.Models.PaginatedList<
		GhahveAlizade.Application.Features.Content.Dtos.BlogPostListItemDto> Posts { get; set; } = null!;
	public List<CategoryOption> Categories { get; set; } = new();
}