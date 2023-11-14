using PortalForgeX.Endpoints.Internal;
using PortalForgeX.Infrastructure.Middleware;
using System.Reflection;

namespace PortalForgeX.Extensions;

public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Helper method to register classes into the IServiceCollection based on its Type.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="typeToAdd"></param>
    /// <param name="lifetime"></param>
    /// <param name="assembly"></param>
    private static void RegisterType(IServiceCollection services, Type typeToAdd, ServiceLifetime? lifetime = null, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetAssembly(typeToAdd);

        foreach (var definedType in assembly!.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeToAdd)))
        {
            services.Add(new ServiceDescriptor(typeToAdd, definedType, lifetime ?? ServiceLifetime.Scoped));
        }
    }

    /// <summary>
    /// Register the IFeatureEndpoints into the ServiceCollection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly">If null, the assembly of IFeatureEndpoint will be used.</param>
    public static void AddFeaturesEndpoints(this IServiceCollection services, Assembly? assembly = null)
        => RegisterType(services, typeof(IFeatureEndpoint), ServiceLifetime.Transient, assembly: assembly);

    /// <summary>
    /// Map the endpoints by calling the IFeatureEndpoints AddRoutes().
    /// </summary>
    /// <param name="app"></param>
    public static void MapFeaturesEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.ServiceProvider.GetServices<IFeatureEndpoint>();
        if (endpoints is null)
        {
            return;
        }

        foreach (var endpoint in endpoints)
        {
            endpoint.AddRoutes(app);
        }
    }

    /// <summary>
    /// Register the IMiddlewares into the ServiceCollection.
    /// </summary>
    /// <param name="services"></param>
    public static void AddInfrastructureMiddleware(this IServiceCollection services) => services
        .AddScoped<ExceptionLoggingMiddleware>()
        .AddScoped<TenantAccessMiddleware>();

    /// <summary>
    /// Use the IMiddlewares.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseInfrastructureMiddleware(this IApplicationBuilder builder) => builder
        .UseMiddleware<ExceptionLoggingMiddleware>()
        .UseMiddleware<TenantAccessMiddleware>();
}
