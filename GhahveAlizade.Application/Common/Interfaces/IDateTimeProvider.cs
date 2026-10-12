namespace GhahveAlizade.Application.Common.Interfaces;

public interface IDateTimeProvider
{
	DateTime UtcNow { get; }
	DateOnly TodayUtc { get; }
}