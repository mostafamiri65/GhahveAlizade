using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.UserPanel.Commands.ToggleMyWishlistItem;

public record ToggleMyWishlistItemCommand(long UserId, long ProductId)
	: IRequest<ErrorOr<bool>>; // true = اضافه شد، false = حذف شد
