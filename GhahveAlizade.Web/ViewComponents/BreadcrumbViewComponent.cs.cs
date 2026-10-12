using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.ViewComponents;

public class BreadcrumbViewComponent : ViewComponent
{
	private readonly ISchemaBuilder _schemaBuilder;

	public BreadcrumbViewComponent(ISchemaBuilder schemaBuilder)
		=> _schemaBuilder = schemaBuilder;

	public IViewComponentResult Invoke(params (string Name, string? Url)[] items)
	{
		var schema = _schemaBuilder.BuildBreadcrumb(items);
		var json = _schemaBuilder.Serialize(schema);

		ViewData["Items"] = items;
		return View(model: json);
	}
}