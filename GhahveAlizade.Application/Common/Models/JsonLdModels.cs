using System.Text.Json.Serialization;

namespace GhahveAlizade.Application.Common.Models;

/// <summary>
/// Base Schema.org object
/// </summary>
public abstract class JsonLdBase
{
	[JsonPropertyName("@context")]
	public string Context { get; set; } = "https://schema.org";

	[JsonPropertyName("@type")]
	public abstract string Type { get; }
}

public class JsonLdOrganization : JsonLdBase
{
	public override string Type => "Organization";

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("url")]
	public string Url { get; set; } = string.Empty;

	[JsonPropertyName("logo")]
	public string Logo { get; set; } = string.Empty;

	[JsonPropertyName("email")]
	public string? Email { get; set; }

	[JsonPropertyName("telephone")]
	public string? Telephone { get; set; }

	[JsonPropertyName("address")]
	public JsonLdPostalAddress? Address { get; set; }

	[JsonPropertyName("sameAs")]
	public List<string> SameAs { get; set; } = new();
}

public class JsonLdWebSite : JsonLdBase
{
	public override string Type => "WebSite";

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("url")]
	public string Url { get; set; } = string.Empty;

	[JsonPropertyName("potentialAction")]
	public JsonLdSearchAction? PotentialAction { get; set; }

	[JsonPropertyName("publisher")]
	public JsonLdOrganization? Publisher { get; set; }
}

public class JsonLdSearchAction
{
	[JsonPropertyName("@type")]
	public string Type => "SearchAction";

	[JsonPropertyName("target")]
	public JsonLdSearchTarget Target { get; set; } = new();

	[JsonPropertyName("query-input")]
	public string QueryInput { get; set; } = "required name=search_term_string";
}

public class JsonLdSearchTarget
{
	[JsonPropertyName("@type")]
	public string Type => "EntryPoint";

	[JsonPropertyName("urlTemplate")]
	public string UrlTemplate { get; set; } = string.Empty;
}

public class JsonLdProduct : JsonLdBase
{
	public override string Type => "Product";

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("description")]
	public string? Description { get; set; }

	[JsonPropertyName("image")]
	public List<string> Image { get; set; } = new();

	[JsonPropertyName("sku")]
	public string? Sku { get; set; }

	[JsonPropertyName("brand")]
	public JsonLdBrand? Brand { get; set; }

	[JsonPropertyName("category")]
	public string? Category { get; set; }

	[JsonPropertyName("offers")]
	public JsonLdOffer? Offers { get; set; }

	[JsonPropertyName("aggregateRating")]
	public JsonLdAggregateRating? AggregateRating { get; set; }

	[JsonPropertyName("review")]
	public List<JsonLdReview> Review { get; set; } = new();

	[JsonPropertyName("url")]
	public string Url { get; set; } = string.Empty;
}

public class JsonLdBrand
{
	[JsonPropertyName("@type")]
	public string Type => "Brand";

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;
}

public class JsonLdOffer
{
	[JsonPropertyName("@type")]
	public string Type => "Offer";

	[JsonPropertyName("url")]
	public string Url { get; set; } = string.Empty;

	[JsonPropertyName("priceCurrency")]
	public string PriceCurrency { get; set; } = "IRR";

	[JsonPropertyName("price")]
	public string Price { get; set; } = "0";

	[JsonPropertyName("priceValidUntil")]
	public string? PriceValidUntil { get; set; }

	[JsonPropertyName("availability")]
	public string Availability { get; set; } = "https://schema.org/InStock";

	[JsonPropertyName("itemCondition")]
	public string ItemCondition { get; set; } = "https://schema.org/NewCondition";

	[JsonPropertyName("seller")]
	public JsonLdOrganization? Seller { get; set; }
}

public class JsonLdAggregateRating
{
	[JsonPropertyName("@type")]
	public string Type => "AggregateRating";

	[JsonPropertyName("ratingValue")]
	public string RatingValue { get; set; } = "0";

