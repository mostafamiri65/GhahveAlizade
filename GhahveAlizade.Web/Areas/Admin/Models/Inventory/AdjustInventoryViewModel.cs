namespace GhahveAlizade.Web.Areas.Admin.Models.Inventory;

public class AdjustInventoryViewModel
{
	public long InventoryItemId { get; set; }
	public decimal CurrentQuantity { get; set; }
	public decimal NewQuantity { get; set; }
	public string? Notes { get; set; }
}