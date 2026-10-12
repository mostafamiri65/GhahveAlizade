using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogCategory;

public record DeleteBlogCategoryCommand(long Id) : IRequest<ErrorOr<Success>>;
