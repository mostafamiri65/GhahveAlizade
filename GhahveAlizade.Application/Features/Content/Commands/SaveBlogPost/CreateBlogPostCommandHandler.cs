using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Content;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogPost;

public class CreateBlogPostCommandHandler
	: IRequestHandler<CreateBlogPostCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public CreateBlogPostCommandHandler(IAppDbContext context, IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<long>> Handle(CreateBlogPostCommand request, CancellationToken ct)
	{
		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Title);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.BlogPosts.AnyAsync(p => p.Slug == slug && !p.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		var post = new BlogPost
		{
			Title = request.Title.Trim(),
			Slug = slug,
			Summary = request.Summary?.Trim(),
			Content = request.Content,
			FeaturedImageUrl = request.FeaturedImageUrl?.Trim(),
			FeaturedImageAlt = request.FeaturedImageAlt?.Trim(),
			CategoryId = request.CategoryId,
			AuthorName = request.AuthorName?.Trim(),
			IsPublished = request.IsPublished,
			PublishedAtUtc = request.IsPublished ? _dateTime.UtcNow : null,
			IsFeatured = request.IsFeatured,
			AllowComments = request.AllowComments,
			MetaTitle = request.MetaTitle?.Trim(),
			MetaDescription = request.MetaDescription?.Trim(),
			MetaKeywords = request.MetaKeywords?.Trim(),
			CanonicalUrl = request.CanonicalUrl?.Trim(),
			OgImage = request.OgImage?.Trim(),
			ReadingTimeMinutes = EstimateReadingTime(request.Content)
		};

		// Tagها
		if (request.TagIds.Any())
		{
			foreach (var tagId in request.TagIds)
			{
				post.PostTags.Add(new BlogPostTag { BlogTagId = tagId });
			}
		}

		_context.BlogPosts.Add(post);
		await _context.SaveChangesAsync(ct);
		return post.Id;
	}

	private static int EstimateReadingTime(string content)
	{
		var text = System.Text.RegularExpressions.Regex.Replace(content, "<[^>]*>", "");
		var words = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
		return Math.Max(1, (int)Math.Ceiling(words / 200.0)); // ~200 کلمه در دقیقه
	}
}
