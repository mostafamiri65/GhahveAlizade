using ErrorOr;
using GhahveAlizade.Application.Features.UserPanel.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyAddresses;

public record GetMyAddressesQuery(long UserId) : IRequest<ErrorOr<List<MyAddressDto>>>;
