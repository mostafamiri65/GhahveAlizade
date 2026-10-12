using FluentValidation;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Commands.CreateBrand;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    private readonly IAppDbContext _context;

    public CreateBrandCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("نام برند الزامی است.")
            .MaximumLength(300);

        RuleFor(x => x.Description)
            .MaximumLength(2000);

        RuleFor(x => x.WebsiteUrl)
            .MaximumLength(500)
            .Must(BeValidUrl).WithMessage("آدرس وب‌سایت معتبر نیست.")
            .When(x => !string.IsNullOrWhiteSpace(x.WebsiteUrl));

        RuleFor(x => x.LogoUrl)
            .MaximumLength(1000);

        RuleFor(x => x.CountryOfOrigin)
            .MaximumLength(100);

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.MetaTitle).MaximumLength(200);
        RuleFor(x => x.MetaDescription).MaximumLength(500);
        RuleFor(x => x.MetaKeywords).MaximumLength(500);

        RuleFor(x => x)
            .MustAsync(BeUniqueSlugAsync).WithMessage("این نامک قبلاً استفاده شده است.")
            .WithName("Slug");
    }

    private async Task<bool> BeUniqueSlugAsync(CreateBrandCommand cmd, CancellationToken ct)
    {
        var slug = SlugHelper.GenerateSlug(cmd.Slug ?? cmd.Name);
        return !await _context.Brands.AnyAsync(b => b.Slug == slug && !b.IsDeleted, ct);
    }

    private static bool BeValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out var result)
            && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}