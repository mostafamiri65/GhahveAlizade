using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Newsletter.Commands.ToggleSubscriber;

public record ToggleSubscriberActiveCommand(long Id) : IRequest<ErrorOr<bool>>;
