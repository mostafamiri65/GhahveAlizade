using ErrorOr;
using GhahveAlizade.Application.Features.Settings.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Settings.Queries.GetSettingsGrouped;

public record GetSettingsGroupedQuery : IRequest<ErrorOr<List<SettingGroupDto>>>;
