using ErrorOr;
using GhahveAlizade.Application.Features.Seo.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Queries.GetRedirectById;

public record GetRedirectByIdQuery(long Id) : IRequest<ErrorOr<RedirectDto>>;
