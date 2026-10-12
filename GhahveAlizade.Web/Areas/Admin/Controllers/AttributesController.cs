using GhahveAlizade.Application.Features.Catalog.Attributes.Commands.CreateAttribute;
using GhahveAlizade.Application.Features.Catalog.Attributes.Commands.DeleteAttribute;
using GhahveAlizade.Application.Features.Catalog.Attributes.Commands.UpdateAttribute;
using GhahveAlizade.Application.Features.Catalog.Attributes.Queries.GetAttributeById;
using GhahveAlizade.Application.Features.Catalog.Attributes.Queries.GetAttributesList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class AttributesController : AdminBaseController
{
    [HasPermission(Permissions.Attributes.View)]
    public async Task<IActionResult> Index(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        Domain.Enums.AttributeType? type = null,
        bool? isFilterable = null,
        bool? isVariant = null,
        string? sortBy = "displayOrder",
        string? sortDir = "asc",
        CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = "مدیریت ویژگی‌ها";
        ViewData["PageIcon"] = "bi-tags";

        var query = new GetAttributesListQuery(
            pageNumber, pageSize, search, type, isFilterable, isVariant, sortBy, sortDir);

        var result = await Mediator.Send(query, cancellationToken);
        if (result.IsError) return Problem(result.Errors);

        ViewBag.Search = search;
        ViewBag.Type = type;
        ViewBag.IsFilterable = isFilterable;
        ViewBag.IsVariant = isVariant;
        ViewBag.SortBy = sortBy;

        return View(result.Value);
    }

    // ================ ایجاد ================

    [HasPermission(Permissions.Attributes.Create)]
    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "افزودن ویژگی";
        ViewData["PageIcon"] = "bi-plus-circle";

        return View(new AttributeFormViewModel
        {
            IsFilterable = true,
            Values = new List<AttributeValueViewModel> { new() }
        });
    }

    [HasPermission(Permissions.Attributes.Create)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        AttributeFormViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var command = new CreateAttributeCommand(
            Name: model.Name,
            Slug: model.Slug,
            Type: model.Type,
            Unit: model.Unit,
            IsFilterable: model.IsFilterable,
            IsRequired: model.IsRequired,
            IsVariant: model.IsVariant,
            DisplayOrder: model.DisplayOrder,
            Values: model.Values
                .Where(v => !string.IsNullOrWhiteSpace(v.Value))
                .Select(v => new AttributeValueInput(
                    null,
                    v.Value!,
                    v.DisplayValue,
                    v.ColorCode,
                    v.DisplayOrder))
                .ToList());

        var result = await Mediator.Send(command, cancellationToken);
        if (result.IsError)
        {
            AddErrorsToModelState(result.Errors);
            return View(model);
        }

        TempData["SuccessMessage"] = "ویژگی با موفقیت ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }

    // ================ ویرایش ================

    [HasPermission(Permissions.Attributes.Edit)]
    [HttpGet]
    public async Task<IActionResult> Edit(long id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "ویرایش ویژگی";
        ViewData["PageIcon"] = "bi-pencil";

        var result = await Mediator.Send(new GetAttributeByIdQuery(id), cancellationToken);
        if (result.IsError)
        {
            TempData["ErrorMessage"] = result.FirstError.Description;
            return RedirectToAction(nameof(Index));
        }

        var a = result.Value;
        var model = new AttributeFormViewModel
        {
            Id = a.Id,
            Name = a.Name,
            Slug = a.Slug,
            Type = a.Type,
            Unit = a.Unit,
            IsFilterable = a.IsFilterable,
            IsRequired = a.IsRequired,
            IsVariant = a.IsVariant,
            DisplayOrder = a.DisplayOrder,
            Values = a.Values.Select(v => new AttributeValueViewModel
            {
                Id = v.Id,
                Value = v.Value,
                DisplayValue = v.DisplayValue,
                ColorCode = v.ColorCode,
                DisplayOrder = v.DisplayOrder
            }).ToList()
        };

        if (model.Values.Count == 0)
            model.Values.Add(new AttributeValueViewModel());

        return View(model);
    }

    [HasPermission(Permissions.Attributes.Edit)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        long id,
        AttributeFormViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var command = new UpdateAttributeCommand(
            Id: id,
            Name: model.Name,
            Slug: model.Slug,
            Type: model.Type,
            Unit: model.Unit,
            IsFilterable: model.IsFilterable,
            IsRequired: model.IsRequired,
            IsVariant: model.IsVariant,
            DisplayOrder: model.DisplayOrder,
            Values: model.Values
                .Where(v => !string.IsNullOrWhiteSpace(v.Value))
                .Select(v => new AttributeValueInput(
                    v.Id > 0 ? v.Id : null,
                    v.Value!,
                    v.DisplayValue,
                    v.ColorCode,
                    v.DisplayOrder))
                .ToList());

        var result = await Mediator.Send(command, cancellationToken);
        if (result.IsError)
        {
            AddErrorsToModelState(result.Errors);
            model.Id = id;
            return View(model);
        }

        TempData["SuccessMessage"] = "ویژگی با موفقیت ویرایش شد.";
        return RedirectToAction(nameof(Index));
    }

    // ================ حذف ================

    [HasPermission(Permissions.Attributes.Delete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteAttributeCommand(id), cancellationToken);

        if (result.IsError)
            TempData["ErrorMessage"] = result.FirstError.Description;
        else
            TempData["SuccessMessage"] = "ویژگی با موفقیت حذف شد.";

        return RedirectToAction(nameof(Index));
    }
}