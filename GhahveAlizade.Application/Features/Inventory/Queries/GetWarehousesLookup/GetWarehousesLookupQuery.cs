using ErrorOr;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;

public record GetWarehousesLookupQuery : IRequest<ErrorOr<List<LookupItemDto>>>;
