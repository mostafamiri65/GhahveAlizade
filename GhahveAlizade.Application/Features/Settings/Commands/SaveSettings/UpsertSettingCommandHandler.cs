using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Settings.Commands.SaveSettings;

public class UpsertSettingCommandHandler
	: IRequestHandler<UpsertSettingCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;

	public UpsertSettingCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(UpsertSettingCommand request, CancellationToken ct)
	{
		var key = request.Key.Trim();

		var setting = await _context.AppSettings
			.FirstOrDefaultAsync(s => s.Key == key && !s.IsDeleted, ct);

		if (setting is null)
		{
			setting = new AppSetting
			{
				Key = key,
				Value = request.Value,
				Description = request.Description?.Trim(),
				Group = request.Group?.Trim(),
				ValueType = request.ValueType,
				IsPublic = request.IsPublic,
				DisplayOrder = request.DisplayOrder
			};
			_context.AppSettings.Add(setting);
		}
		else
		{
			setting.Value = request.Value;
			setting.Description = request.Description?.Trim();
			setting.Group = request.Group?.Trim();
			setting.ValueType = request.ValueType;
			setting.IsPublic = request.IsPublic;
			setting.DisplayOrder = request.DisplayOrder;
		}

		await _context.SaveChangesAsync(ct);
		return setting.Id;
	}
}
