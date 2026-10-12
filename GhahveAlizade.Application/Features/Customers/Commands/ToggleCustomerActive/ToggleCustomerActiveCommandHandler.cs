using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Customers.Commands.ToggleCustomerActive;

public class ToggleCustomerActiveCommandHandler
	: IRequestHandler<ToggleCustomerActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;

	public ToggleCustomerActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleCustomerActiveCommand request, CancellationToken ct)
	{
		var customer = await _context.Customers
			.FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, ct);

		if (customer is null)
			return Error.NotFound("Customer.NotFound", "مشتری یافت نشد.");

		customer.IsActive = !customer.IsActive;
		await _context.SaveChangesAsync(ct);
		return customer.IsActive;
	}
}