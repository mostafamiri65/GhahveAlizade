using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.UserPanel.Commands.DeleteMyAddress;

public record DeleteMyAddressCommand(long UserId, long AddressId) : IRequest<ErrorOr<Deleted>>;
