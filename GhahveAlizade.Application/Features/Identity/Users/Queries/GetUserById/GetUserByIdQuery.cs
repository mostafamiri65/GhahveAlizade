using ErrorOr;
using GhahveAlizade.Application.Features.Identity.Users.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Identity.Users.Queries.GetUserById;

public record GetUserByIdQuery(long Id) : IRequest<ErrorOr<UserDetailDto>>;