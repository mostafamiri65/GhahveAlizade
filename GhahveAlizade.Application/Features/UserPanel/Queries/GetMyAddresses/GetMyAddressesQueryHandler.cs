using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.UserPanel.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyAddresses;

public class GetMyAddressesQueryHandler
	: IRequestHandler<GetMyAddressesQuery, ErrorOr<List<MyAddressDto>>>
{
	private readonly IAppDbContext _context;

	public GetMyAddressesQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<MyAddressDto>>> Handle(GetMyAddressesQuery request, CancellationToken ct)
	{
		var userIdStr = request.UserId.ToString();

		var addresses = await _context.CustomerAddresses
			.AsNoTracking()
			.Where(a => !a.IsDeleted && a.Customer != null && a.Customer.UserId == userIdStr)
			.OrderBy(a => a.IsDefault == false)
			.ThenByDescending(a => a.CreatedAtUtc)
			.ProjectToType<MyAddressDto>()
			.ToListAsync(ct);

		return addresses;
	}
}
