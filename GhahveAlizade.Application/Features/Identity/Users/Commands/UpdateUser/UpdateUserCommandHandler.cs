using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Identity.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler
	: IRequestHandler<UpdateUserCommand, ErrorOr<Success>>
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IDateTimeProvider _dateTime;

	public UpdateUserCommandHandler(
		UserManager<ApplicationUser> userManager,
		IDateTimeProvider dateTime)
	{
		_userManager = userManager;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<Success>> Handle(
		UpdateUserCommand request, CancellationToken ct)
	{
		var user = await _userManager.FindByIdAsync(request.Id.ToString());
		if (user is null || user.IsDeleted)
			return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		// چک یکتا بودن ایمیل
		if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
		{
			var existing = await _userManager.FindByEmailAsync(request.Email);
			if (existing is not null && existing.Id != user.Id)
				return Error.Conflict("User.EmailExists", "این ایمیل قبلاً استفاده شده است.");
		}

		// چک یکتا بودن موبایل
		if (!string.IsNullOrWhiteSpace(request.PhoneNumber) && request.PhoneNumber != user.PhoneNumber)
		{
			var existing = _userManager.Users
				.FirstOrDefault(u => u.PhoneNumber == request.PhoneNumber && u.Id != user.Id && !u.IsDeleted);
			if (existing is not null)
				return Error.Conflict("User.PhoneExists", "این شماره موبایل قبلاً استفاده شده است.");
		}

		user.FirstName = request.FirstName?.Trim();
		user.LastName = request.LastName?.Trim();
		user.Email = request.Email?.Trim();
		user.PhoneNumber = request.PhoneNumber?.Trim();
		user.NationalCode = request.NationalCode?.Trim();
		user.BirthDate = request.BirthDate;
		user.Gender = request.Gender?.Trim();
		user.IsActive = request.IsActive;
		user.UpdatedAtUtc = _dateTime.UtcNow;

		var result = await _userManager.UpdateAsync(user);
		if (!result.Succeeded)
		{
			var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();
			return errors;
		}

		return Result.Success;
	}
}