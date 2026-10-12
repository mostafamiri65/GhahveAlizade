using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveSeoLandingPage;

public record DeleteSeoLandingPageCommand(long Id) : IRequest<ErrorOr<Success>>;
