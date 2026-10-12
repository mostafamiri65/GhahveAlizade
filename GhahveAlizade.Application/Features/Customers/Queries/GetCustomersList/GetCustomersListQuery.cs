using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Customers.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Customers.Queries.GetCustomersList;

public record GetCustomersListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	long? CustomerGroupId = null,
	bool? IsActive = null,
	bool? IsVerified = null,
	bool? HasOrders = null,
	string? SortBy = "createdAt",
	string? SortDir = "desc")
	: IRequest<ErrorOr<PaginatedList<CustomerListItemDto>>>;