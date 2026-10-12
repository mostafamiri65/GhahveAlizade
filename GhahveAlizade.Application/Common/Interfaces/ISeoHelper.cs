namespace GhahveAlizade.Application.Common.Interfaces;

public interface ISeoHelper
{
	string GetCanonicalUrl(string path);
	string GetAbsoluteUrl(string? relativeUrl);
	string GetOgImageUrl(string? imageUrl);
}