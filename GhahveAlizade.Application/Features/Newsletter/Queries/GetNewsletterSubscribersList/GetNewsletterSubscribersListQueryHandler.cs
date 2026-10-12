using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Newsletter.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Newsletter.Queries.GetNewsletterSubscribersList;

public class GetNewsletterSubscribersListQueryHandler
	: IRequestHandler<GetNewsletterSubscribersListQuery, ErrorOr<PaginatedList<NewsletterSubscriberListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetNewsletterSubscribersListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<NewsletterSubscriberListItemDto>>> Handle(
		GetNewsletterSubscribersListQuery request, CancellationToken ct)
	{
		var query = _context.NewsletterSubscribers.AsNoTracking().Where(n => !n.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(n =>
				n.Email.Contains(s) ||
				(n.FirstName != null && n.FirstName.Contains(s)) ||
				(n.LastName != null && n.LastName.Contains(s)) ||
				(n.PhoneNumber != null && n.PhoneNumber.Contains(s)));
		}

		if (request.IsActive.HasValue)
			query = query.Where(n => n.IsActive == request.IsActive.Value);

		if (request.IsVerified.HasValue)
			query = query.Where(n => n.IsVerified == request.IsVerified.Value);

		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("email", "asc") => query.OrderBy(n => n.Email),
			("email", _) => query.OrderByDescending(n => n.Email),
			("createdat", "asc") => query.OrderBy(n => n.CreatedAtUtc),
			_ => query.OrderByDescending(n => n.CreatedAtUtc)
		};

		return await PaginatedList<NewsletterSubscriberListItemDto>.CreateAsync(
			query.ProjectToType<NewsletterSubscriberListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}