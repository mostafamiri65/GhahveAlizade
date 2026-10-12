using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace GhahveAlizade.Infrastructure.Services.Sms;

public class KavenegarSettings
{
	public string ApiKey { get; set; } = default!;
	public string Sender { get; set; } = default!;
	public string OtpTemplate { get; set; } = "ghahve-otp";
	public bool Enabled { get; set; } = true;
}

public class KavenegarSmsService : ISmsService
{
	private readonly HttpClient _httpClient;
	private readonly KavenegarSettings _settings;
	private readonly ILogger<KavenegarSmsService> _logger;

	public KavenegarSmsService(
		HttpClient httpClient,
		IOptions<KavenegarSettings> settings,
		ILogger<KavenegarSmsService> logger)
	{
		_httpClient = httpClient;
		_settings = settings.Value;
		_logger = logger;
	}

	public async Task<SmsResult> SendOtpAsync(string phoneNumber, string code, CancellationToken cancellationToken = default)
	{
		if (!_settings.Enabled)
		{
			_logger.LogWarning("SMS disabled. OTP for {Phone}: {Code}", phoneNumber, code);
			return new SmsResult(true, "DEV-MODE");
		}

		try
		{
			// Kavenegar Lookup API
			var url = $"https://api.kavenegar.com/v1/{_settings.ApiKey}/verify/lookup.json" +
					  $"?receptor={phoneNumber}&token={code}&template={_settings.OtpTemplate}";

			var response = await _httpClient.GetAsync(url, cancellationToken);
			var content = await response.Content.ReadAsStringAsync(cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Kavenegar error: {Content}", content);
				return new SmsResult(false, ErrorMessage: "خطا در ارسال پیامک");
			}

			return new SmsResult(true, MessageId: content);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error sending OTP SMS to {Phone}", phoneNumber);
			return new SmsResult(false, ErrorMessage: "خطای شبکه در ارسال پیامک");
		}
	}

	public async Task<SmsResult> SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
	{
		if (!_settings.Enabled)
		{
			_logger.LogWarning("SMS disabled. Message for {Phone}: {Message}", phoneNumber, message);
			return new SmsResult(true, "DEV-MODE");
		}

		try
		{
			var url = $"https://api.kavenegar.com/v1/{_settings.ApiKey}/sms/send.json" +
					  $"?receptor={phoneNumber}&sender={_settings.Sender}&message={Uri.EscapeDataString(message)}";

			var response = await _httpClient.GetAsync(url, cancellationToken);
			var content = await response.Content.ReadAsStringAsync(cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Kavenegar error: {Content}", content);
				return new SmsResult(false, ErrorMessage: "خطا در ارسال پیامک");
			}

			return new SmsResult(true, MessageId: content);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error sending SMS to {Phone}", phoneNumber);
			return new SmsResult(false, ErrorMessage: "خطای شبکه در ارسال پیامک");
		}
	}
}