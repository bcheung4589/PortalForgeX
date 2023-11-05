using AutoMapper;
using PortalForgeX.Domain.Entities.Identity;
using PortalForgeX.Domain.Entities.Tenants;
using PortalForgeX.Shared.Features.Tenants;

namespace PortalForgeX.Application.Mapping;

public class PortalProfiles : Profile
{
    public PortalProfiles()
    {
        // Tenant

        CreateMap<Tenant, TenantViewModel>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.ExternalId, opt => opt.MapFrom(x => x.ExternalId))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(x => x.InternalName, opt => opt.MapFrom(x => x.InternalName))
            .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status.ToString()))
            .ForMember(x => x.Host, opt => opt.MapFrom(x => x.Host))
            .ForMember(x => x.IsActive, opt => opt.MapFrom(x => x.IsActive))
            .ForMember(x => x.Remarks, opt => opt.MapFrom(x => x.Remarks))
            .ForMember(x => x.ManagerId, opt => opt.MapFrom(x => x.ManagerId))
            .ForMember(x => x.ManagerName, opt => opt.MapFrom(x => x.Manager!.UserName));

        CreateMap<Tenant, TenantFormModel>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.ExternalId, opt => opt.MapFrom(x => x.ExternalId))
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(x => x.InternalName, opt => opt.MapFrom(x => x.InternalName))
            .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status.ToString()))
            .ForMember(x => x.Host, opt => opt.MapFrom(x => x.Host))
            .ForMember(x => x.IsActive, opt => opt.MapFrom(x => x.IsActive))
            .ForMember(x => x.Remarks, opt => opt.MapFrom(x => x.Remarks))
            .ForMember(x => x.CreationTime, opt => opt.MapFrom(x => x.CreationTime))
            .ForMember(x => x.LastModificationTime, opt => opt.MapFrom(x => x.LastModificationTime))
            .ForMember(x => x.ManagerId, opt => opt.MapFrom(x => x.ManagerId))
            .ForMember(x => x.ManagerName, opt => opt.MapFrom(x => x.Manager!.UserName))
            .ReverseMap()
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.Manager, opt => opt.Ignore());

        CreateMap<TenantSettings, TenantFormModel>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
            .ForMember(x => x.LogoUrl, opt => opt.MapFrom(x => x.LogoUrl))
            .ForMember(x => x.Brand, opt => opt.MapFrom(x => x.Brand))
            .ForMember(x => x.PrimaryColor, opt => opt.MapFrom(x => x.PrimaryColor))
            .ForMember(x => x.SecondaryColor, opt => opt.MapFrom(x => x.SecondaryColor))
            .ForMember(x => x.IsPublicRegisterEnabled, opt => opt.MapFrom(x => x.IsPublicRegisterEnabled))
            .ForMember(x => x.AdditionalData, opt => opt.MapFrom(x => x.AdditionalData))
            .ReverseMap()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.LastModificationTime, opt => opt.Ignore());

        // Tenant User Profile

        CreateMap<ApplicationUser, TenantUserViewModel>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.FirstName))
            .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.LastName))
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.UserName))
            .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email))
            .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(x => x.PhoneNumber))
            .ForMember(x => x.IsActive, opt => opt.MapFrom(x => x.IsActive))
            .ForMember(x => x.Remarks, opt => opt.MapFrom(x => x.Remarks))
            .ForMember(x => x.CreationTime, opt => opt.MapFrom(x => x.CreationTime))
            .ForMember(x => x.LastModificationTime, opt => opt.MapFrom(x => x.LastModificationTime))
            .ForMember(x => x.LastLoggedInTime, opt => opt.MapFrom(x => x.LastLoggedInTime))
            .ForMember(x => x.LockoutEnd, opt => opt.MapFrom(x => x.LockoutEnd))
            .ForMember(x => x.AccessFailedCount, opt => opt.MapFrom(x => x.AccessFailedCount))
            .ForMember(x => x.LockoutEnabled, opt => opt.MapFrom(x => x.LockoutEnabled))
            .ForMember(x => x.TenantId, opt => opt.MapFrom(x => x.TenantId));

        CreateMap<TenantUserProfile, TenantUserViewModel>()
            .ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title));

        CreateMap<ApplicationUser, TenantUserFormModel>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.FirstName))
            .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.LastName))
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.UserName))
            .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email))
            .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(x => x.PhoneNumber))
            .ForMember(x => x.IsActive, opt => opt.MapFrom(x => x.IsActive))
            .ForMember(x => x.Remarks, opt => opt.MapFrom(x => x.Remarks))
            .ForMember(x => x.CreationTime, opt => opt.MapFrom(x => x.CreationTime))
            .ForMember(x => x.LastModificationTime, opt => opt.MapFrom(x => x.LastModificationTime))
            .ForMember(x => x.LastLoggedInTime, opt => opt.MapFrom(x => x.LastLoggedInTime))
            .ForMember(x => x.LockoutEnd, opt => opt.MapFrom(x => x.LockoutEnd))
            .ForMember(x => x.AccessFailedCount, opt => opt.MapFrom(x => x.AccessFailedCount))
            .ForMember(x => x.LockoutEnabled, opt => opt.MapFrom(x => x.LockoutEnabled))
            .ForMember(x => x.TenantId, opt => opt.MapFrom(x => x.TenantId))
            .ForMember(x => x.TenantName, opt => opt.MapFrom(x => x.Tenant!.Name))
            .ReverseMap()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.LastModificationTime, opt => opt.Ignore());

        CreateMap<TenantUserProfile, TenantUserFormModel>()
            .ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title))
            .ReverseMap()
            .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.Id));
    }
}
