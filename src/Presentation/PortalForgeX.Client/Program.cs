using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PortalForgeX.Client.Authentication;
using PortalForgeX.Client.Communication;
using PortalForgeX.Client.Extensions;
using PortalForgeX.Client.Facades;
using PortalForgeX.Shared.Communication;
using PortalForgeX.Shared.Facades;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add authentication services
builder.Services.AddAuthorizationCore()
    .AddCascadingAuthenticationState()
    .AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

// Add HttpClient Factory to the Server API
builder.Services.AddHttpClient(IHttpClientFactoryExtensions.SERVER_API_CLIENT_NAME, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Add Render Context for the Client.
builder.Services.AddSingleton<IRenderContext, ClientRenderContext>();

/**
 * The Facade layer is an abstraction layer for communication in the Components to the Application.
 * Server Facades use WebSockets for requests, executing directly on the Server; 
 * Client Facades use Http for requests, excuting against the API Endpoints.
 * 
 * Register here all the Http Facades.
 */
builder.Services.AddScoped<IClientFacade, HttpClientFacade>();
builder.Services.AddScoped<IClientContactFacade, HttpClientContactFacade>();
builder.Services.AddScoped<IBusinessLocationFacade, HttpBusinessLocationFacade>();
builder.Services.AddScoped<ICheckoutFacade, HttpCheckoutFacade>();
builder.Services.AddScoped<IPaymentFacade, HttpPaymentFacade>();

// Add Toast Service
builder.Services.AddBlazoredToast();

// Build and run the application
await builder.Build().RunAsync();
