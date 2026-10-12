using FluentValidation;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.CreateCoffeeOrigin;

public class CreateCoffeeOriginCommandValidator : AbstractValidator<CreateCoffeeOriginCommand>
{
    private readonly IAppDbContext _context;

    public CreateCoffeeOriginCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.CountryCode).MaximumLength(5);
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0);

        RuleFor(x => x)
            .MustAsync(BeUniqueSlugAsync).WithMessage("این نامک قبلاً استفاده شده است.")
            .WithName("Slug");

        RuleForEach(x => x.Regions).ChildRules(r =>
        {
            r.RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            r.RuleFor(x => x.Description).MaximumLength(2000);
            r.RuleFor(x => x.Altitude).MaximumLength(100);
        });
    }

    private async Task<bool> BeUniqueSlugAsync(CreateCoffeeOriginCommand cmd, CancellationToken ct)
    {
        var slug = SlugHelper.GenerateSlug(cmd.Slug ?? cmd.Name);
        return !await _context.CoffeeOrigins.AnyAsync(o => o.Slug == slug && !o.IsDeleted, ct);
    }
}