using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Auth.Commands.VerifyOtp;

public record VerifyOtpCommand(string PhoneNumber, string Code) : IRequest<ErrorOr<VerifyOtpResponse>>;

public record VerifyOtpResponse(
	long UserId,
	string PhoneNumber,
	string? FullName,
	bool HasPassword,
	bool IsNewUser);