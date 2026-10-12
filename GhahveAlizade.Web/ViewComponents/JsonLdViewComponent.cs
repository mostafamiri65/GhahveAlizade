using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.ViewComponents;

public class JsonLdViewComponent : ViewComponent
{
	private readonly ISchemaBuilder _schemaBuilder;

	public JsonLdViewComponent(ISchemaBuilder schemaBuilder)
		=> _schemaBuilder = schemaBuilder;

	public IViewComponentResult Invoke(JsonLdBase schema)
	{
		var json = _schemaBuilder.Serialize(schema);
		return View(model: json);
	}
}