using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Commands.DeleteAttribute;

public record DeleteAttributeCommand(long Id) : IRequest<ErrorOr<Success>>;