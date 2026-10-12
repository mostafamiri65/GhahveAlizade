using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveRoastProfile;

public record CreateRoastProfileCommand(
	string Name, string? Slug, string? Description, string? ColorCode,
	int RoastLevel, bool IsActive, int DisplayOrder) : IRequest<ErrorOr<long>>;
