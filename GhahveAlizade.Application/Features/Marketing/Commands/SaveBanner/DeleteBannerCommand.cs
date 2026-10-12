using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Marketing.Commands.SaveBanner;

public record DeleteBannerCommand(long Id) : IRequest<ErrorOr<Success>>;
