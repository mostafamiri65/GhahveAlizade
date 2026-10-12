using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogCategory;
public record ToggleBlogCategoryActiveCommand(long Id) : IRequest<ErrorOr<bool>>;