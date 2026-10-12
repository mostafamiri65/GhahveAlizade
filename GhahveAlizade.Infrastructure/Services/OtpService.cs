using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using GhahveAlizade.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace GhahveAlizade.Infrastructure.Services;

public class OtpService : IOtpService
{
	private readonly AppDbContext _context;
	private readonly IDateTimeProvider _dateTime;
	private readonly ILogger<OtpService> _logger;

	private const int OtpLength = 6;
	private const int ExpiryMinutes = 3;
	private const int ResendCooldownSeconds = 60;

	public OtpService(
		AppDbContext context,
		IDateTimeProvider dateTime,
		ILogger<OtpService> logger)
	{
		_context = context;
		_dateTime = dateTime;
		_logger = logger;
	}

	public async Task<string> GenerateAsync(
		string phoneNumber,
		string purpose,
		string? ipAddress = null,
		string? userAgent = null,
		CancellationToken cancellationToken = default)
	{
		// چک کردن cooldown
		var recentOtp = await _context.PhoneOtps
			.Where(o => o.PhoneNumber == phoneNumber
					 && o.Purpose == purpose
					 && o.CreatedAtUtc > _dateTime.UtcNow.AddSeconds(-ResendCooldownSeconds))
			.FirstOrDefaultAsync(cancellationToken);

		if (recentOtp is not null)
		{
			var remaining = ResendCooldownSeconds - (int)(_dateTime.UtcNow - recentOtp.CreatedAtUtc).TotalSeconds;
			throw new InvalidOperationException($"لطفاً {remaining} ثانیه دیگر تلاش کنید.");
		}

		// باطل کردن OTPهای قبلی
		var oldOtps = await _context.PhoneOtps
			.Where(o => o.PhoneNumber == phoneNumber
					 && o.Purpose == purpose
					 && !o.IsUsed)
			.ToListAsync(cancellationToken);

		foreach (var old in oldOtps)
		{
			old.IsUsed = true;
			old.UsedAtUtc = _dateTime.UtcNow;
		}

		// ساخت OTP جدید
		var code = GenerateRandomCode();

		_context.PhoneOtps.Add(new PhoneOtp
		{
			PhoneNumber = phoneNumber,
			Code = code,
			Purpose = purpose,
			ExpiresAtUtc = _dateTime.UtcNow.AddMinutes(ExpiryMinutes),
			IpAddress = ipAddress,
			UserAgent = userAgent,
			MaxAttempts = 5
		});

		await _context.SaveChangesAsync(cancellationToken);

		_logger.LogInformation("OTP generated for {Phone} ({Purpose})", MaskPhone(phoneNumber), purpose);

		return code;
	}

	public async Task<OtpVerifyResult> VerifyAsync(
		string phoneNumber,
		string code,
		string purpose,
		CancellationToken cancellationToken = default)
	{
		var otp = await _context.PhoneOtps
			.Where(o => o.PhoneNumber == phoneNumber
					 && o.Purpose == purpose
					 && !o.IsUsed)
			.OrderByDescending(o => o.CreatedAtUtc)
			.FirstOrDefaultAsync(cancellationToken);

		if (otp is null)
			return new OtpVerifyResult(false, "کد تایید یافت نشد. لطفاً دوباره درخواست کنید.");

		if (otp.ExpiresAtUtc < _dateTime.UtcNow)
			return new OtpVerifyResult(false, "کد تایید منقضی شده است.");

		if (otp.AttemptCount >= otp.MaxAttempts)
			return new OtpVerifyResult(false, "تعداد تلاش‌های شما بیش از حد مجاز است. لطفاً کد جدید درخواست کنید.");

		// افزایش شمارنده تلاش
		otp.AttemptCount++;

		if (otp.Code != code)
		{
			await _context.SaveChangesAsync(cancellationToken);
			return new OtpVerifyResult(false, "کد وارد شده صحیح نیست.");
		}

		// موفق
		otp.IsUsed = true;
		otp.UsedAtUtc = _dateTime.UtcNow;
		await _context.SaveChangesAsync(cancellationToken);

		return new OtpVerifyResult(true);
	}

	public async Task InvalidateAsync(string phoneNumber, string purpose, CancellationToken cancellationToken = default)
	{
		var otps = await _context.PhoneOtps
			.Where(o => o.PhoneNumber == phoneNumber
					 && o.Purpose == purpose
					 && !o.IsUsed)
			.ToListAsync(cancellationToken);

		foreach (var otp in otps)
		{
			otp.IsUsed = true;
			otp.UsedAtUtc = _dateTime.UtcNow;
		}

		await _context.SaveChangesAsync(cancellationToken);
	}

	private static string GenerateRandomCode()
	{
		// استفاده از RandomNumberGenerator برای امنیت بیشتر
		var bytes = new byte[4];
		RandomNumberGenerator.Fill(bytes);
		var number = BitConverter.ToUInt32(bytes, 0) % 1000000;
		return number.ToString("D6");
	}

	private static string MaskPhone(string phone)
		=> phone.Length >= 7
			? $"{phone[..4]}***{phone[^3..]}"
			: "***";
}