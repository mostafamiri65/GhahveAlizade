using ErrorOr;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetBranchesLookup;

public record GetBranchesLookupQuery(bool OnlyActive = true) : IRequest<ErrorOr<List<LookupItemDto>>>;
