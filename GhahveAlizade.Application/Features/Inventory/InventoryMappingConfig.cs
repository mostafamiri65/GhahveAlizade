using GhahveAlizade.Application.Features.Catalog.Brands.Dtos;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using GhahveAlizade.Domain.Inventory;
using Mapster;

namespace GhahveAlizade.Application.Features.Inventory;

public class InventoryMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Warehouse, WarehouseDto>()
			.Map(dest => dest.InventoryItemCount, src => 0);

		config.NewConfig<Warehouse, WarehouseListItemDto>()
			.Map(dest => dest.InventoryItemCount, src => 0);

		config.NewConfig<Branch, BranchDto>();
		config.NewConfig<Branch, BranchListItemDto>();

		config.NewConfig<InventoryItem, InventoryItemDto>()
			.Map(dest => dest.ProductName, src => src.Product != null ? src.Product.Name : "")
			.Map(dest => dest.ProductSlug, src => src.Product != null ? src.Product.Slug : "")
			.Map(dest => dest.ProductSku, src => src.Product != null ? src.Product.Sku : null)
			.Map(dest => dest.VariantName, src => src.ProductVariant != null ? src.ProductVariant.Name : null)
			.Map(dest => dest.WarehouseName, src => src.Warehouse != null ? src.Warehouse.Name : "");

		config.NewConfig<InventoryItem, InventoryItemListItemDto>()
			.Map(dest => dest.ProductName, src => src.Product != null ? src.Product.Name : "")
			.Map(dest => dest.ProductSku, src => src.Product != null ? src.Product.Sku : null)
			.Map(dest => dest.VariantName, src => src.ProductVariant != null ? src.ProductVariant.Name : null)
			.Map(dest => dest.WarehouseName, src => src.Warehouse != null ? src.Warehouse.Name : "");

		config.NewConfig<InventoryTransaction, InventoryTransactionDto>();
	}
}