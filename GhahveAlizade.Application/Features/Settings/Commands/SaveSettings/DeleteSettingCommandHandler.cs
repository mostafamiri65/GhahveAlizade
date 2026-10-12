using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Settings.Commands.SaveSettings;

public class DeleteSettingCommandHandler
	: IRequestHandler<DeleteSettingCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public DeleteSettingCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteSettingCommand request, CancellationToken ct)
	{
		var setting = await _context.AppSettings
			.FirstOrDefaultAsync(s => s.Id == request.Id && !s.IsDeleted, ct);

		if (setting is null) return Error.NotFound("Setting.NotFound", "تنظیم یافت نشد.");

		_context.AppSettings.Remove(setting);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}