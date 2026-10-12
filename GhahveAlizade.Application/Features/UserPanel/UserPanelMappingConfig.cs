using GhahveAlizade.Application.Features.UserPanel.Dtos;
using GhahveAlizade.Domain.Commerce;
using GhahveAlizade.Domain.Customer;
using GhahveAlizade.Domain.Loyalty;
using GhahveAlizade.Domain.Review;
using GhahveAlizade.Domain.Wishlist;
using Mapster;

namespace GhahveAlizade.Application.Features.UserPanel;

public class UserPanelMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Order, MyOrderListItemDto>()
			.Map(d => d.ItemCount, s => s.Items.Count)
			.Map(d => d.FirstItemImageUrl,
				s => s.Items.OrderBy(i => i.Id).FirstOrDefault() != null
					? s.Items.OrderBy(i => i.Id).First().ProductImageUrl
					: null);

		config.NewConfig<CustomerAddress, MyAddressDto>();

		config.NewConfig<WishlistItem, MyWishlistItemDto>()
			.Map(d => d.ItemId, s => s.Id)
			.Map(d => d.ProductId, s => s.ProductId)
			.Map(d => d.ProductName, s => s.Product.Name)
			.Map(d => d.ProductSlug, s => s.Product.Slug)
			.Map(d => d.ImageUrl, s => s.Product.Images != null && s.Product.Images.Any()
				? (s.Product.Images.FirstOrDefault(i => i.IsPrimary) ?? s.Product.Images.First()).ImageUrl
				: null)
			.Map(d => d.Price, s => s.Product.BasePrice)
			.Map(d => d.AddedAtUtc, s => s.CreatedAtUtc);

		config.NewConfig<PointTransaction, MyPointTransactionDto>();
	}
}
