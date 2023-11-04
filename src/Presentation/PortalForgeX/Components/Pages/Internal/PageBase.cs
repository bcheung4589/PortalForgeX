using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PortalForgeX.Components.Pages.Internal;

public abstract class PageBase : ComponentBase
{
    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = null!;

    [Inject]
    protected IToastService ToastService { get; set; } = null!;

    [Inject]
    protected NavigationManager Navigation { get; set; } = null!;

    /// <summary>
    /// Indicates if a task is running.
    /// </summary>
    protected bool IsLoading { get; set; }

    /// <summary>
    /// Load the local DataSources. This method will be called OnInitializedAsync.
    /// </summary>
    /// <returns></returns>
    protected virtual Task InitDataSourcesAsync() => Task.CompletedTask;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("pageLoad");
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await TryRunAsync(InitDataSourcesAsync);
    }

    /// <summary>
    /// Encapsulate the loadDataCallback inside a try-catch with error handling. 
    /// This method will also handle the IsLoading property.
    /// </summary>
    /// <param name="loadDataCallback"></param>
    /// <returns></returns>
    protected async Task<bool> TryRunAsync(Func<Task> loadDataCallback)
    {
        if (IsLoading) return false;
        IsLoading = true;

        try
        {
            await loadDataCallback();
            return true;
        }
        catch (Exception)
        {
            ToastService.ShowError("Something went wrong!");
            return false;
        }
        finally
        {
            IsLoading = false;
        }
    }
}
