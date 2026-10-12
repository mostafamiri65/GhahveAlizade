using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Newsletter.Commands.ToggleSubscriber;

public record VerifySubscriberCommand(long Id) : IRequest<ErrorOr<Success>>;