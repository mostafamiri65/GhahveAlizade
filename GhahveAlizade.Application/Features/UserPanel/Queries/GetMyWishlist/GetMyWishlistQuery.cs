using ErrorOr;
using GhahveAlizade.Application.Features.UserPanel.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyWishlist;

public record GetMyWishlistQuery(long UserId) : IRequest<ErrorOr<List<MyWishlistItemDto>>>;
