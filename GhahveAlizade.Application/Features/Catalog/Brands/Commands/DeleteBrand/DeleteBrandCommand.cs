using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Commands.DeleteBrand;

public record DeleteBrandCommand(long Id) : IRequest<ErrorOr<Success>>;