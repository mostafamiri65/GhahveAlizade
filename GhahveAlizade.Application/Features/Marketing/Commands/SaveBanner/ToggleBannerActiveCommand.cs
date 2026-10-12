using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Marketing.Commands.SaveBanner;
public record ToggleBannerActiveCommand(long Id) : IRequest<ErrorOr<bool>>;