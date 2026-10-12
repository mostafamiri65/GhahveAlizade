using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Content;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogPost;

public class UpdateBlogPostCommandHandler
	: IRequestHandler<UpdateBlogPostCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public UpdateBlogPostCommandHandler(IAppDbContext context, IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<Success>> Handle(UpdateBlogPostCommand request, CancellationToken ct)
	{
		var post = await _context.BlogPosts
			.Include(p => p.PostTags)
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, ct);

		if (post is null) return Error.NotFound("BlogPost.NotFound", "مقاله یافت نشد.");

		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Title);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.BlogPosts.AnyAsync(p => p.Slug == slug && p.Id != post.Id && !p.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		var wasPublished = post.IsPublished;

		post.Title = request.Title.Trim();
		post.Slug = slug;
		post.Summary = request.Summary?.Trim();
		post.Content = request.Content;
		post.FeaturedImageUrl = request.FeaturedImageUrl?.Trim();
		post.FeaturedImageAlt = request.FeaturedImageAlt?.Trim();
		post.CategoryId = request.CategoryId;
		post.AuthorName = request.AuthorName?.Trim();
		post.IsPublished = request.IsPublished;
		post.IsFeatured = request.IsFeatured;
		post.AllowComments = request.AllowComments;
		post.MetaTitle = request.MetaTitle?.Trim();
		post.MetaDescription = request.MetaDescription?.Trim();
		post.MetaKeywords = request.MetaKeywords?.Trim();
		post.CanonicalUrl = request.CanonicalUrl?.Trim();
		post.OgImage = request.OgImage?.Trim();
		post.ReadingTimeMinutes = EstimateReadingTime(request.Content);

		if (request.IsPublished && !wasPublished)
			post.PublishedAtUtc = _dateTime.UtcNow;

		// Tagها: حذف قبلی‌ها، اضافه‌ی جدیدها
		foreach (var pt in post.PostTags.ToList())
			_context.BlogPostTags.Remove(pt);

		foreach (var tagId in request.TagIds.Distinct())
		{
			post.PostTags.Add(new BlogPostTag
			{
				BlogPostId = post.Id,
				BlogTagId = tagId
			});
		}

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}

	private static int EstimateReadingTime(string content)
	{
		var text = System.Text.RegularExpressions.Regex.Replace(content, "<[^>]*>", "");
		var words = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
		return Math.Max(1, (int)Math.Ceiling(words / 200.0));
	}
}
