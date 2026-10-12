using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Catalog.Products.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductFormData;

public class GetProductFormDataQueryHandler
	: IRequestHandler<GetProductFormDataQuery, ErrorOr<ProductFormDataDto>>
{
	private readonly IAppDbContext _context;

	public GetProductFormDataQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<ProductFormDataDto>> Handle(
		GetProductFormDataQuery request,
		CancellationToken cancellationToken)
	{
		var dto = new ProductFormDataDto
		{
			Categories = await _context.Categories
				.Where(c => !c.IsDeleted && c.IsActive)
				.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name)
				.Select(c => new LookupItem
				{
					Id = c.Id,
					Name = c.Name,
					Extra = c.Parent != null ? c.Parent.Name : null
				})
				.ToListAsync(cancellationToken),

			Brands = await _context.Brands
				.Where(b => !b.IsDeleted && b.IsActive)
				.OrderBy(b => b.DisplayOrder).ThenBy(b => b.Name)
				.Select(b => new LookupItem { Id = b.Id, Name = b.Name })
				.ToListAsync(cancellationToken),

			CoffeeOrigins = await _context.CoffeeOrigins
				.Where(o => !o.IsDeleted && o.IsActive)
				.OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name)
				.Select(o => new LookupItem
				{
					Id = o.Id,
					Name = o.Name,
					Extra = o.CountryCode
				})
				.ToListAsync(cancellationToken),

			CoffeeProcesses = await _context.CoffeeProcesses
				.Where(p => !p.IsDeleted && p.IsActive)
				.OrderBy(p => p.DisplayOrder).ThenBy(p => p.Name)
				.Select(p => new LookupItem { Id = p.Id, Name = p.Name })
				.ToListAsync(cancellationToken),

			RoastProfiles = await _context.RoastProfiles
				.Where(r => !r.IsDeleted && r.IsActive)
				.OrderBy(r => r.DisplayOrder).ThenBy(r => r.Name)
				.Select(r => new LookupItem
				{
					Id = r.Id,
					Name = r.Name,
					Extra = r.ColorCode
				})
				.ToListAsync(cancellationToken),

			FlavorNotes = await _context.FlavorNotes
				.Where(f => !f.IsDeleted && f.IsActive)
				.OrderBy(f => f.Category).ThenBy(f => f.DisplayOrder)
				.Select(f => new LookupItem
				{
					Id = f.Id,
					Name = f.Name,
					Extra = f.Category
				})
				.ToListAsync(cancellationToken),

			Attributes = await _context.Attributes
				.Where(a => !a.IsDeleted)
				.OrderBy(a => a.DisplayOrder).ThenBy(a => a.Name)
				.Select(a => new LookupItem
				{
					Id = a.Id,
					Name = a.Name,
					Extra = a.Unit
				})
				.ToListAsync(cancellationToken)
		};

		return dto;
	}
}