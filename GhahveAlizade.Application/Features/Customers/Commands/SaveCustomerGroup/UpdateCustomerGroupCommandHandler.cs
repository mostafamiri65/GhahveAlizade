using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Customers.Commands.SaveCustomerGroup;

public class UpdateCustomerGroupCommandHandler
	: IRequestHandler<UpdateCustomerGroupCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public UpdateCustomerGroupCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateCustomerGroupCommand request, CancellationToken ct)
	{
		var group = await _context.CustomerGroups
			.FirstOrDefaultAsync(g => g.Id == request.Id && !g.IsDeleted, ct);

		if (group is null) return Error.NotFound("CustomerGroup.NotFound", "گروه یافت نشد.");

		var name = request.Name.Trim();
		if (await _context.CustomerGroups.AnyAsync(
			g => g.Name == name && g.Id != group.Id && !g.IsDeleted, ct))
			return Error.Conflict("CustomerGroup.NameExists", "این نام گروه قبلاً استفاده شده است.");

		group.Name = name;
		group.Description = request.Description?.Trim();
		group.DiscountPercent = request.DiscountPercent;
		group.PriceListId = request.PriceListId;
		group.IsActive = request.IsActive;
		group.DisplayOrder = request.DisplayOrder;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
