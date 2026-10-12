using System.Security.Claims;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GhahveAlizade.Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
		=> _httpContextAccessor = httpContextAccessor;

	public long? UserId
	{
		get
		{
			var id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
			return long.TryParse(id, out var userId) ? userId : null;
		}
	}

	public string? UserName
		=> _httpContextAccessor.HttpContext?.User?.Identity?.Name;

	public bool IsAuthenticated
		=> _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

	public string? IpAddress
		=> _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

	public string? UserAgent
		=> _httpContextAccessor.HttpContext?.Request?.Headers.UserAgent.ToString();
}