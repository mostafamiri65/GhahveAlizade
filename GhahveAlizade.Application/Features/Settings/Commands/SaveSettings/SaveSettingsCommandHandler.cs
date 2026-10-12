using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Settings.Commands.SaveSettings;

public class SaveSettingsCommandHandler
	: IRequestHandler<SaveSettingsCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public SaveSettingsCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(SaveSettingsCommand request, CancellationToken ct)
	{
		var keys = request.Settings.Select(s => s.Key).ToList();

		var existing = await _context.AppSettings
			.Where(s => keys.Contains(s.Key) && !s.IsDeleted)
			.ToListAsync(ct);

		foreach (var input in request.Settings)
		{
			var setting = existing.FirstOrDefault(s => s.Key == input.Key);
			if (setting is not null)
			{
				setting.Value = input.Value;
			}
		}

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
