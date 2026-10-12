using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Newsletter.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Newsletter.Queries.GetNewsletterSubscribersList;

public record GetNewsletterSubscribersListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	bool? IsActive = null,
	bool? IsVerified = null,
	string? SortBy = "createdAt",
	string? SortDir = "desc")
	: IRequest<ErrorOr<PaginatedList<NewsletterSubscriberListItemDto>>>;