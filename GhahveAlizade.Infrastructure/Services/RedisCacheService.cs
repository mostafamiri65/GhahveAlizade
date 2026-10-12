using System.Text.Json;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Services;

public class RedisCacheService : ICacheService
{
	private readonly IDistributedCache _cache;
	private readonly ILogger<RedisCacheService> _logger;
	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
	{
		_cache = cache;
		_logger = logger;
	}

	public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
	{
		try
		{
			var json = await _cache.GetStringAsync(key, cancellationToken);
			if (string.IsNullOrEmpty(json)) return default;
			return JsonSerializer.Deserialize<T>(json, JsonOptions);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error getting cache key {Key}", key);
			return default;
		}
	}

	public async Task SetAsync<T>(
		string key,
		T value,
		TimeSpan? expiration = null,
		CancellationToken cancellationToken = default)
	{
		try
		{
			var json = JsonSerializer.Serialize(value, JsonOptions);
			var options = new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
			};
			await _cache.SetStringAsync(key, json, options, cancellationToken);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error setting cache key {Key}", key);
		}
	}

	public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
	{
		try
		{
			await _cache.RemoveAsync(key, cancellationToken);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error removing cache key {Key}", key);
		}
	}

	public Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
	{
		// نیاز به StackExchange.Redis مستقیم برای SCAN
		// فعلاً No-op، بعداً پیاده‌سازی می‌کنیم
		_logger.LogWarning("RemoveByPrefixAsync not implemented yet for prefix {Prefix}", prefix);
		return Task.CompletedTask;
	}
}