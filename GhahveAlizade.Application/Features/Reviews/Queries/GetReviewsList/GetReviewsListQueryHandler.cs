using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Reviews.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reviews.Queries.GetReviewsList;

public class GetReviewsListQueryHandler
	: IRequestHandler<GetReviewsListQuery, ErrorOr<PaginatedList<ReviewListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetReviewsListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<ReviewListItemDto>>> Handle(
		GetReviewsListQuery request, CancellationToken ct)
	{
		var query = _context.ProductReviews
			.AsNoTracking()
			.Include(r => r.Product)
			.Include(r => r.Customer)
			.Where(r => !r.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(r =>
				(r.AuthorName != null && r.AuthorName.Contains(s)) ||
				(r.Title != null && r.Title.Contains(s)) ||
				(r.Content != null && r.Content.Contains(s)));
		}

		if (request.ProductId.HasValue)
			query = query.Where(r => r.ProductId == request.ProductId.Value);

		if (request.Rating.HasValue)
			query = query.Where(r => r.Rating == request.Rating.Value);

		if (request.IsApproved.HasValue)
			query = query.Where(r => r.IsApproved == request.IsApproved.Value);

		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("rating", "asc") => query.OrderBy(r => r.Rating),
			("rating", _) => query.OrderByDescending(r => r.Rating),
			("helpful", "asc") => query.OrderBy(r => r.HelpfulCount),
			("helpful", _) => query.OrderByDescending(r => r.HelpfulCount),
			("createdat", "asc") => query.OrderBy(r => r.CreatedAtUtc),
			_ => query.OrderByDescending(r => r.CreatedAtUtc)
		};

		return await PaginatedList<ReviewListItemDto>.CreateAsync(
			query.ProjectToType<ReviewListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}