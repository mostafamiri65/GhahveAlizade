using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetInventoryTransactions;

public record GetInventoryTransactionsQuery(
	long InventoryItemId,
	int PageNumber = 1,
	int PageSize = 30)
	: IRequest<ErrorOr<PaginatedList<InventoryTransactionDto>>>;
