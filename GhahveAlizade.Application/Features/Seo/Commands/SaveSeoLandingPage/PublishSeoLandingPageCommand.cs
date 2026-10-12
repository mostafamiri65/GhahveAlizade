using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveSeoLandingPage;

public record PublishSeoLandingPageCommand(long Id) : IRequest<ErrorOr<bool>>;