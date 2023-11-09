using AutoMapper;
using Blazored.Toast;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using PortalForgeX.Application.Data;
using PortalForgeX.Application.Features.Clients;
using PortalForgeX.Application.Features.Internal;
using PortalForgeX.Application.Mapping;
using PortalForgeX.Application.Notifiers;
using PortalForgeX.Application.Tenants;
using PortalForgeX.Communication;
using PortalForgeX.Components;
using PortalForgeX.Domain.Entities.Identity;
using PortalForgeX.Domain.Services;
using PortalForgeX.Extensions;
using PortalForgeX.Facades;
using PortalForgeX.Identity;
using PortalForgeX.Identity.Extensions;
using PortalForgeX.Infrastructure.Configurations;
using PortalForgeX.Infrastructure.Notifiers;
using PortalForgeX.Persistence.EFCore;
using PortalForgeX.Persistence.EFCore.Seeders;
using PortalForgeX.Shared;
using PortalForgeX.Shared.Communication;
using PortalForgeX.Shared.Facades;
using PortalForgeX.Shared.Features.Clients;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

#if DEBUG

// Show detailed errors on Circuit exceptions
builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });

#endif

/**
 * Load the settings as IOptions for usage in the application.
 */
builder.Services.Configure<EmailSettingsOptions>(builder.Configuration.GetSection(EmailSettingsOptions.Section));

/**
 * Serilog configuration and injection into the Loggin Service.
 */
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.File("./Logs/log_.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

/**
 * Portal Context Factory
 * Usage: Razor Component and recommended way of retrieving DbContext for Blazor
 */
var portalConnection = builder.Configuration.GetConnectionString("PortalConnection");
builder.Services.AddDbContextFactory<PortalContext>(options => options.UseSqlServer(portalConnection), lifetime: ServiceLifetime.Scoped);

/**
 * Database Context for Portal objects like Users and Tenants.
 * Usage: Backend projects that uses IPortalContext to retrieve DbContext
 */
builder.Services.AddDbContext<IPortalContext, PortalContext>(options => options.UseSqlServer(portalConnection,
    sqlServerOptionsAction: sqlOptions => sqlOptions.MigrationsAssembly(typeof(PortalContext).Assembly.FullName)));

/**
 * Database Context for Domain Objects.
 * - The DomainContext is used by Tenants for data persistance and each Tenant gets its own database.
 * - Retrieving by IDomainContext or DomainContext will provide a DbContext coupled to Tenant in TenantAccessor (login/claim).
 */
builder.Services.AddSingleton<ITenantConnectionProvider>(new TenantConnectionProvider(builder.Configuration.GetConnectionString("DomainConnection")!));
builder.Services.AddDbContext<IDomainContext, DomainContext>((services, options) =>
{
    var connectionProvider = services.GetRequiredService<ITenantConnectionProvider>();
    var accessor = services.GetRequiredService<TenantAccessor>();

    options.UseSqlServer(connectionProvider.Provide(accessor.CurrentTenant), sqlServerOptionsAction: sqlOptions => sqlOptions.MigrationsAssembly(typeof(DomainContext).Assembly.FullName));
});

/**
 * Add IDomainContextFactory to create DomainContexts based on provided Tenant.
 * Use the IDomainContextFactory if Tenant should be provided runtime.
 */
builder.Services.AddScoped<IDomainContextFactory, DomainContextFactory>();

// Unit of Work for DomainContext and Domain Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

#if DEBUG

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#endif

/**
 * Configure and add (Identity) Authentication and -Cookies
 * for ApplicationUser and ApplicationRole.
 */
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<UserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedEmail = true)
    .AddRoles<ApplicationRole>()
    .AddSignInManager()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<PortalContext>();

/**
 * Database Initializers are used for migrations and seeding on startup.
 * For manual seeders see: PortalForgeX.Persistence.EFCore.Seeders (ISeedService)
 */
builder.Services.AddScoped<PortalContextInitializer>();

/**
 * Override the LoginPath for the Application. (Logout redirects to /Identity/Account/Login)
 */
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

// Remove the inbound role claim.
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

// Configure AutoMapper
builder.Services.AddSingleton(new MapperConfiguration(cfg =>
{
    cfg.AddMaps(Assembly.GetAssembly(typeof(DomainProfiles)));
}).CreateMapper());

// Configure MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ICommand).Assembly);
});

// Add Tenants Services
builder.Services.AddScoped<TenantAccessor>();
builder.Services.AddScoped<ITenantService, TenantService>();

// Reset; because this breaks the application
builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();

// RenderContext communicates to components in which RenderMode the component is running.
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IRenderContext, ServerRenderContext>();

/**
 * The Facade layer is an abstraction layer for communication in the Components to the Application.
 * Server Facades use WebSockets for requests, executing directly on the Server; 
 * Client Facades use Http for requests, excuting against the API Endpoints.
 * 
 * Register here all the Server Facades. Register the Http facades in the PortalForgeX.Client project.
 */
builder.Services.AddScoped<IClientFacade, ClientFacade>();
builder.Services.AddScoped<IClientContactFacade, ClientContactFacade>();
builder.Services.AddScoped<IBusinessLocationFacade, BusinessLocationFacade>();
builder.Services.AddScoped<ICheckoutFacade, CheckoutFacade>();
builder.Services.AddScoped<IPaymentFacade, PaymentFacade>();

// Add (portal) middlewares from infrastructure.
builder.Services.AddInfrastructureMiddleware();

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateClientValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<ClientValidation>();

// Add Toast Service
builder.Services.AddBlazoredToast();

// Add Smtp Notification Service
builder.Services.AddScoped<ISmtpService, SmtpService>();

#if DEBUG

// Seeders
builder.Services.AddScoped<IPaymentSeeder, PaymentSeeder>();
builder.Services.AddScoped<IClientSeeder, ClientSeeder>();
builder.Services.AddScoped<ITenantSeeder, TenantSeeder>();

#endif

// Features Endpoints
builder.Services.AddFeaturesEndpoints();

// ASP.NET Core Feature Management
builder.Services.AddFeatureManagement();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(ApiEndpoint_v1.VERSION, new OpenApiInfo
    {
        Title = "POS/Cana CRM",
        Version = ApiEndpoint_v1.VERSION
    });
});

var app = builder.Build();

/**
 * Initialize (apply pending migrations) and seed the Portal database
 * with the user roles and admin acount.
 */
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<PortalContextInitializer>();
    await initializer.InitialiseAsync();
    await initializer.SeedAsync();
}

/**
 * Enable Request Buffering to read the Request.Body 
 * if ApplicationException needs to be captured in
 * <see cref="PortalForgeX.Infrastructure.Middleware.ExceptionLoggingMiddleware"/>.
 */
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Use Swagger for Endpoint testing.
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "POS/Cana CRM v1"));

    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(PortalForgeX.Client._Imports).Assembly);

// Add application middlewares
app.UseInfrastructureMiddleware();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

// Add application routes
app.MapFeaturesEndpoints();

app.Run();
