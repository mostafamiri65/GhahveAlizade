using ErrorOr;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetFlavorNotes;

public record GetFlavorNoteByIdQuery(long Id) : IRequest<ErrorOr<FlavorNoteDto>>;
