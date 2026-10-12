using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveRedirect;

public record DeleteRedirectCommand(long Id) : IRequest<ErrorOr<Success>>;
