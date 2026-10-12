using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveRedirect;
public record ToggleRedirectActiveCommand(long Id) : IRequest<ErrorOr<bool>>;