using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Customer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.UserPanel.Commands.SaveMyAddress;

public class SaveMyAddressCommandHandler : IRequestHandler<SaveMyAddressCommand, ErrorOr<Created>>
{
	private readonly IAppDbContext _context;

	public SaveMyAddressCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Created>> Handle(SaveMyAddressCommand request, CancellationToken ct)
	{
		var userIdStr = request.UserId;

		if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.AddressLine1))
			return Error.Validation("Address.Invalid", "عنوان و آدرس الزامی است.");

		var customer = await _context.Customers
			.Where(c => c.UserId == userIdStr && !c.IsDeleted)
			.FirstOrDefaultAsync(ct)
			?? new Customer { UserId = userIdStr };

		if (customer.Id == 0) _context.Customers.Add(customer);

		CustomerAddress address;

		if (request.AddressId.HasValue)
		{
			address = await _context.CustomerAddresses
				.Where(a => a.Id == request.AddressId.Value && !a.IsDeleted
					&& a.Customer != null && a.Customer.UserId == userIdStr) // بررسی مالکیت
				.FirstAsync(ct);

			if (address is null)
				return Error.NotFound("Address.NotFound", "آدرس یافت نشد.");
		}
		else
		{
			address = new CustomerAddress { CustomerId = customer.Id };
			_context.CustomerAddresses.Add(address);
		}

		address.Title = request.Title.Trim();
		address.FirstName = request.FirstName.Trim();
		address.LastName = request.LastName.Trim();
		address.PhoneNumber = request.PhoneNumber.Trim();
		address.Province = request.Province.Trim();
		address.City = request.City.Trim();
		address.PostalCode = request.PostalCode.Trim();
		address.AddressLine1 = request.AddressLine1.Trim();
		address.AddressLine2 = request.AddressLine2?.Trim();
		address.IsShipping = true;
		address.UpdatedAtUtc = DateTime.UtcNow;

		if (request.IsDefault)
		{
			var others = await _context.CustomerAddresses
				.Where(a => a.CustomerId == customer.Id && !a.IsDeleted && a.Id != address.Id)
				.ToListAsync(ct);
			foreach (var o in others)
				o.IsDefault = false;
			address.IsDefault = true;
		}

		await _context.SaveChangesAsync(ct);
		return Result.Created;
	}
}
