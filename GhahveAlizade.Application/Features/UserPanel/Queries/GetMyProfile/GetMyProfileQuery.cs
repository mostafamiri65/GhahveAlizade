using ErrorOr;
using GhahveAlizade.Application.Features.UserPanel.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyProfile;

public record GetMyProfileQuery(long UserId) : IRequest<ErrorOr<MyProfileDto>>;
