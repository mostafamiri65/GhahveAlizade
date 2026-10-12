using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Products.Commands.DeleteProduct;

public record DeleteProductCommand(long Id) : IRequest<ErrorOr<Success>>;