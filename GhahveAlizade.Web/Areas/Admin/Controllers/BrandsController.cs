using GhahveAlizade.Application.Features.Catalog.Brands.Commands.CreateBrand;
using GhahveAlizade.Application.Features.Catalog.Brands.Commands.DeleteBrand;
using GhahveAlizade.Application.Features.Catalog.Brands.Commands.ToggleBrandActive;
using GhahveAlizade.Application.Features.Catalog.Brands.Commands.UpdateBrand;
using GhahveAlizade.Application.Features.Catalog.Brands.Queries.GetBrandById;
using GhahveAlizade.Application.Features.Catalog.Brands.Queries.GetBrandsList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Brands;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class BrandsController : AdminBaseController
{
    // ================ لیست ================

    [HasPermission(Permissions.Brands.View)]
    public async Task<IActionResult> Index(
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        bool? isActive = null,
        bool? isFeatured = null,
        string? sortBy = "displayOrder",
        string? sortDir = "asc",
        CancellationToken cancellationToken = default)
    {
        ViewData["Title"] = "مدیریت برندها";
        ViewData["PageIcon"] = "bi-award";

        var query = new GetBrandsListQuery(
            pageNumber, pageSize, search, isActive, isFeatured, sortBy, sortDir);

        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsError)
            return Problem(result.Errors);

        ViewBag.Search = search;
        ViewBag.IsActive = isActive;
        ViewBag.IsFeatured = isFeatured;
        ViewBag.SortBy = sortBy;

        return View(result.Value);
    }

    // ================ ایجاد ================

    [HasPermission(Permissions.Brands.Create)]
    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "افزودن برند";
        ViewData["PageIcon"] = "bi-plus-circle";

        return View(new BrandFormViewModel { IsActive = true });
    }

    [HasPermission(Permissions.Brands.Create)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        BrandFormViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var command = new CreateBrandCommand(
            Name: model.Name,
            Slug: model.Slug,
            Description: model.Description,
            LogoUrl: model.LogoUrl,
            WebsiteUrl: model.WebsiteUrl,
            CountryOfOrigin: model.CountryOfOrigin,
            DisplayOrder: model.DisplayOrder,
            IsActive: model.IsActive,
            IsFeatured: model.IsFeatured,
            MetaTitle: model.MetaTitle,
            MetaDescription: model.MetaDescription,
            MetaKeywords: model.MetaKeywords,
            CanonicalUrl: model.CanonicalUrl,
            OgImage: model.OgImage);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsError)
        {
            AddErrorsToModelState(result.Errors);
            return View(model);
        }

        TempData["SuccessMessage"] = "برند با موفقیت ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }

    // ================ ویرایش ================

    [HasPermission(Permissions.Brands.Edit)]
    [HttpGet]
    public async Task<IActionResult> Edit(long id, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "ویرایش برند";
        ViewData["PageIcon"] = "bi-pencil";

        var result = await Mediator.Send(new GetBrandByIdQuery(id), cancellationToken);

        if (result.IsError)
        {
            TempData["ErrorMessage"] = result.FirstError.Description;
            return RedirectToAction(nameof(Index));
        }

        var b = result.Value;
        var model = new BrandFormViewModel
        {
            Id = b.Id,
            Name = b.Name,
            Slug = b.Slug,
            Description = b.Description,
            LogoUrl = b.LogoUrl,
            WebsiteUrl = b.WebsiteUrl,
            CountryOfOrigin = b.CountryOfOrigin,
            DisplayOrder = b.DisplayOrder,
            IsActive = b.IsActive,
            IsFeatured = b.IsFeatured,
            MetaTitle = b.MetaTitle,
            MetaDescription = b.MetaDescription,
            MetaKeywords = b.MetaKeywords,
            CanonicalUrl = b.CanonicalUrl,
            OgImage = b.OgImage
        };

        return View(model);
    }

    [HasPermission(Permissions.Brands.Edit)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        long id,
        BrandFormViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var command = new UpdateBrandCommand(
            Id: id,
            Name: model.Name,
            Slug: model.Slug,
            Description: model.Description,
            LogoUrl: model.LogoUrl,
            WebsiteUrl: model.WebsiteUrl,
            CountryOfOrigin: model.CountryOfOrigin,
            DisplayOrder: model.DisplayOrder,
            IsActive: model.IsActive,
            IsFeatured: model.IsFeatured,
            MetaTitle: model.MetaTitle,
            MetaDescription: model.MetaDescription,
            MetaKeywords: model.MetaKeywords,
            CanonicalUrl: model.CanonicalUrl,
            OgImage: model.OgImage);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsError)
        {
            AddErrorsToModelState(result.Errors);
            model.Id = id;
            return View(model);
        }

        TempData["SuccessMessage"] = "برند با موفقیت ویرایش شد.";
        return RedirectToAction(nameof(Index));
    }

    // ================ حذف ================

    [HasPermission(Permissions.Brands.Delete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteBrandCommand(id), cancellationToken);

        if (result.IsError)
            TempData["ErrorMessage"] = result.FirstError.Description;
        else
            TempData["SuccessMessage"] = "برند با موفقیت حذف شد.";

        return RedirectToAction(nameof(Index));
    }

    // ================ Toggle Active ================

    [HasPermission(Permissions.Brands.Edit)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(long id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ToggleBrandActiveCommand(id), cancellationToken);

        if (result.IsError)
            return Json(new { success = false, message = result.FirstError.Description });

        return Json(new
        {
            success = true,
            isActive = result.Value,
            message = result.Value ? "فعال شد" : "غیرفعال شد"
        });
    }
}