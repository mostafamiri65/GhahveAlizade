using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Products.Commands.PublishProduct;

public record PublishProductCommand(long Id) : IRequest<ErrorOr<bool>>;