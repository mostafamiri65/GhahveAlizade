using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveSeoMetadata;

public record DeleteSeoMetadataCommand(long Id) : IRequest<ErrorOr<Success>>;