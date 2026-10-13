using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Customer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Auth.Commands.CompleteProfile;

/// <summary>
/// تکمیل مشخصات کاربر بعد از تأیید OTP (کاربر جدید).
/// هم Customer پروفایل و هم ApplicationUser را به‌روزرسانی می‌کند.
/// </summary>
public record CompleteProfileCommand(
	long UserId,
	string FirstName,
	string LastName,
	string? Email,
	string? NationalCode,
	DateOnly? BirthDate,
	string? Gender) : IRequest<ErrorOr<Success>>;
