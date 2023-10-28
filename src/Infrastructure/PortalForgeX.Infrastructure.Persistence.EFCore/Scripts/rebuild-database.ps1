Remove-Item -Path ".\Migrations\*"

dotnet ef database drop -f --context PortalContext
dotnet ef migrations add CreateIdentitySchema --context PortalContext
dotnet ef database update --context PortalContext

dotnet ef database drop -f --context DomainContext
dotnet ef migrations add CreateDomainSchema --context DomainContext
dotnet ef database update --context DomainContext