using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace PortalForgeX.Components.Pages.Internal;

public abstract class FormPage<TModel> : PageBase
{
    /// <summary>
    /// The EditContext for the <EditForm></EditForm>.
    /// </summary>
    protected EditContext? FormEditContext { get; set; } = null!;

    /// <summary>
    /// The Model for the Form.
    /// </summary>
    protected TModel Model { get; set; } = default!;

    /// <summary>
    /// Force the initialization of the Model.
    /// </summary>
    protected abstract TModel InitModel { get; }

    /// <summary>
    /// Initialize the FormEditContext with the Model.
    /// Make sure Model is initialized in InitDataSourcesAsync().
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Model is required.</exception>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Model ??= InitModel;
        FormEditContext = new(Model!);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("formPageLoad");
        }
    }
}
