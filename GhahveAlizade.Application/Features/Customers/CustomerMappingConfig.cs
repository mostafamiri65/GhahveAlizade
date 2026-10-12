using GhahveAlizade.Application.Features.Customers.Dtos;
using GhahveAlizade.Domain.Customer;
using GhahveAlizade.Domain.Wholesale;
using Mapster;

namespace GhahveAlizade.Application.Features.Customers;

public class CustomerMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Customer, CustomerDto>()
			.Map(dest => dest.FullName,
				src => $"{src.FirstName} {src.LastName}".Trim())
			.Map(dest => dest.CustomerGroupName,
				src => src.CustomerGroup != null ? src.CustomerGroup.Name : null)
			.Map(dest => dest.Addresses,
				src => src.Addresses.Where(a => !a.IsDeleted).ToList())
			.Map(dest => dest.Notes,
				src => src.Notes.Where(n => !n.IsDeleted)
					.OrderByDescending(n => n.CreatedAtUtc).ToList());

		config.NewConfig<Customer, CustomerListItemDto>()
			.Map(dest => dest.FullName,
				src => $"{src.FirstName} {src.LastName}".Trim())
			.Map(dest => dest.CustomerGroupName,
				src => src.CustomerGroup != null ? src.CustomerGroup.Name : null);

		config.NewConfig<CustomerAddress, CustomerAddressDto>();
		config.NewConfig<CustomerNote, CustomerNoteDto>();

		config.NewConfig<CustomerGroup, CustomerGroupDto>()
			.Map(dest => dest.PriceListName,
				src => src.PriceList != null ? src.PriceList.Name : null)
			.Map(dest => dest.CustomerCount, src => 0);
	}
}