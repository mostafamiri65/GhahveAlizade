using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Bundles.Commands.SaveBundle;

public record ToggleBundleActiveCommand(long Id) : IRequest<ErrorOr<bool>>;