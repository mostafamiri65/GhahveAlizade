using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Auth.Commands.RequestOtp;

public record RequestOtpCommand(string PhoneNumber) : IRequest<ErrorOr<RequestOtpResponse>>;

public record RequestOtpResponse(string PhoneNumber, int ExpiresInSeconds, bool IsNewUser);