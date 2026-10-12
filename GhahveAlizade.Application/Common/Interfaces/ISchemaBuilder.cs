using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Catalog.Products.Dtos;

namespace GhahveAlizade.Application.Common.Interfaces;

public interface ISchemaBuilder
{
	JsonLdOrganization BuildOrganization();
	JsonLdWebSite BuildWebSite();
	JsonLdProduct BuildProduct(ProductDetailDto product);
	JsonLdBreadcrumbList BuildBreadcrumb(params (string Name, string? Url)[] items);
	JsonLdArticle BuildArticle(string headline, string? description, string? image,
		DateTime published, DateTime? modified, string? author, string url);
	JsonLdItemList BuildItemList(List<(string Name, string Url)> items);
	JsonLdFAQPage BuildFAQ(List<(string Question, string Answer)> items);
	string Serialize(JsonLdBase schema);
}