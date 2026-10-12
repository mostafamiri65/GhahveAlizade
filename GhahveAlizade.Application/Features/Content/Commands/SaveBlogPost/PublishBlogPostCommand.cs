using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogPost;

public record PublishBlogPostCommand(long Id) : IRequest<ErrorOr<bool>>;
