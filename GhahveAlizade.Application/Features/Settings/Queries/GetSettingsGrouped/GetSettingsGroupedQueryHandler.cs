using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Settings.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Settings.Queries.GetSettingsGrouped;

public class GetSettingsGroupedQueryHandler
	: IRequestHandler<GetSettingsGroupedQuery, ErrorOr<List<SettingGroupDto>>>
{
	private readonly IAppDbContext _context;

	public GetSettingsGroupedQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<SettingGroupDto>>> Handle(
		GetSettingsGroupedQuery request, CancellationToken ct)
	{
		var settings = await _context.AppSettings
			.AsNoTracking()
			.Where(s => !s.IsDeleted)
			.OrderBy(s => s.Group).ThenBy(s => s.DisplayOrder)
			.ProjectToType<SettingDto>()
			.ToListAsync(ct);

		var grouped = settings
			.GroupBy(s => s.Group ?? "Other")
			.Select(g => new SettingGroupDto
			{
				Group = g.Key,
				GroupDisplayName = GetGroupDisplayName(g.Key),
				Settings = g.ToList()
			})
			.ToList();

		return grouped;
	}

	private static string GetGroupDisplayName(string group) => group switch
	{
		"General" => "عمومی",
		"Payment" => "پرداخت",
		"Shipping" => "ارسال",
		"Seo" => "سئو",
		"Social" => "شبکه‌های اجتماعی",
		"Contact" => "تماس با ما",
		"Email" => "ایمیل",
		"Sms" => "پیامک",
		_ => group
	};
}