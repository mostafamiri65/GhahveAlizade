using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogPost;

public record DeleteBlogPostCommand(long Id) : IRequest<ErrorOr<Success>>;
