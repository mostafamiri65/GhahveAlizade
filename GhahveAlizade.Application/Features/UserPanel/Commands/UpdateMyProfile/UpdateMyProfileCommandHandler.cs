using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Customer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.UserPanel.Commands.UpdateMyProfile;

public class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommand, ErrorOr<Updated>>
{
	private readonly IAppDbContext _context;

	public UpdateMyProfileCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Updated>> Handle(UpdateMyProfileCommand request, CancellationToken ct)
	{
		var userIdStr = request.UserId;

		var customer = await _context.Customers
			.Where(c => c.UserId == userIdStr && !c.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (customer is null)
		{
			// اولین بار: رکورد Customer ساخته می‌شود
			customer = new Customer { UserId = userIdStr };
			_context.Customers.Add(customer);
		}

		if (!string.IsNullOrWhiteSpace(request.Email))
		{
			var emailTaken = await _context.Customers.AnyAsync(c =>
				c.Id != customer.Id && !c.IsDeleted && c.Email == request.Email, ct);
			if (emailTaken)
				return Error.Conflict("Profile.EmailTaken", "این ایمیل قبلاً استفاده شده است.");
		}

		customer.FirstName = request.FirstName?.Trim();
		customer.LastName = request.LastName?.Trim();
		customer.Email = request.Email?.Trim();
		customer.NationalCode = request.NationalCode?.Trim();
		customer.BirthDate = request.BirthDate;
		customer.Gender = request.Gender;
		customer.AcceptsMarketing = request.AcceptsMarketing;
		customer.UpdatedAtUtc = DateTime.UtcNow;
		customer.UpdatedBy = request.UserId;

		await _context.SaveChangesAsync(ct);
		return Result.Updated;
	}
}
