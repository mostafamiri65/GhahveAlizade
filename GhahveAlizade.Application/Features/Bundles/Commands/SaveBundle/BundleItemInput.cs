namespace GhahveAlizade.Application.Features.Bundles.Commands.SaveBundle;

public record BundleItemInput(long ProductId, long? ProductVariantId, int Quantity, int DisplayOrder);
