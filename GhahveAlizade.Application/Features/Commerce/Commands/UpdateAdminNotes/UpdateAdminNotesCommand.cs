using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Commerce.Commands.UpdateAdminNotes;

public record UpdateAdminNotesCommand(long OrderId, string? AdminNotes) : IRequest<ErrorOr<Success>>;
