namespace GhahveAlizade.Application.Common.Interfaces;

/// <summary>
/// برای ورود/خروج در MVC (Cookie). Application فقط این رو می‌بینه.
/// </summary>
public interface ICookieAuthService
{
	Task SignInAsync(long userId, bool isPersistent = false, CancellationToken cancellationToken = default);
	Task SignOutAsync(CancellationToken cancellationToken = default);
}