using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Support.Dtos;
using GhahveAlizade.Domain.Support;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Queries.GetTicketsList;

public record GetTicketsListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	TicketStatus? Status = null,
	TicketPriority? Priority = null)
	: IRequest<ErrorOr<PaginatedList<AdminTicketListItemDto>>>;
