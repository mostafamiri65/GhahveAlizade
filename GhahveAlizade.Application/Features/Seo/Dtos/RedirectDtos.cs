using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Application.Features.Seo.Dtos;

public class RedirectDto
{
	public long Id { get; set; }
	public string FromPath { get; set; } = string.Empty;
	public string ToPath { get; set; } = string.Empty;
	public RedirectType Type { get; set; }
	public string? TypeName { get; set; }
	public bool IsActive { get; set; }
	public int HitCount { get; set; }
	public DateTime? LastHitAtUtc { get; set; }
	public string? Notes { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}

public class RedirectListItemDto
{
	public long Id { get; set; }
	public string FromPath { get; set; } = string.Empty;
	public string ToPath { get; set; } = string.Empty;
	public RedirectType Type { get; set; }
	public string? TypeName { get; set; }
	public bool IsActive { get; set; }
	public int HitCount { get; set; }
	public DateTime? LastHitAtUtc { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}