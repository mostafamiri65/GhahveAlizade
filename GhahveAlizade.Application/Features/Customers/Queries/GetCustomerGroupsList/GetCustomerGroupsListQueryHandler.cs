using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Customers.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Customers.Queries.GetCustomerGroupsList;

public class GetCustomerGroupsListQueryHandler
	: IRequestHandler<GetCustomerGroupsListQuery, ErrorOr<List<CustomerGroupDto>>>
{
	private readonly IAppDbContext _context;

	public GetCustomerGroupsListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<CustomerGroupDto>>> Handle(
		GetCustomerGroupsListQuery request, CancellationToken ct)
	{
		var groups = await _context.CustomerGroups
			.AsNoTracking()
			.Include(g => g.PriceList)
			.Where(g => !g.IsDeleted)
			.OrderBy(g => g.DisplayOrder).ThenBy(g => g.Name)
			.ProjectToType<CustomerGroupDto>()
			.ToListAsync(ct);

		return groups;
	}
}