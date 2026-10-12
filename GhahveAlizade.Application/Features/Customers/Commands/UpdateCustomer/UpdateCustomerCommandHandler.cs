using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler
	: IRequestHandler<UpdateCustomerCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public UpdateCustomerCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateCustomerCommand request, CancellationToken ct)
	{
		var customer = await _context.Customers
			.FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, ct);

		if (customer is null)
			return Error.NotFound("Customer.NotFound", "مشتری یافت نشد.");

		// چک یکتا بودن موبایل
		if (!string.IsNullOrWhiteSpace(request.PhoneNumber) &&
			request.PhoneNumber != customer.PhoneNumber)
		{
			var exists = await _context.Customers.AnyAsync(c =>
				c.PhoneNumber == request.PhoneNumber &&
				c.Id != customer.Id && !c.IsDeleted, ct);

			if (exists)
				return Error.Conflict("Customer.PhoneExists", "این شماره موبایل قبلاً استفاده شده است.");
		}

		// چک یکتا بودن ایمیل
		if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != customer.Email)
		{
			var exists = await _context.Customers.AnyAsync(c =>
				c.Email == request.Email &&
				c.Id != customer.Id && !c.IsDeleted, ct);

			if (exists)
				return Error.Conflict("Customer.EmailExists", "این ایمیل قبلاً استفاده شده است.");
		}

		customer.FirstName = request.FirstName?.Trim();
		customer.LastName = request.LastName?.Trim();
		customer.Email = request.Email?.Trim();
		customer.PhoneNumber = request.PhoneNumber?.Trim();
		customer.NationalCode = request.NationalCode?.Trim();
		customer.BirthDate = request.BirthDate;
		customer.Gender = request.Gender?.Trim();
		customer.IsActive = request.IsActive;
		customer.IsVerified = request.IsVerified;
		customer.AcceptsMarketing = request.AcceptsMarketing;
		customer.CustomerGroupId = request.CustomerGroupId;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}