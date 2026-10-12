using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Settings.Commands.SaveSettings;

public record DeleteSettingCommand(long Id) : IRequest<ErrorOr<Success>>;