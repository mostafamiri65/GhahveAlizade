using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetInventoryTransactions;

public class GetInventoryTransactionsQueryHandler
	: IRequestHandler<GetInventoryTransactionsQuery, ErrorOr<PaginatedList<InventoryTransactionDto>>>
{
	private readonly IAppDbContext _context;
	public GetInventoryTransactionsQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<InventoryTransactionDto>>> Handle(
		GetInventoryTransactionsQuery request, CancellationToken ct)
	{
		var query = _context.InventoryTransactions
			.AsNoTracking()
			.Where(t => t.InventoryItemId == request.InventoryItemId && !t.IsDeleted)
			.OrderByDescending(t => t.CreatedAtUtc);

		return await PaginatedList<InventoryTransactionDto>.CreateAsync(
			query.ProjectToType<InventoryTransactionDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}