namespace GhahveAlizade.Infrastructure.Identity;

public static class PermissionDisplayNames
{
	public static readonly Dictionary<string, string> Map = new()
	{
		// Users
		["Users.View"] = "مشاهده کاربران",
		["Users.Create"] = "ایجاد کاربر",
		["Users.Edit"] = "ویرایش کاربر",
		["Users.Delete"] = "حذف کاربر",
		["Users.ManageRoles"] = "مدیریت نقش‌های کاربر",
		["Users.ResetPassword"] = "بازنشانی رمز عبور",

		// Roles
		["Roles.View"] = "مشاهده نقش‌ها",
		["Roles.Create"] = "ایجاد نقش",
		["Roles.Edit"] = "ویرایش نقش",
		["Roles.Delete"] = "حذف نقش",
		["Roles.ManagePermissions"] = "مدیریت دسترسی‌های نقش",

		// Products
		["Products.View"] = "مشاهده محصولات",
		["Products.Create"] = "ایجاد محصول",
		["Products.Edit"] = "ویرایش محصول",
		["Products.Delete"] = "حذف محصول",
		["Products.Publish"] = "انتشار محصول",
		["Products.ManageImages"] = "مدیریت تصاویر محصول",
		["Products.ManageVariants"] = "مدیریت واریانت‌ها",
		["Products.ManageAttributes"] = "مدیریت ویژگی‌های محصول",

		// Categories
		["Categories.View"] = "مشاهده دسته‌بندی‌ها",
		["Categories.Create"] = "ایجاد دسته‌بندی",
		["Categories.Edit"] = "ویرایش دسته‌بندی",
		["Categories.Delete"] = "حذف دسته‌بندی",

		// Brands
		["Brands.View"] = "مشاهده برندها",
		["Brands.Create"] = "ایجاد برند",
		["Brands.Edit"] = "ویرایش برند",
		["Brands.Delete"] = "حذف برند",

		// Attributes
		["Attributes.View"] = "مشاهده ویژگی‌ها",
		["Attributes.Create"] = "ایجاد ویژگی",
		["Attributes.Edit"] = "ویرایش ویژگی",
		["Attributes.Delete"] = "حذف ویژگی",

		// Bundles
		["Bundles.View"] = "مشاهده بسته‌ها",
		["Bundles.Create"] = "ایجاد بسته",
		["Bundles.Edit"] = "ویرایش بسته",
		["Bundles.Delete"] = "حذف بسته",

		// Coffee
		["Coffee.ViewOrigins"] = "مشاهده خاستگاه‌ها",
		["Coffee.ManageOrigins"] = "مدیریت خاستگاه‌ها",
		["Coffee.ViewProcesses"] = "مشاهده فرآیندها",
		["Coffee.ManageProcesses"] = "مدیریت فرآیندها",
		["Coffee.ViewRoastProfiles"] = "مشاهده پروفایل‌های رست",
		["Coffee.ManageRoastProfiles"] = "مدیریت پروفایل‌های رست",
		["Coffee.ViewFlavorNotes"] = "مشاهده نُت‌های طعمی",
		["Coffee.ManageFlavorNotes"] = "مدیریت نُت‌های طعمی",
		["Coffee.ManageBatches"] = "مدیریت بچ‌ها",

		// Inventory
		["Inventory.View"] = "مشاهده انبار",
		["Inventory.Adjust"] = "تعدیل موجودی",
		["Inventory.Transfer"] = "انتقال بین انبار",
		["Inventory.ViewTransactions"] = "مشاهده تراکنش‌های انبار",
		["Inventory.ManageWarehouses"] = "مدیریت انبارها",
		["Inventory.ManageBranches"] = "مدیریت شعبه‌ها",

		// Orders
		["Orders.View"] = "مشاهده سفارشات",
		["Orders.ViewDetails"] = "مشاهده جزئیات سفارش",
		["Orders.Edit"] = "ویرایش سفارش",
		["Orders.Cancel"] = "لغو سفارش",
		["Orders.Refund"] = "بازپرداخت سفارش",
		["Orders.ChangeStatus"] = "تغییر وضعیت سفارش",
		["Orders.Print"] = "چاپ سفارش",
		["Orders.Export"] = "خروجی سفارشات",

		// Payments
		["Payments.View"] = "مشاهده پرداخت‌ها",
		["Payments.Verify"] = "تأیید پرداخت",
		["Payments.Refund"] = "بازپرداخت پرداخت",

		// Shipments
		["Shipments.View"] = "مشاهده مرسولات",
		["Shipments.Create"] = "ایجاد مرسوله",
		["Shipments.Edit"] = "ویرایش مرسوله",
		["Shipments.UpdateTracking"] = "به‌روزرسانی کد رهگیری",
		["Shipments.ManageMethods"] = "مدیریت روش‌های ارسال",

		// Customers
		["Customers.View"] = "مشاهده مشتریان",
		["Customers.ViewDetails"] = "مشاهده جزئیات مشتری",
		["Customers.Create"] = "ایجاد مشتری",
		["Customers.Edit"] = "ویرایش مشتری",
		["Customers.Delete"] = "حذف مشتری",
		["Customers.ManageNotes"] = "مدیریت یادداشت‌ها",
		["Customers.ManageGroups"] = "مدیریت گروه‌ها",
		["Customers.Export"] = "خروجی مشتریان",

		// Wholesale
		["Wholesale.View"] = "مشاهده عمده‌فروشی",
		["Wholesale.Approve"] = "تأیید درخواست عمده",
		["Wholesale.Reject"] = "رد درخواست عمده",
		["Wholesale.ManageCustomers"] = "مدیریت مشتریان عمده",
		["Wholesale.ManageRequests"] = "مدیریت درخواست‌های عمده",

		// Blog
		["Blog.ViewPosts"] = "مشاهده پست‌ها",
		["Blog.CreatePost"] = "ایجاد پست",
		["Blog.EditPost"] = "ویرایش پست",
		["Blog.DeletePost"] = "حذف پست",
		["Blog.PublishPost"] = "انتشار پست",
		["Blog.ManageCategories"] = "مدیریت دسته‌بندی‌های وبلاگ",
		["Blog.ManageTags"] = "مدیریت برچسب‌های وبلاگ",

		// Reviews
		["Reviews.View"] = "مشاهده نظرات",
		["Reviews.Approve"] = "تأیید نظر",
		["Reviews.Reject"] = "رد نظر",
		["Reviews.Reply"] = "پاسخ به نظر",
		["Reviews.Delete"] = "حذف نظر",

		// SEO
		["Seo.ViewMetadata"] = "مشاهده متادیتای سئو",
		["Seo.EditMetadata"] = "ویرایش متادیتای سئو",
		["Seo.ManageRedirects"] = "مدیریت ریدایرکت‌ها",
		["Seo.ManageLandingPages"] = "مدیریت صفحات فرود",

		// Marketing
		["Marketing.ViewCampaigns"] = "مشاهده کمپین‌ها",
		["Marketing.ManageCampaigns"] = "مدیریت کمپین‌ها",
		["Marketing.ViewBanners"] = "مشاهده بنرها",
		["Marketing.ManageBanners"] = "مدیریت بنرها",
		["Marketing.ViewAbandonedCarts"] = "مشاهده سبدهای رها شده",
		["Marketing.ManageReferrals"] = "مدیریت معرفی‌ها",

		// Coupons
		["Coupons.View"] = "مشاهده کدهای تخفیف",
		["Coupons.Create"] = "ایجاد کد تخفیف",
		["Coupons.Edit"] = "ویرایش کد تخفیف",
		["Coupons.Delete"] = "حذف کد تخفیف",

		// Newsletter
		["Newsletter.View"] = "مشاهده خبرنامه",
		["Newsletter.Send"] = "ارسال خبرنامه",
		["Newsletter.Export"] = "خروجی خبرنامه",

		// Pricing
		["Pricing.View"] = "مشاهده قیمت‌گذاری",
		["Pricing.ManagePriceLists"] = "مدیریت لیست‌های قیمت",
		["Pricing.ManageQuantityPrices"] = "مدیریت قیمت‌های تعدادی",

		// Loyalty
		["Loyalty.View"] = "مشاهده باشگاه مشتریان",
		["Loyalty.AdjustPoints"] = "تعدیل امتیاز",
		["Loyalty.ManageTiers"] = "مدیریت سطوح",

		// Reports
		["Reports.ViewSales"] = "گزارش فروش",
		["Reports.ViewInventory"] = "گزارش انبار",
		["Reports.ViewCustomers"] = "گزارش مشتریان",
		["Reports.ViewMarketing"] = "گزارش بازاریابی",
		["Reports.Export"] = "خروجی گزارش‌ها",

		// Settings
		["Settings.View"] = "مشاهده تنظیمات",
		["Settings.Edit"] = "ویرایش تنظیمات",
		["Settings.ManagePayment"] = "مدیریت پرداخت",
		["Settings.ManageShipping"] = "مدیریت ارسال",
		["Settings.ManageTax"] = "مدیریت مالیات",
		["Settings.ViewLogs"] = "مشاهده لاگ‌ها",
		["Settings.ManageAccounting"] = "مدیریت حسابداری",
	};

	public static string Get(string permissionName)
		=> Map.TryGetValue(permissionName, out var display) ? display : permissionName;
}