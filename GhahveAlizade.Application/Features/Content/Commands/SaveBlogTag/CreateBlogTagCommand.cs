using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogTag;

public record CreateBlogTagCommand(string Name, string? Slug, string? Description)
	: IRequest<ErrorOr<long>>;
