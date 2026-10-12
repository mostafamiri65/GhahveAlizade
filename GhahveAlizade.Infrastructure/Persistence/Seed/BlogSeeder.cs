using GhahveAlizade.Domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Persistence.Seed;

/// <summary>
/// دیتای دمو وبلاگ: دسته‌بندی‌ها، تگ‌ها و ۶ مقاله.
/// Idempotent — فقط اگر هیچ پستی نباشد اجرا می‌شود.
/// </summary>
public class BlogSeeder
{
	private readonly AppDbContext _context;
	private readonly ILogger<BlogSeeder> _logger;

	public BlogSeeder(AppDbContext context, ILogger<BlogSeeder> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		if (await _context.BlogPosts.IgnoreQueryFilters().AnyAsync(cancellationToken))
		{
			_logger.LogInformation("Blog posts already exist; skipping blog seed.");
			return;
		}

		var catBrew = new BlogCategory { Name = "دم‌آوری", Slug = "brewing", Description = "روش‌ها و تکنیک‌های دم‌آوری قهوه", DisplayOrder = 1 };
		var catBeans = new BlogCategory { Name = "دانش دانه", Slug = "coffee-beans", Description = "هر آنچه باید درباره دانه قهوه بدانید", DisplayOrder = 2 };
		var catRecipes = new BlogCategory { Name = "دستورها", Slug = "recipes", Description = "دستورهای قهوه و نوشیدنی‌های محبوب", DisplayOrder = 3 };

		var tagV60 = new BlogTag { Name = "V60", Slug = "v60" };
		var tagEspresso = new BlogTag { Name = "اسپرسو", Slug = "espresso" };
		var tagRoast = new BlogTag { Name = "رُست", Slug = "roast" };
		var tagLatte = new BlogTag { Name = "لاته‌آرت", Slug = "latte-art" };

		var posts = new List<BlogPost>
		{
			new()
			{
				Title = "راهنمای کامل دم‌آوری V60 برای مبتدی‌ها",
				Slug = "v60-brewing-guide-beginners",
				Summary = "با این راهنمای گام‌به‌گام، از امروز قهوه V60 حرفه‌ای دم کنید — از نسبت آب تا دمای ایده‌آل.",
				Content = "<h2>چرا V60؟</h2><p>روش V60 یکی از محبوب‌ترین روش‌های پورآور دنیاست؛ ساده، ارزان و با کنترل کامل روی طعم. در این روش آب از روی لایه قهوه آسیاب‌شده عبور می‌کند و نوشیدنی‌ای تمیز و شفاف می‌سازد.</p><h2>وسایل مورد نیاز</h2><p>دریپر V60، فیلتر کاغذی، ترازو، کتری برقی (ترجیحاً گوسبوی)، قهوه تازه رُست‌شده و آب فیلترشده.</p><h2>دستور پیشنهادی</h2><p><strong>نسبت:</strong> ۱ به ۱۶ (مثلاً ۱۵ گرم قهوه و ۲۴۰ گرم آب)<br><strong>آسیاب:</strong> متوسط-درشت مثل نمک دریایی<br><strong>دما:</strong> ۹۲ تا ۹۵ درجه سانتی‌گراد</p><h2>مراحل</h2><p>۱. فیلتر را با آب داغ بشویید تا طعم کاغذ گرفته شود.<br>۲. قهوه را بریزید و ۳۰ گرم آب بریزید؛ ۳۰ تا ۴۵ ثانیه صبر کنید (بلوم).<br>۳. در سه مرحله، آب را به‌صورت دایره‌ای اضافه کنید.<br>۴. کل دم‌آوری حدود ۲:۳۰ تا ۳ دقیقه طول می‌کشد.</p><p>نکته طلایی: همیشه از قهوه تازه رُست‌شده استفاده کنید؛ فرقی نمی‌کند دستورتان چقدر دقیق باشد، قهوه کهنه هرگز طعم خوبی نمی‌دهد.</p>",
				FeaturedImageUrl = "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?auto=format&fit=crop&w=1200&q=70",
				FeaturedImageAlt = "دم‌آوری V60",
				Category = catBrew, AuthorName = "مصطفی علیزاده",
				IsPublished = true, IsFeatured = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-5),
				ViewCount = 860, ReadingTimeMinutes = 6,
				MetaTitle = "راهنمای دم‌آوری V60 | وبلاگ قهوه علیزاده",
				MetaDescription = "آموزش کامل دم‌آوری V60 برای مبتدی‌ها با دستور دقیق.",
				PostTags = { new BlogPostTag { BlogTag = tagV60 } },
			},
			new()
			{
				Title = "تفاوت قهوه عربیکا و روبوستا چیست؟",
				Slug = "arabica-vs-robusta",
				Summary = "دو گونه اصلی قهوه دنیا را بشناسید: از طعم و کافئین تا قیمت و کاربرد.",
				Content = "<h2>عربیکا؛ ملکه قهوه‌ها</h2><p>عربیکا حدود ۶۰٪ تولید جهانی است؛ طعمی پیچیده، اسیدیته خوب و شیرینی طبیعی دارد. کافئین کمتر (۱.۲-۱.۵٪) و حساس‌تر به شرایط آب‌وهوایی است.</p><h2>روبوستا؛ قدرت خالص</h2><p>روبوستا کافئین تقریباً دوبرابر (۲.۲-۲.۷٪) دارد؛ طعمی تلخ‌تر، بدنه سنگین‌تر و کرمای پایدارتر برای اسپرسو. ارزان‌تر و مقاوم‌تر است.</p><h2>کدام را انتخاب کنیم؟</h2><p>برای قهوه فیلتر و تجربه طعمی، عربیکا. برای اسپرسو ایتالیایی و کافئین بیشتر، ترکیب با روبوستا. ما در بلند اسپرسو علیزاده همین تعادل را رعایت کرده‌ایم.</p>",
				FeaturedImageUrl = "https://images.unsplash.com/photo-1447933601403-0c6688de566e?auto=format&fit=crop&w=1200&q=70",
				FeaturedImageAlt = "دانه‌های قهوه",
				Category = catBeans, AuthorName = "مصطفی علیزاده",
				IsPublished = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-12),
				ViewCount = 1240, ReadingTimeMinutes = 5,
				MetaTitle = "تفاوت عربیکا و روبوستا | وبلاگ قهوه علیزاده",
				MetaDescription = "مقایسه کامل عربیکا و روبوستا از نظر طعم، کافئین و قیمت.",
				PostTags = { new BlogPostTag { BlogTag = tagEspresso } },
			},
			new()
			{
				Title = "رُست روشن یا تیره؟ کدام مناسب شماست؟",
				Slug = "light-vs-dark-roast",
				Summary = "در این مقاله تفاوت رُست روشن و تیره را از نظر طعم، کافئین و روش دم‌آوری بررسی می‌کنیم.",
				Content = "<h2>رُست روشن</h2><p>طعم اصلی دانه حفظ می‌شود؛ اسیدیته بالا، عطر میوه‌ای و گل‌دار. مناسب قهوه‌های تک‌خاستگاه با کیفیت بالا و روش‌های فیلتر.</p><h2>رُست تیره</h2><p>طعم رُست غالب می‌شود؛ تلخ، دودی، بدنه سنگین. مناسب اسپرسو، موکاپات و عاشقان طعم قوی.</p><h2>کافئین چه؟</h2><p>برخلاف باور عمومی، رُست روشن کافئین کمی بیشتر دارد (چون جرم دانه در رُست طولانی‌تر بیشتر کم می‌شود).</p><h2>جمع‌بندی</h2><p>اگر طعم‌های میوه‌ای و پیچیده دوست دارید → روشن. اگر شکلات تلخ و بدنه سنگین می‌خواهید → تیره. بین این دو، متوسط تعادل عالی می‌دهد.</p>",
				FeaturedImageUrl = "https://images.unsplash.com/photo-1587734195503-904fca47e0e9?auto=format&fit=crop&w=1200&q=70",
				FeaturedImageAlt = "دانه‌های رُست",
				Category = catBeans, AuthorName = "رُسترخانه علیزاده",
				IsPublished = true, IsFeatured = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-8),
				ViewCount = 980, ReadingTimeMinutes = 7,
				MetaTitle = "رُست روشن یا تیره | وبلاگ قهوه علیزاده",
				MetaDescription = "راهنمای انتخاب بین رُست روشن و تیره بر اساس سلیقه شما.",
				PostTags = { new BlogPostTag { BlogTag = tagRoast } },
			},
			new()
			{
				Title = "دستور لاته در خانه بدون دستگاه اسپرسو",
				Slug = "latte-at-home-no-machine",
				Summary = "با قهوه‌دانه پودری، فرنچ‌پرس و شیر داغ، لاته‌کافه‌ای در خانه بسازید.",
				Content = "<h2>لاته چیست؟</h2><p>لاته = یک‌سوم اسپرسو + دو‌سوم شیر بخارخورده + لایه نازک فوم. نرمی شیر و تلخی ملایم قهوه، ترکیبی دوست‌داشتنی می‌سازد.</p><h2>بدون دستگاه اسپرسو؟</h2><p>۱. با فرنچ‌پرس یا موکاپات، یک شات قهوه غلیظ درست کنید (نسبت ۱ به ۱۰).<br>۲. شیر را در مایکروویو یا روی حرارت تا ۶۰ درجه گرم کنید.<br>۳. با فومر دستی یا شیکر، فوم نرم بسازید.<br>۴. شیر را روی قهوه بریزید و لذت ببرید!</p><h2>نکته حرفه‌ای</h2><p>برای لاته، رُست متوسط تا تیره مثل بلند اسپرسو علیزاده بهترین نتیجه را می‌دهد؛ تلخی ملایم و شیرینی کاراملی با شیر عالی ترکیب می‌شود.</p>",
				FeaturedImageUrl = "https://images.unsplash.com/photo-1561047029-3000c68339ca?auto=format&fit=crop&w=1200&q=70",
				FeaturedImageAlt = "لاته‌آرت",
				Category = catRecipes, AuthorName = "تیم قهوه علیزاده",
				IsPublished = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-2),
				ViewCount = 640, ReadingTimeMinutes = 4,
				MetaTitle = "دستور لاته در خانه | وبلاگ قهوه علیزاده",
				MetaDescription = "آموزش ساخت لاته در خانه بدون دستگاه اسپرسو.",
				PostTags = { new BlogPostTag { BlogTag = tagLatte } },
			},
			new()
			{
				Title = "چطور قهوه تازه را از کهنه تشخیص دهیم؟",
				Slug = "fresh-vs-stale-coffee",
				Summary = "پنج نشانه ساده برای تشخیص تازگی قهوه — قبل از اینکه پولتان را دور بریزید!",
				Content = "<h2>۱. تاریخ رُست، نه تاریخ انقضا</h2><p>همیشه تاریخ رُست را ببینید؛ قهوه تا ۳ ماه پس از رُست در اوج است (اگر بسته‌بندی با شیر یک‌طرفه باشد).</p><h2>۲. عطر</h2><p>قهوه تازه وقتی پاکیت را باز می‌کنید، عطر تند و خوشی دارد. کهنه بوسی خفه و کاغذی می‌دهد.</p><h3>۳. بلوم</h3><p>قهوه تازه هنگام دم‌آوری اولیه، حباب و پف می‌کند (CO2). اگر پف نکرد، کهنه است.</p><h3>۴. روغن سطح دانه</h3><p>در رُست تیره کمی روغن طبیعی است؛ اما روغن زیاد روی قهوه کهنه نشانه اکسیداسیون است.</p><h3>۵. طعم</h3><p>قهوه کهنه طعم خاک‌وهلو و کاغذی دارد؛ شیرینی و عطرش رفته.</p>",
				FeaturedImageUrl = "https://images.unsplash.com/photo-1511920170033-f8396924c348?auto=format&fit=crop&w=1200&q=70",
				FeaturedImageAlt = "قهوه تازه",
				Category = catBeans, AuthorName = "مصطفی علیزاده",
				IsPublished = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-18),
				ViewCount = 520, ReadingTimeMinutes = 5,
				MetaTitle = "تشخیص قهوه تازه از کهنه | وبلاگ قهوه علیزاده",
				MetaDescription = "پنج نشانه تشخیص تازگی قهوه.",
			},
			new()
			{
				Title = "سفر یک دانه قهوه: از مزرعه تا فنجان",
				Slug = "coffee-journey-farm-to-cup",
				Summary = "داستان جذاب سفر دانه قهوه از مزارع استوایی تا فنجان صبحگاهی شما.",
				Content = "<h2>مزرعه</h2><p>قهوه در «کمربند قهوه» بین مدارهای رأس‌السرطان و رأس‌الجدی رشد می‌کند. گیاه قهوه ۳ تا ۴ سال طول می‌کشد تا اولین میوه (گیلاس قهوه) را بدهد.</p><h2>برداشت و پروسس</h2><p>گیلاس‌های قرمز رسیده با دست چیده می‌شوند و بسته به روش پروسس (شسته، طبیعی، هانی) از پوست و موسیلاج جدا می‌شوند.</p><h2>خشک کردن و حمل</h2><p>دانه‌ها روی پاسیو یا در دستگاه‌های خشک‌کن تا رطوبت ۱۰-۱۲٪ خشک می‌شوند و در کیسه‌های برزیل به رستری‌ها می‌روند.</p><h2>رُست و دم‌آوری</h2><p>در رستری، دانه با حرارت دقیق طعمش شکل می‌گیرد و در نهایت، در فنجان شما به مقصد می‌رسد. این سفر ۸ تا ۱۲ ماه طول می‌کشد!</p>",
				FeaturedImageUrl = "https://images.unsplash.com/photo-1509042239860-f550ce710b93?auto=format&fit=crop&w=1200&q=70",
				FeaturedImageAlt = "مزرعه قهوه",
				Category = catBeans, AuthorName = "تیم قهوه علیزاده",
				IsPublished = true,
				PublishedAtUtc = DateTime.UtcNow.AddDays(-25),
				ViewCount = 730, ReadingTimeMinutes = 8,
				MetaTitle = "سفر دانه قهوه | وبلاگ قهوه علیزاده",
				MetaDescription = "داستان سفر دانه قهوه از مزرعه تا فنجان.",
			},
		};

		await _context.BlogPosts.AddRangeAsync(posts, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		_logger.LogInformation("Seeded {Count} demo blog posts.", posts.Count);
	}
}