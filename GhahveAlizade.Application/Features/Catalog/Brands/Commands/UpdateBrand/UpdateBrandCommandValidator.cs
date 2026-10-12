using FluentValidation;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Commands.UpdateBrand;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    private readonly IAppDbContext _context;

    public UpdateBrandCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("نام برند الزامی است.")
            .MaximumLength(300);

        RuleFor(x => x.Description).MaximumLength(2000);

        RuleFor(x => x.WebsiteUrl)
            .MaximumLength(500)
            .Must(BeValidUrl).WithMessage("آدرس وب‌سایت معتبر نیست.")
            .When(x => !string.IsNullOrWhiteSpace(x.WebsiteUrl));

        RuleFor(x => x)
            .MustAsync(BeUniqueSlugAsync).WithMessage("این نامک قبلاً استفاده شده است.")
            .WithName("Slug");
    }

    private async Task<bool> BeUniqueSlugAsync(UpdateBrandCommand cmd, CancellationToken ct)
    {
        var slug = SlugHelper.GenerateSlug(cmd.Slug ?? cmd.Name);
        return !await _context.Brands.AnyAsync(
            b => b.Slug == slug && b.Id != cmd.Id && !b.IsDeleted, ct);
    }

    private static bool BeValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out var result)
            && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}