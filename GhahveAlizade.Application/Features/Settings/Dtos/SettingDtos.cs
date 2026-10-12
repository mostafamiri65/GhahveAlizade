namespace GhahveAlizade.Application.Features.Settings.Dtos;

public class SettingDto
{
	public long Id { get; set; }
	public string Key { get; set; } = string.Empty;
	public string? Value { get; set; }
	public string? Description { get; set; }
	public string? Group { get; set; }
	public string ValueType { get; set; } = "String";
	public bool IsPublic { get; set; }
	public int DisplayOrder { get; set; }
}

public class SettingGroupDto
{
	public string Group { get; set; } = string.Empty;
	public string? GroupDisplayName { get; set; }
	public List<SettingDto> Settings { get; set; } = new();
}