namespace GhahveAlizade.Application.Common.Interfaces;

public interface IOtpService
{
	Task<string> GenerateAsync(
		string phoneNumber,
		string purpose,
		string? ipAddress = null,
		string? userAgent = null,
		CancellationToken cancellationToken = default);

	Task<OtpVerifyResult> VerifyAsync(
		string phoneNumber,
		string code,
		string purpose,
		CancellationToken cancellationToken = default);

	Task InvalidateAsync(string phoneNumber, string purpose, CancellationToken cancellationToken = default);
}

public record OtpVerifyResult(bool Success, string? ErrorMessage = null);