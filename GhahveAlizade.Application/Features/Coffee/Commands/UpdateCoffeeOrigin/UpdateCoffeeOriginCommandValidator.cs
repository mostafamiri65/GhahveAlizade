using FluentValidation;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.UpdateCoffeeOrigin;

public class UpdateCoffeeOriginCommandValidator : AbstractValidator<UpdateCoffeeOriginCommand>
{
    private readonly IAppDbContext _context;

    public UpdateCoffeeOriginCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.CountryCode).MaximumLength(5);

        RuleFor(x => x)
            .MustAsync(BeUniqueSlugAsync).WithMessage("این نامک قبلاً استفاده شده است.")
            .WithName("Slug");

        RuleForEach(x => x.Regions).ChildRules(r =>
        {
            r.RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            r.RuleFor(x => x.Description).MaximumLength(2000);
        });
    }

    private async Task<bool> BeUniqueSlugAsync(UpdateCoffeeOriginCommand cmd, CancellationToken ct)
    {
        var slug = SlugHelper.GenerateSlug(cmd.Slug ?? cmd.Name);
        return !await _context.CoffeeOrigins.AnyAsync(
            o => o.Slug == slug && o.Id != cmd.Id && !o.IsDeleted, ct);
    }
}