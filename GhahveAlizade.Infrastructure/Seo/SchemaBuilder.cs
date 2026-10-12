using System.Text.Json;
using System.Text.Json.Serialization;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Catalog.Products.Dtos;
using Microsoft.Extensions.Options;

namespace GhahveAlizade.Infrastructure.Seo;

public class SchemaBuilder : ISchemaBuilder
{
	private readonly SeoOptions _options;
	private readonly ISeoHelper _seoHelper;

	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		WriteIndented = false
	};

	public SchemaBuilder(IOptions<SeoOptions> options, ISeoHelper seoHelper)
	{
		_options = options.Value;
		_seoHelper = seoHelper;
	}

	public JsonLdOrganization BuildOrganization() => new()
	{
		Name = _options.OrganizationLegalName,
		Url = _options.SiteBaseUrl,
		Logo = _seoHelper.GetAbsoluteUrl(_options.OrganizationLogo),
		Email = _options.OrganizationEmail,
		Telephone = _options.OrganizationPhone,
		Address = new JsonLdPostalAddress
		{
			StreetAddress = _options.OrganizationAddress,
			AddressLocality = _options.OrganizationCity,
			AddressCountry = _options.OrganizationCountry
		},
		SameAs = _options.SocialProfiles
	};

	public JsonLdWebSite BuildWebSite() => new()
	{
		Name = _options.SiteName,
		Url = _options.SiteBaseUrl,
		PotentialAction = new JsonLdSearchAction
		{
			Target = new JsonLdSearchTarget
			{
				UrlTemplate = $"{_options.SiteBaseUrl}/search?q={{search_term_string}}"
			}
		},
		Publisher = BuildOrganization()
	};

	public JsonLdProduct BuildProduct(ProductDetailDto product)
	{
		var url = _seoHelper.GetCanonicalUrl($"product/{product.Slug}");

		var price = product.HasVariants
			? product.Variants.Where(v => v.IsActive).Select(v => v.Price).DefaultIfEmpty(0).Min()
			: product.BasePrice ?? 0;

		var inStock = product.Variants.Any(v => v.IsActive) || (product.BasePrice.HasValue && product.BasePrice > 0);

		var schema = new JsonLdProduct
		{
			Name = product.Name,
			Description = product.ShortDescription ?? product.Description,
			Url = url,
			Sku = product.Sku,
			Image = product.Images.Select(i => _seoHelper.GetAbsoluteUrl(i.ImageUrl)).ToList(),
			Category = product.CategoryName,
			Offers = new JsonLdOffer
			{
				Url = url,
				Price = price.ToString("0"),
				PriceCurrency = "IRR",
				Availability = inStock
					? "https://schema.org/InStock"
					: "https://schema.org/OutOfStock",
				Seller = BuildOrganization()
			},
			Brand = !string.IsNullOrEmpty(product.BrandName)
				? new JsonLdBrand { Name = product.BrandName }
				: null
		};

		if (product.ReviewCount > 0 && product.AverageRating > 0)
		{
			schema.AggregateRating = new JsonLdAggregateRating
			{
				RatingValue = product.AverageRating.ToString("0.0"),
				ReviewCount = product.ReviewCount.ToString()
			};
		}

		return schema;
	}

	public JsonLdBreadcrumbList BuildBreadcrumb(params (string Name, string? Url)[] items)
	{
		var list = new JsonLdBreadcrumbList();
		var position = 1;

		foreach (var (name, url) in items)
		{
			list.ItemListElement.Add(new JsonLdBreadcrumbItem
			{
				Position = position++,
				Name = name,
				Item = string.IsNullOrEmpty(url) ? null : _seoHelper.GetAbsoluteUrl(url)
			});
		}

		return list;
	}

	public JsonLdArticle BuildArticle(
		string headline,
		string? description,
		string? image,
		DateTime published,
		DateTime? modified,
		string? author,
		string url)
	{
		var schema = new JsonLdArticle
		{
			Headline = headline,
			Description = description,
			DatePublished = published.ToString("o"),
			DateModified = modified?.ToString("o"),
			MainEntityOfPage = _seoHelper.GetAbsoluteUrl(url),
			Publisher = BuildOrganization()
		};

		if (!string.IsNullOrEmpty(image))
			schema.Image.Add(_seoHelper.GetAbsoluteUrl(image));

		if (!string.IsNullOrEmpty(author))
			schema.Author = new JsonLdPerson { Name = author };

		return schema;
	}

	public JsonLdItemList BuildItemList(List<(string Name, string Url)> items)
	{
		var schema = new JsonLdItemList
		{
			NumberOfItems = items.Count
		};

		var position = 1;
		foreach (var (name, url) in items)
		{
			schema.ItemListElement.Add(new JsonLdItemListElement
			{
				Position = position++,
				Name = name,
				Url = _seoHelper.GetAbsoluteUrl(url)
			});
		}

		return schema;
	}

	public JsonLdFAQPage BuildFAQ(List<(string Question, string Answer)> items)
	{
		var schema = new JsonLdFAQPage();

		foreach (var (question, answer) in items)
		{
			schema.MainEntity.Add(new JsonLdQuestion
			{
				Name = question,
				AcceptedAnswer = new JsonLdAnswer { Text = answer }
			});
		}

		return schema;
	}

	public string Serialize(JsonLdBase schema)
		=> JsonSerializer.Serialize(schema, JsonOptions);
}