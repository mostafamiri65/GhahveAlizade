using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Common.Models;

public class PaginatedList<T>
{
	public List<T> Items { get; init; } = [];
	public int PageNumber { get; init; }
	public int PageSize { get; init; }
	public int TotalCount { get; init; }
	public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
	public bool HasPrevious => PageNumber > 1;
	public bool HasNext => PageNumber < TotalPages;

	public PaginatedList() { }

	public PaginatedList(List<T> items, int totalCount, int pageNumber, int pageSize)
	{
		Items = items;
		TotalCount = totalCount;
		PageNumber = pageNumber;
		PageSize = pageSize;
	}

	public static async Task<PaginatedList<T>> CreateAsync(
		IQueryable<T> source,
		int pageNumber,
		int pageSize,
		CancellationToken cancellationToken = default)
	{
		var count = await source.CountAsync(cancellationToken);
		var items = await source
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return new PaginatedList<T>(items, count, pageNumber, pageSize);
	}
}