using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogTag;

public record UpdateBlogTagCommand(long Id, string Name, string? Slug, string? Description)
	: IRequest<ErrorOr<Success>>;
