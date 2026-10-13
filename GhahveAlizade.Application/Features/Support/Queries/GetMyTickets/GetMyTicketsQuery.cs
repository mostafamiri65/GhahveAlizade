using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Support.Dtos;
using GhahveAlizade.Domain.Support;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Queries.GetMyTickets;

public record GetMyTicketsQuery(
	long UserId,
	int PageNumber = 1,
	int PageSize = 15,
	TicketStatus? Status = null)
	: IRequest<ErrorOr<PaginatedList<TicketListItemDto>>>;
