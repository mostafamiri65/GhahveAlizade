using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Bundles.Commands.SaveBundle;

public record DeleteBundleCommand(long Id) : IRequest<ErrorOr<Success>>;
