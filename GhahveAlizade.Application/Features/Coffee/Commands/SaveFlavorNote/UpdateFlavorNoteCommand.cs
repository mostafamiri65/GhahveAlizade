using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveFlavorNote;

public record UpdateFlavorNoteCommand(
	long Id, string Name, string? Slug, string? Category, string? IconUrl,
	string? ColorCode, bool IsActive, int DisplayOrder) : IRequest<ErrorOr<Success>>;
