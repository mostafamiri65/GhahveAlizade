using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.AiAdvisor.Queries.GetCatalogContext;

public record GetCatalogContextQuery() : IRequest<ErrorOr<string>>;
