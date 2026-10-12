using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.UserPanel.Commands.DeleteMyAddress;

public class DeleteMyAddressCommandHandler : IRequestHandler<DeleteMyAddressCommand, ErrorOr<Deleted>>
{
	private readonly IAppDbContext _context;

	public DeleteMyAddressCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Deleted>> Handle(DeleteMyAddressCommand request, CancellationToken ct)
	{
		var userIdStr = request.UserId.ToString();

		var address = await _context.CustomerAddresses
			.Where(a => a.Id == request.AddressId && !a.IsDeleted
				&& a.Customer != null && a.Customer.UserId == userIdStr)
			.FirstOrDefaultAsync(ct);

		if (address is null)
			return Error.NotFound("Address.NotFound", "آدرس یافت نشد.");

		address.IsDeleted = true; // Soft delete — مطابق BaseEntity
		address.DeletedAtUtc = DateTime.UtcNow;
		address.DeletedBy = request.UserId;

		await _context.SaveChangesAsync(ct);
		return Result.Deleted;
	}
}
