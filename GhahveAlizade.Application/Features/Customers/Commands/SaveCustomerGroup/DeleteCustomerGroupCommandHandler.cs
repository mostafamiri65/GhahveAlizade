using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Customers.Commands.SaveCustomerGroup;

public class DeleteCustomerGroupCommandHandler
	: IRequestHandler<DeleteCustomerGroupCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteCustomerGroupCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteCustomerGroupCommand request, CancellationToken ct)
	{
		var group = await _context.CustomerGroups
			.FirstOrDefaultAsync(g => g.Id == request.Id && !g.IsDeleted, ct);

		if (group is null) return Error.NotFound("CustomerGroup.NotFound", "گروه یافت نشد.");

		var hasCustomers = await _context.Customers
			.AnyAsync(c => c.CustomerGroupId == group.Id && !c.IsDeleted, ct);

		if (hasCustomers)
			return Error.Conflict("CustomerGroup.HasCustomers", "این گروه دارای مشتری است.");

		_context.CustomerGroups.Remove(group);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}