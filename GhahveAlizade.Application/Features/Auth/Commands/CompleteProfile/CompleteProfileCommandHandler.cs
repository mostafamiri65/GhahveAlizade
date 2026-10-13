using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Customer;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Auth.Commands.CompleteProfile;

public class CompleteProfileCommandHandler : IRequestHandler<CompleteProfileCommand, ErrorOr<Success>>
{
	private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;
	private readonly IAppDbContext _context;

	public CompleteProfileCommandHandler(
		UserManager<Domain.Identity.ApplicationUser> userManager,
		IAppDbContext context)
	{
		_userManager = userManager;
		_context = context;
	}

	public async Task<ErrorOr<Success>> Handle(CompleteProfileCommand request, CancellationToken ct)
	{
		var user = await _userManager.Users
			.FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted, ct);

		if (user is null)
			return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
			return Error.Validation("Profile.NameRequired", "نام و نام خانوادگی الزامی است.");

		// چک یکتایی ایمیل (در ApplicationUser و Customer)
		if (!string.IsNullOrWhiteSpace(request.Email))
		{
			var emailTaken = await _userManager.Users
				.AnyAsync(u => u.Id != user.Id && u.Email == request.Email && !u.IsDeleted, ct);
			if (emailTaken)
				return Error.Conflict("Profile.EmailTaken", "این ایمیل قبلاً ثبت شده است.");
		}

		// به‌روزرسانی ApplicationUser
		user.FirstName = request.FirstName.Trim();
		user.LastName = request.LastName.Trim();
		user.Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();
		user.NationalCode = request.NationalCode?.Trim();
		user.BirthDate = request.BirthDate;
		user.Gender = request.Gender;
		user.UpdatedAtUtc = DateTime.UtcNow;
		await _userManager.UpdateAsync(user);

		// به‌روزرسانی/ساخت Customer
		var userIdStr = user.Id;
		var customer = await _context.Customers
			.FirstOrDefaultAsync(c => c.UserId == userIdStr && !c.IsDeleted, ct);

		if (customer is null)
		{
			customer = new Customer
			{
				UserId = userIdStr,
				PhoneNumber = user.PhoneNumber
			};
			_context.Customers.Add(customer);
		}

		customer.FirstName = user.FirstName;
		customer.LastName = user.LastName;
		customer.Email = user.Email;
		customer.NationalCode = user.NationalCode;
		customer.BirthDate = request.BirthDate;
		customer.Gender = request.Gender;
		customer.UpdatedAtUtc = DateTime.UtcNow;
		customer.UpdatedBy = user.Id;

		await _context.SaveChangesAsync(ct);

		return Result.Success;
	}
}
