namespace GhahveAlizade.Infrastructure.Persistence.Seed;

public static class Permissions
{
	// ============ Users & Roles ============
	public static class Users
	{
		public const string View = "Users.View";
		public const string Create = "Users.Create";
		public const string Edit = "Users.Edit";
		public const string Delete = "Users.Delete";
		public const string ManageRoles = "Users.ManageRoles";
		public const string ResetPassword = "Users.ResetPassword";
	}

	public static class Roles
	{
		public const string View = "Roles.View";
		public const string Create = "Roles.Create";
		public const string Edit = "Roles.Edit";
		public const string Delete = "Roles.Delete";
		public const string ManagePermissions = "Roles.ManagePermissions";
	}

	// ============ Catalog ============
	public static class Products
	{
		public const string View = "Products.View";
		public const string Create = "Products.Create";
		public const string Edit = "Products.Edit";
		public const string Delete = "Products.Delete";
		public const string Publish = "Products.Publish";
		public const string ManageImages = "Products.ManageImages";
		public const string ManageVariants = "Products.ManageVariants";
		public const string ManageAttributes = "Products.ManageAttributes";
	}
	public static class Admin
	{
		public const string Access = "Admin.Access";
		public const string ViewDashboard = "Admin.ViewDashboard";
	}
	public static class Categories
	{
		public const string View = "Categories.View";
		public const string Create = "Categories.Create";
		public const string Edit = "Categories.Edit";
		public const string Delete = "Categories.Delete";
	}

	public static class Brands
	{
		public const string View = "Brands.View";
		public const string Create = "Brands.Create";
		public const string Edit = "Brands.Edit";
		public const string Delete = "Brands.Delete";
	}

	public static class Attributes
	{
		public const string View = "Attributes.View";
		public const string Create = "Attributes.Create";
		public const string Edit = "Attributes.Edit";
		public const string Delete = "Attributes.Delete";
	}

	public static class Bundles
	{
		public const string View = "Bundles.View";
		public const string Create = "Bundles.Create";
		public const string Edit = "Bundles.Edit";
		public const string Delete = "Bundles.Delete";
	}

	// ============ Coffee ============
	public static class Coffee
	{
		public const string ViewOrigins = "Coffee.ViewOrigins";
		public const string ManageOrigins = "Coffee.ManageOrigins";
		public const string ViewProcesses = "Coffee.ViewProcesses";
		public const string ManageProcesses = "Coffee.ManageProcesses";
		public const string ViewRoastProfiles = "Coffee.ViewRoastProfiles";
		public const string ManageRoastProfiles = "Coffee.ManageRoastProfiles";
		public const string ViewFlavorNotes = "Coffee.ViewFlavorNotes";
		public const string ManageFlavorNotes = "Coffee.ManageFlavorNotes";
		public const string ManageBatches = "Coffee.ManageBatches";
	}

	// ============ Inventory ============
	public static class Inventory
	{
		public const string View = "Inventory.View";
		public const string Adjust = "Inventory.Adjust";
		public const string Transfer = "Inventory.Transfer";
		public const string ViewTransactions = "Inventory.ViewTransactions";
		public const string ManageWarehouses = "Inventory.ManageWarehouses";
		public const string ManageBranches = "Inventory.ManageBranches";
	}

	// ============ Orders ============
	public static class Orders
	{
		public const string View = "Orders.View";
		public const string ViewDetails = "Orders.ViewDetails";
		public const string Edit = "Orders.Edit";
		public const string Cancel = "Orders.Cancel";
		public const string Refund = "Orders.Refund";
		public const string ChangeStatus = "Orders.ChangeStatus";
		public const string Print = "Orders.Print";
		public const string Export = "Orders.Export";
	}

	public static class Payments
	{
		public const string View = "Payments.View";
		public const string Verify = "Payments.Verify";
		public const string Refund = "Payments.Refund";
	}

	public static class Shipments
	{
		public const string View = "Shipments.View";
		public const string Create = "Shipments.Create";
		public const string Edit = "Shipments.Edit";
		public const string UpdateTracking = "Shipments.UpdateTracking";
		public const string ManageMethods = "Shipments.ManageMethods";
	}