	[JsonPropertyName("reviewCount")]
	public string ReviewCount { get; set; } = "0";

	[JsonPropertyName("bestRating")]
	public string BestRating { get; set; } = "5";

	[JsonPropertyName("worstRating")]
	public string WorstRating { get; set; } = "1";
}

public class JsonLdReview
{
	[JsonPropertyName("@type")]
	public string Type => "Review";

	[JsonPropertyName("author")]
	public JsonLdPerson Author { get; set; } = new();

	[JsonPropertyName("datePublished")]
	public string DatePublished { get; set; } = string.Empty;

	[JsonPropertyName("reviewBody")]
	public string ReviewBody { get; set; } = string.Empty;

	[JsonPropertyName("reviewRating")]
	public JsonLdRating ReviewRating { get; set; } = new();
}

public class JsonLdPerson
{
	[JsonPropertyName("@type")]
	public string Type => "Person";

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;
}

public class JsonLdRating
{
	[JsonPropertyName("@type")]
	public string Type => "Rating";

	[JsonPropertyName("ratingValue")]
	public string RatingValue { get; set; } = "0";

	[JsonPropertyName("bestRating")]
	public string BestRating { get; set; } = "5";
}

public class JsonLdBreadcrumbList : JsonLdBase
{
	public override string Type => "BreadcrumbList";

	[JsonPropertyName("itemListElement")]
	public List<JsonLdBreadcrumbItem> ItemListElement { get; set; } = new();
}

public class JsonLdBreadcrumbItem
{
	[JsonPropertyName("@type")]
	public string Type => "ListItem";

	[JsonPropertyName("position")]
	public int Position { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("item")]
	public string? Item { get; set; }
}

public class JsonLdArticle : JsonLdBase
{
	public override string Type => "Article";

	[JsonPropertyName("headline")]
	public string Headline { get; set; } = string.Empty;

	[JsonPropertyName("description")]
	public string? Description { get; set; }

	[JsonPropertyName("image")]
	public List<string> Image { get; set; } = new();

	[JsonPropertyName("datePublished")]
	public string DatePublished { get; set; } = string.Empty;

	[JsonPropertyName("dateModified")]
	public string? DateModified { get; set; }

	[JsonPropertyName("author")]
	public JsonLdPerson? Author { get; set; }

	[JsonPropertyName("publisher")]
	public JsonLdOrganization? Publisher { get; set; }

	[JsonPropertyName("mainEntityOfPage")]
	public string? MainEntityOfPage { get; set; }
}

public class JsonLdItemList : JsonLdBase
{
	public override string Type => "ItemList";

	[JsonPropertyName("itemListElement")]
	public List<JsonLdItemListElement> ItemListElement { get; set; } = new();

	[JsonPropertyName("numberOfItems")]
	public int NumberOfItems { get; set; }
}

public class JsonLdItemListElement
{
	[JsonPropertyName("@type")]
	public string Type => "ListItem";

	[JsonPropertyName("position")]
	public int Position { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; } = string.Empty;

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;
}

public class JsonLdPostalAddress
{
	[JsonPropertyName("@type")]
	public string Type => "PostalAddress";

	[JsonPropertyName("streetAddress")]
	public string? StreetAddress { get; set; }

	[JsonPropertyName("addressLocality")]
	public string? AddressLocality { get; set; }

	[JsonPropertyName("addressCountry")]
	public string? AddressCountry { get; set; }
}

public class JsonLdFAQPage : JsonLdBase
{
	public override string Type => "FAQPage";

	[JsonPropertyName("mainEntity")]
	public List<JsonLdQuestion> MainEntity { get; set; } = new();
}

public class JsonLdQuestion
{
	[JsonPropertyName("@type")]
	public string Type => "Question";

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("acceptedAnswer")]
	public JsonLdAnswer AcceptedAnswer { get; set; } = new();
}

public class JsonLdAnswer
{
	[JsonPropertyName("@type")]
	public string Type => "Answer";

	[JsonPropertyName("text")]
	public string Text { get; set; } = string.Empty;
}