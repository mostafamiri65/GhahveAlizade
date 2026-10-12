using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Settings.Commands.SaveSettings;

public record SaveSettingsCommand(List<SettingValueInput> Settings)
	: IRequest<ErrorOr<Success>>;