	// ============ Customers ============
	public static class Customers
	{
		public const string View = "Customers.View";
		public const string ViewDetails = "Customers.ViewDetails";
		public const string Create = "Customers.Create";
		public const string Edit = "Customers.Edit";
		public const string Delete = "Customers.Delete";
		public const string ManageNotes = "Customers.ManageNotes";
		public const string ManageGroups = "Customers.ManageGroups";
		public const string Export = "Customers.Export";
	}

	// ============ Wholesale ============
	public static class Wholesale
	{
		public const string View = "Wholesale.View";
		public const string Approve = "Wholesale.Approve";
		public const string Reject = "Wholesale.Reject";
		public const string ManageCustomers = "Wholesale.ManageCustomers";
		public const string ManageRequests = "Wholesale.ManageRequests";
	}

	// ============ Content (Blog) ============
	public static class Blog
	{
		public const string ViewPosts = "Blog.ViewPosts";
		public const string CreatePost = "Blog.CreatePost";
		public const string EditPost = "Blog.EditPost";
		public const string DeletePost = "Blog.DeletePost";
		public const string PublishPost = "Blog.PublishPost";
		public const string ManageCategories = "Blog.ManageCategories";
		public const string ManageTags = "Blog.ManageTags";
	}

	// ============ Reviews ============
	public static class Reviews
	{
		public const string View = "Reviews.View";
		public const string Approve = "Reviews.Approve";
		public const string Reject = "Reviews.Reject";
		public const string Reply = "Reviews.Reply";
		public const string Delete = "Reviews.Delete";
	}

	// ============ SEO ============
	public static class Seo
	{
		public const string ViewMetadata = "Seo.ViewMetadata";
		public const string EditMetadata = "Seo.EditMetadata";
		public const string ManageRedirects = "Seo.ManageRedirects";
		public const string ManageLandingPages = "Seo.ManageLandingPages";
	}

	// ============ Marketing ============
	public static class Marketing
	{
		public const string ViewCampaigns = "Marketing.ViewCampaigns";
		public const string ManageCampaigns = "Marketing.ManageCampaigns";
		public const string ViewBanners = "Marketing.ViewBanners";
		public const string ManageBanners = "Marketing.ManageBanners";
		public const string ViewAbandonedCarts = "Marketing.ViewAbandonedCarts";
		public const string ManageReferrals = "Marketing.ManageReferrals";
	}

	public static class Coupons
	{
		public const string View = "Coupons.View";
		public const string Create = "Coupons.Create";
		public const string Edit = "Coupons.Edit";
		public const string Delete = "Coupons.Delete";
	}

	public static class Newsletter
	{
		public const string View = "Newsletter.View";
		public const string Send = "Newsletter.Send";
		public const string Export = "Newsletter.Export";
	}

	// ============ Pricing ============
	public static class Pricing
	{
		public const string View = "Pricing.View";
		public const string ManagePriceLists = "Pricing.ManagePriceLists";
		public const string ManageQuantityPrices = "Pricing.ManageQuantityPrices";
	}

	// ============ Loyalty ============
	public static class Loyalty
	{
		public const string View = "Loyalty.View";
		public const string AdjustPoints = "Loyalty.AdjustPoints";
		public const string ManageTiers = "Loyalty.ManageTiers";
	}

	// ============ Reports ============
	public static class Reports
	{
		public const string ViewSales = "Reports.ViewSales";
		public const string ViewInventory = "Reports.ViewInventory";
		public const string ViewCustomers = "Reports.ViewCustomers";
		public const string ViewMarketing = "Reports.ViewMarketing";
		public const string Export = "Reports.Export";
	}

	// ============ Settings ============
	public static class Settings
	{
		public const string View = "Settings.View";
		public const string Edit = "Settings.Edit";
		public const string ManagePayment = "Settings.ManagePayment";
		public const string ManageShipping = "Settings.ManageShipping";
		public const string ManageTax = "Settings.ManageTax";
		public const string ViewLogs = "Settings.ViewLogs";
		public const string ManageAccounting = "Settings.ManageAccounting";
	}
}