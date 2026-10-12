using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogPost;
public record ToggleBlogPostFeaturedCommand(long Id) : IRequest<ErrorOr<bool>>;