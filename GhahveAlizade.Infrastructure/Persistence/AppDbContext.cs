using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Catalog;
using GhahveAlizade.Domain.Coffee;
using GhahveAlizade.Domain.Commerce;
using GhahveAlizade.Domain.Content;
using GhahveAlizade.Domain.Customer;
using GhahveAlizade.Domain.Identity;
using GhahveAlizade.Domain.Inventory;
using GhahveAlizade.Domain.Loyalty;
using GhahveAlizade.Domain.Marketing;
using GhahveAlizade.Domain.Newsletter;
using GhahveAlizade.Domain.Payment;
using GhahveAlizade.Domain.Pricing;
using GhahveAlizade.Domain.Promotion;
using GhahveAlizade.Domain.Review;
using GhahveAlizade.Domain.SEO;
using GhahveAlizade.Domain.Settings;
using GhahveAlizade.Domain.Shipping;
using GhahveAlizade.Domain.Support;
using GhahveAlizade.Domain.Wholesale;
using GhahveAlizade.Domain.Wishlist;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Reflection;
using System.Security;

namespace GhahveAlizade.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>, IAppDbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<AppSetting> AppSettings => Set<AppSetting>();
	// Catalog
	public DbSet<Category> Categories => Set<Category>();
	public DbSet<Brand> Brands => Set<Brand>();
	public DbSet<Product> Products => Set<Product>();
	public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
	public DbSet<ProductImage> ProductImages => Set<ProductImage>();
	public DbSet<Domain.Catalog.Attribute> Attributes => Set<Domain.Catalog.Attribute>();
	public DbSet<AttributeValue> AttributeValues => Set<AttributeValue>();
	public DbSet<ProductAttribute> ProductAttributes => Set<ProductAttribute>();
	public DbSet<ProductRelation> ProductRelations => Set<ProductRelation>();

	// Coffee
	public DbSet<CoffeeOrigin> CoffeeOrigins => Set<CoffeeOrigin>();
	public DbSet<CoffeeRegion> CoffeeRegions => Set<CoffeeRegion>();
	public DbSet<CoffeeProcess> CoffeeProcesses => Set<CoffeeProcess>();
	public DbSet<RoastProfile> RoastProfiles => Set<RoastProfile>();
	public DbSet<FlavorNote> FlavorNotes => Set<FlavorNote>();
	public DbSet<ProductBatch> ProductBatches => Set<ProductBatch>();
	public DbSet<BatchInventory> BatchInventories => Set<BatchInventory>();

	// Inventory
	public DbSet<Warehouse> Warehouses => Set<Warehouse>();
	public DbSet<Branch> Branches => Set<Branch>();
	public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
	public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
	public DbSet<StockReservation> StockReservations => Set<StockReservation>();

	// Customer
	public DbSet<Customer> Customers => Set<Customer>();
	public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
	public DbSet<CustomerNote> CustomerNotes => Set<CustomerNote>();

	// Commerce
	public DbSet<Cart> Carts => Set<Cart>();
	public DbSet<CartItem> CartItems => Set<CartItem>();
	public DbSet<Domain.Commerce.Order> Orders => Set<Domain.Commerce.Order>();
	public DbSet<OrderItem> OrderItems => Set<OrderItem>();
	public DbSet<OrderAddress> OrderAddresses => Set<OrderAddress>();

	// Payment
	public DbSet<Payment> Payments => Set<Payment>();
	public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();

	// Shipping
	public DbSet<ShippingMethod> ShippingMethods => Set<ShippingMethod>();
	public DbSet<Shipment> Shipments => Set<Shipment>();

	// Promotion
	public DbSet<Coupon> Coupons => Set<Coupon>();
	public DbSet<CouponUsage> CouponUsages => Set<CouponUsage>();
	public DbSet<CustomerCoupon> CustomerCoupons => Set<CustomerCoupon>();

	// Content
	public DbSet<BlogCategory> BlogCategories => Set<BlogCategory>();
	public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
	public DbSet<BlogTag> BlogTags => Set<BlogTag>();
	public DbSet<BlogPostTag> BlogPostTags => Set<BlogPostTag>();

	// SEO
	public DbSet<SeoMetadata> SeoMetadata => Set<SeoMetadata>();
	public DbSet<Redirect> Redirects => Set<Redirect>();
	public DbSet<SeoLandingPage> SeoLandingPages => Set<SeoLandingPage>();

	// Review
	public DbSet<ProductReview> ProductReviews => Set<ProductReview>();
	public DbSet<ReviewImage> ReviewImages => Set<ReviewImage>();
	public DbSet<ReviewReply> ReviewReplies => Set<ReviewReply>();

	// Wishlist
	public DbSet<Wishlist> Wishlists => Set<Wishlist>();
	public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();

	// Bundle
	public DbSet<Domain.Bundle.ProductBundle> ProductBundles => Set<Domain.Bundle.ProductBundle>();
	public DbSet<Domain.Bundle.ProductBundleItem> ProductBundleItems => Set<Domain.Bundle.ProductBundleItem>();

	// Loyalty
	public DbSet<CustomerPointAccount> CustomerPointAccounts => Set<CustomerPointAccount>();
	public DbSet<PointTransaction> PointTransactions => Set<PointTransaction>();

	// Marketing
	public DbSet<Campaign> Campaigns => Set<Campaign>();
	public DbSet<Banner> Banners => Set<Banner>();
	public DbSet<BannerPlacement> BannerPlacements => Set<BannerPlacement>();
	public DbSet<AbandonedCart> AbandonedCarts => Set<AbandonedCart>();
	public DbSet<AbandonedCartNotification> AbandonedCartNotifications => Set<AbandonedCartNotification>();
	public DbSet<Referral> Referrals => Set<Referral>();
	public DbSet<ReferralReward> ReferralRewards => Set<ReferralReward>();

	// Newsletter
	public DbSet<NewsletterSubscriber> NewsletterSubscribers => Set<NewsletterSubscriber>();

	// Wholesale
	public DbSet<CustomerGroup> CustomerGroups => Set<CustomerGroup>();
	public DbSet<WholesaleCustomer> WholesaleCustomers => Set<WholesaleCustomer>();
	public DbSet<WholesaleRequest> WholesaleRequests => Set<WholesaleRequest>();

	// Pricing
	public DbSet<PriceList> PriceLists => Set<PriceList>();
	public DbSet<PriceListItem> PriceListItems => Set<PriceListItem>();
	public DbSet<QuantityPrice> QuantityPrices => Set<QuantityPrice>();

	// Accounting
	public DbSet<Domain.Accounting.AccountingSyncLog> AccountingSyncLogs => Set<Domain.Accounting.AccountingSyncLog>();
	public DbSet<Domain.Accounting.AccountingMapping> AccountingMappings => Set<Domain.Accounting.AccountingMapping>();

	// Support (تیکتینگ)
	public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
	public DbSet<TicketMessage> TicketMessages => Set<TicketMessage>();

	// Support (چت آنلاین)
	public DbSet<SupportChatSession> SupportChatSessions => Set<SupportChatSession>();
	public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

	// Identity
	public DbSet<Permission> Permissions => Set<Permission>();
	public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
	public DbSet<PhoneOtp> PhoneOtps => Set<PhoneOtp>();
	public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);


		// 1. اعمال همه Configurationها
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		// 2. Collation فارسی
		builder.UseCollation("Persian_100_CI_AS_SC");

		// 3. پیش‌فرض decimal برای همه‌ی propertyها (fallback)
		foreach (var property in builder.Model.GetEntityTypes()
			.SelectMany(t => t.GetProperties())
			.Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
		{
			if (property.GetColumnType() is null)
			{
				property.SetColumnType("decimal(18,2)");
			}
		}

		// 4. Global Query Filter برای Soft Delete
		foreach (var entityType in builder.Model.GetEntityTypes())
		{
			if (typeof(Domain.Common.BaseEntity).IsAssignableFrom(entityType.ClrType))
			{
				var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
				var property = System.Linq.Expressions.Expression.Property(parameter, nameof(Domain.Common.BaseEntity.IsDeleted));
				var filter = System.Linq.Expressions.Expression.Lambda(
					System.Linq.Expressions.Expression.Equal(property, System.Linq.Expressions.Expression.Constant(false)),
					parameter);

				builder.Entity(entityType.ClrType).HasQueryFilter(filter);
			}
		}

		// تنظیم Identity
		builder.Entity<ApplicationUser>(b => b.ToTable("Users"));
		builder.Entity<ApplicationRole>(b => b.ToTable("Roles"));
		builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<long>>(b => b.ToTable("UserRoles"));
		builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<long>>(b => b.ToTable("UserClaims"));
		builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<long>>(b => b.ToTable("UserLogins"));
		builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<long>>(b => b.ToTable("UserTokens"));
		builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>>(b => b.ToTable("RoleClaims"));
	}
}
