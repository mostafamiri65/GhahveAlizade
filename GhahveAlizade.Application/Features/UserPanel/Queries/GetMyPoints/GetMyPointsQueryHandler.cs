using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.UserPanel.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyPoints;

public class GetMyPointsQueryHandler : IRequestHandler<GetMyPointsQuery, ErrorOr<MyPointDto>>
{
	private readonly IAppDbContext _context;

	public GetMyPointsQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<MyPointDto>> Handle(GetMyPointsQuery request, CancellationToken ct)
	{
		var userIdStr = request.UserId;

		var account = await _context.CustomerPointAccounts
			.AsNoTracking()
			.Include(p => p.Customer)
			.Include(p => p.Transactions.Where(t => !t.IsDeleted).OrderByDescending(t => t.CreatedAtUtc).Take(50))
			.Where(p => !p.IsDeleted && p.Customer != null && p.Customer.UserId == userIdStr)
			.FirstOrDefaultAsync(ct);

		if (account is null)
			return new MyPointDto(); // حسابی ندارد: همه صفر

		return new MyPointDto
		{
			CurrentPoints = account.CurrentPoints,
			TotalEarned = account.TotalEarnedPoints,
			TotalRedeemed = account.TotalRedeemedPoints,
			Tier = account.Tier,
			Transactions = account.Transactions.Adapt<List<MyPointTransactionDto>>()
		};
	}
}
