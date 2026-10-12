using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveFlavorNote;

public record DeleteFlavorNoteCommand(long Id) : IRequest<ErrorOr<Success>>;
