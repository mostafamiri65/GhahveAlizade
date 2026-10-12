using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.SEO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveSeoMetadata;

public class UpsertSeoMetadataCommandHandler
	: IRequestHandler<UpsertSeoMetadataCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;

	public UpsertSeoMetadataCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(UpsertSeoMetadataCommand request, CancellationToken ct)
	{
		SeoMetadata? metadata;

		if (request.Id.HasValue && request.Id > 0)
		{
			metadata = await _context.SeoMetadata
				.FirstOrDefaultAsync(s => s.Id == request.Id.Value && !s.IsDeleted, ct);
			if (metadata is null) return Error.NotFound("Seo.NotFound", "متادیتا یافت نشد.");
		}
		else
		{
			// چک یکتا: EntityType + EntityId + Culture
			var exists = await _context.SeoMetadata.AnyAsync(s =>
				s.EntityType == request.EntityType &&
				s.EntityId == request.EntityId &&
				s.Culture == request.Culture &&
				!s.IsDeleted, ct);

			if (exists)
				return Error.Conflict("Seo.Exists", "برای این موجودیت در این زبان، متادیتا وجود دارد.");

			metadata = new SeoMetadata();
			_context.SeoMetadata.Add(metadata);
		}

		metadata.EntityType = request.EntityType;
		metadata.EntityId = request.EntityId;
		metadata.Culture = request.Culture;
		metadata.MetaTitle = request.MetaTitle?.Trim();
		metadata.MetaDescription = request.MetaDescription?.Trim();
		metadata.MetaKeywords = request.MetaKeywords?.Trim();
		metadata.CanonicalUrl = request.CanonicalUrl?.Trim();
		metadata.OgTitle = request.OgTitle?.Trim();
		metadata.OgDescription = request.OgDescription?.Trim();
		metadata.OgImage = request.OgImage?.Trim();
		metadata.OgType = request.OgType?.Trim();
		metadata.TwitterCard = request.TwitterCard?.Trim();
		metadata.TwitterTitle = request.TwitterTitle?.Trim();
		metadata.TwitterDescription = request.TwitterDescription?.Trim();
		metadata.TwitterImage = request.TwitterImage?.Trim();
		metadata.Robots = request.Robots;
		metadata.JsonLdSchema = request.JsonLdSchema;
		metadata.FocusKeyword = request.FocusKeyword?.Trim();
		metadata.SeoScore = request.SeoScore;

		await _context.SaveChangesAsync(ct);
		return metadata.Id;
	}
}
