using AutoMapper;
using PortalForgeX.Application.Data;
using PortalForgeX.Domain.Entities;
using PortalForgeX.Shared.DTOs;
using PortalForgeX.Shared.Features.BusinessLocations;
using PortalForgeX.Shared.Features.Checkouts;
using PortalForgeX.Shared.Features.ClientContacts;
using PortalForgeX.Shared.Features.Clients;
using PortalForgeX.Shared.Features.Payments;

namespace PortalForgeX.Application.Mapping;

public class DomainProfiles : Profile
{
    public DomainProfiles()
    {
        CreateMap<Client, ClientDto>()
            .ReverseMap()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
            .ForMember(x => x.Contacts, opt => opt.Ignore())
            .ForMember(x => x.Payments, opt => opt.Ignore())
            .ForMember(x => x.Locations, opt => opt.Ignore());

        CreateMap<ClientContact, ClientContactDto>()
            .ReverseMap()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
            .ForMember(x => x.Client, opt => opt.Ignore());

        CreateMap<BusinessLocation, BusinessLocationDto>()
            .ReverseMap()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
            .ForMember(x => x.Client, opt => opt.Ignore())
            .ForMember(x => x.Checkouts, opt => opt.Ignore())
            .ForMember(x => x.Payments, opt => opt.Ignore());

        CreateMap<Checkout, CheckoutDto>().ReverseMap()
            .ReverseMap()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.LastModificationTime, opt => opt.Ignore());

        CreateMap<Payment, PaymentDto>().ReverseMap()
            .ReverseMap()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
            .ForMember(x => x.Client, opt => opt.Ignore())
            .ForMember(x => x.BusinessLocation, opt => opt.Ignore());

        // Paging Models
        CreateMap(typeof(EntityPage<>), typeof(PagedList<>))
            .ConvertUsing(typeof(EntityPageConverter<,>));
    }
}
