using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogTag;

public record DeleteBlogTagCommand(long Id) : IRequest<ErrorOr<Success>>;