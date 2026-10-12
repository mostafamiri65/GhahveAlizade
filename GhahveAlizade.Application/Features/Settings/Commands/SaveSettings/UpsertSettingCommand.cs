using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Settings.Commands.SaveSettings;

public record UpsertSettingCommand(
	string Key, string? Value, string? Description,
	string? Group, string ValueType, bool IsPublic, int DisplayOrder)
	: IRequest<ErrorOr<long>>;
