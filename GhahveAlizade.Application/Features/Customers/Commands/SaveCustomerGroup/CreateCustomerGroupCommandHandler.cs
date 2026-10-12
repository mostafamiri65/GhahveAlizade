using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Wholesale;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Customers.Commands.SaveCustomerGroup;

public class CreateCustomerGroupCommandHandler
	: IRequestHandler<CreateCustomerGroupCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	public CreateCustomerGroupCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(CreateCustomerGroupCommand request, CancellationToken ct)
	{
		var name = request.Name.Trim();
		if (await _context.CustomerGroups.AnyAsync(g => g.Name == name && !g.IsDeleted, ct))
			return Error.Conflict("CustomerGroup.NameExists", "این نام گروه قبلاً استفاده شده است.");

		var group = new CustomerGroup
		{
			Name = name,
			Description = request.Description?.Trim(),
			DiscountPercent = request.DiscountPercent,
			PriceListId = request.PriceListId,
			IsActive = request.IsActive,
			DisplayOrder = request.DisplayOrder
		};

		_context.CustomerGroups.Add(group);
		await _context.SaveChangesAsync(ct);
		return group.Id;
	}
}
