namespace GhahveAlizade.Application.Common.Interfaces;

public interface ISmsService
{
	Task<SmsResult> SendOtpAsync(string phoneNumber, string code, CancellationToken cancellationToken = default);
	Task<SmsResult> SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
}

public record SmsResult(bool Success, string? MessageId = null, string? ErrorMessage = null);