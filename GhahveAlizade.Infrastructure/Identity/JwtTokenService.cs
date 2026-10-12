using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using GhahveAlizade.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GhahveAlizade.Infrastructure.Identity;

public interface IJwtTokenService
{
	Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(ApplicationUser user);
	Task<ApplicationUser?> ValidateRefreshTokenAsync(string refreshToken);
	Task RevokeRefreshTokenAsync(string refreshToken);
}

public class JwtTokenService : IJwtTokenService
{
	private readonly JwtSettings _settings;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly AppDbContext _context;

	public JwtTokenService(
		IOptions<JwtSettings> settings,
		UserManager<ApplicationUser> userManager,
		AppDbContext context)
	{
		_settings = settings.Value;
		_userManager = userManager;
		_context = context;
	}

	public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(ApplicationUser user)
	{
		var roles = await _userManager.GetRolesAsync(user);

		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
			new(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new(ClaimTypes.Name, user.UserName ?? "")
		};

		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var accessToken = new JwtSecurityToken(
			issuer: _settings.Issuer,
			audience: _settings.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes),
			signingCredentials: credentials);

		var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
		var refreshToken = GenerateRefreshToken();

		// 存储 RefreshToken
		_context.RefreshTokens.Add(new RefreshToken
		{
			UserId = user.Id,
			Token = refreshToken,
			ExpiresAtUtc = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays),
			CreatedAtUtc = DateTime.UtcNow
		});

		await _context.SaveChangesAsync();

		return (accessTokenString, refreshToken);
	}

	public async Task<ApplicationUser?> ValidateRefreshTokenAsync(string refreshToken)
	{
		var stored = await _context.RefreshTokens
			.FirstOrDefaultAsync(r => r.Token == refreshToken
								   && !r.IsRevoked
								   && r.ExpiresAtUtc > DateTime.UtcNow);

		if (stored is null) return null;

		return await _userManager.FindByIdAsync(stored.UserId.ToString());
	}

	public async Task RevokeRefreshTokenAsync(string refreshToken)
	{
		var stored = await _context.RefreshTokens
			.FirstOrDefaultAsync(r => r.Token == refreshToken);

		if (stored is not null)
		{
			stored.IsRevoked = true;
			stored.RevokedAtUtc = DateTime.UtcNow;
			await _context.SaveChangesAsync();
		}
	}

	private static string GenerateRefreshToken()
	{
		var randomBytes = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomBytes);
		return Convert.ToBase64String(randomBytes);
	}
}