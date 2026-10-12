using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Customers.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler
	: IRequestHandler<GetCustomerByIdQuery, ErrorOr<CustomerDto>>
{
	private readonly IAppDbContext _context;

	public GetCustomerByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken ct)
	{
		var customer = await _context.Customers
			.AsNoTracking()
			.Include(c => c.CustomerGroup)
			.Include(c => c.Addresses)
			.Include(c => c.Notes)
			.Where(c => c.Id == request.Id && !c.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (customer is null)
			return Error.NotFound("Customer.NotFound", "مشتری یافت نشد.");

		return customer.Adapt<CustomerDto>();
	}
}