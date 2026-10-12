using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GhahveAlizade.Infrastructure.Persistence.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
	private readonly ICurrentUserService _currentUser;
	private readonly IDateTimeProvider _dateTime;

	public AuditableEntityInterceptor(ICurrentUserService currentUser, IDateTimeProvider dateTime)
	{
		_currentUser = currentUser;
		_dateTime = dateTime;
	}

	public override InterceptionResult<int> SavingChanges(
		DbContextEventData eventData,
		InterceptionResult<int> result)
	{
		UpdateEntities(eventData.Context);
		return base.SavingChanges(eventData, result);
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
	{
		UpdateEntities(eventData.Context);
		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private void UpdateEntities(DbContext? context)
	{
		if (context is null) return;

		var now = _dateTime.UtcNow;
		var userId = _currentUser.UserId ?? 1;

		foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedAtUtc = now;
					entry.Entity.CreatedBy =userId;
					break;

				case EntityState.Modified:
					entry.Entity.UpdatedAtUtc = now;
					entry.Entity.UpdatedBy = userId;
					break;

				case EntityState.Deleted:
					// Soft delete
					entry.State = EntityState.Modified;
					entry.Entity.IsDeleted = true;
					entry.Entity.DeletedAtUtc = now;
					entry.Entity.DeletedBy = userId;
					break;
			}
		}
	}
}