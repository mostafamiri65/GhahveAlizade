using ErrorOr;
using GhahveAlizade.Application.Features.UserPanel.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyDashboard;

public record GetMyDashboardQuery(long UserId) : IRequest<ErrorOr<MyDashboardDto>>;
