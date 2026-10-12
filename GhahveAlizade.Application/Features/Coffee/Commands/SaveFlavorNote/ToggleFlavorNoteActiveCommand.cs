using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveFlavorNote;

public record ToggleFlavorNoteActiveCommand(long Id) : IRequest<ErrorOr<bool>>;