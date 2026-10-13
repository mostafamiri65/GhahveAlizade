using GhahveAlizade.Domain.Accounting;
using GhahveAlizade.Domain.AiAdvisor;
using GhahveAlizade.Domain.Bundle;
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
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Metadata;

namespace GhahveAlizade.Application.Common.Interfaces;

public interface IAppDbContext
{
	DbSet<AppSetting> AppSettings { get; }
	DbSet<ApplicationUser> Users { get; }
	DbSet<ApplicationRole> Roles { get; }
	DbSet<ApplicationUser> Users { get; }
	DbSet<PhoneOtp> PhoneOtps { get; }
	DbSet<RefreshToken> RefreshTokens { get; }
	DbSet<RolePermission> RolePermissions { get; }
	DbSet<Permission> Permissions { get; }
	DbSet<Category> Categories { get; }
	DbSet<Brand> Brands { get; }
	DbSet<Product> Products { get; }
	DbSet<ProductVariant> ProductVariants { get; }
	DbSet<ProductImage> ProductImages { get; }
	DbSet<Domain.Catalog.Attribute> Attributes { get; }
	DbSet<AttributeValue> AttributeValues { get; }
	DbSet<ProductAttribute> ProductAttributes { get; }
	DbSet<ProductRelation> ProductRelations { get; }

	DbSet<CoffeeOrigin> CoffeeOrigins { get; }
	DbSet<CoffeeRegion> CoffeeRegions { get; }
	DbSet<CoffeeProcess> CoffeeProcesses { get; }
	DbSet<RoastProfile> RoastProfiles { get; }
	DbSet<FlavorNote> FlavorNotes { get; }
	DbSet<ProductBatch> ProductBatches { get; }
	DbSet<BatchInventory> BatchInventories { get; }

	DbSet<Warehouse> Warehouses { get; }
	DbSet<Branch> Branches { get; }
	DbSet<InventoryItem> InventoryItems { get; }
	DbSet<InventoryTransaction> InventoryTransactions { get; }
	DbSet<StockReservation> StockReservations { get; }

	DbSet<Customer> Customers { get; }
	DbSet<CustomerAddress> CustomerAddresses { get; }
	DbSet<CustomerNote> CustomerNotes { get; }

	DbSet<Cart> Carts { get; }
	DbSet<CartItem> CartItems { get; }
	DbSet<Order> Orders { get; }
	DbSet<OrderItem> OrderItems { get; }
	DbSet<OrderAddress> OrderAddresses { get; }

	DbSet<Payment> Payments { get; }
	DbSet<PaymentTransaction> PaymentTransactions { get; }

	DbSet<ShippingMethod> ShippingMethods { get; }
	DbSet<Shipment> Shipments { get; }

	DbSet<Coupon> Coupons { get; }
	DbSet<CouponUsage> CouponUsages { get; }
	DbSet<CustomerCoupon> CustomerCoupons { get; }

	DbSet<BlogCategory> BlogCategories { get; }
	DbSet<BlogPost> BlogPosts { get; }
	DbSet<BlogTag> BlogTags { get; }
	DbSet<BlogPostTag> BlogPostTags { get; }

	DbSet<SeoMetadata> SeoMetadata { get; }
	DbSet<Redirect> Redirects { get; }
	DbSet<SeoLandingPage> SeoLandingPages { get; }

	DbSet<ProductReview> ProductReviews { get; }
	DbSet<ReviewImage> ReviewImages { get; }
	DbSet<ReviewReply> ReviewReplies { get; }

	DbSet<Wishlist> Wishlists { get; }
	DbSet<WishlistItem> WishlistItems { get; }

	DbSet<ProductBundle> ProductBundles { get; }
	DbSet<ProductBundleItem> ProductBundleItems { get; }

	DbSet<CustomerPointAccount> CustomerPointAccounts { get; }
	DbSet<PointTransaction> PointTransactions { get; }

	DbSet<Campaign> Campaigns { get; }
	DbSet<Banner> Banners { get; }
	DbSet<BannerPlacement> BannerPlacements { get; }
	DbSet<AbandonedCart> AbandonedCarts { get; }
	DbSet<AbandonedCartNotification> AbandonedCartNotifications { get; }
	DbSet<Referral> Referrals { get; }
	DbSet<ReferralReward> ReferralRewards { get; }

	DbSet<NewsletterSubscriber> NewsletterSubscribers { get; }

	DbSet<CustomerGroup> CustomerGroups { get; }
	DbSet<WholesaleCustomer> WholesaleCustomers { get; }
	DbSet<WholesaleRequest> WholesaleRequests { get; }

	DbSet<PriceList> PriceLists { get; }
	DbSet<PriceListItem> PriceListItems { get; }
	DbSet<QuantityPrice> QuantityPrices { get; }

	DbSet<AccountingSyncLog> AccountingSyncLogs { get; }
	DbSet<AccountingMapping> AccountingMappings { get; }

	// Support (تیکتینگ)
	DbSet<SupportTicket> SupportTickets { get; }
	DbSet<TicketMessage> TicketMessages { get; }

	// Support (چت آنلاین)
	DbSet<SupportChatSession> SupportChatSessions { get; }
	DbSet<ChatMessage> ChatMessages { get; }

	// AI Advisor (مشاور هوشمند قهوه)
	DbSet<AiAdvisorSession> AiAdvisorSessions { get; }
	DbSet<AiAdvisorMessage> AiAdvisorMessages { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
