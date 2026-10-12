using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Newsletter.Commands.ToggleSubscriber;

public record DeleteSubscriberCommand(long Id) : IRequest<ErrorOr<Success>>;
