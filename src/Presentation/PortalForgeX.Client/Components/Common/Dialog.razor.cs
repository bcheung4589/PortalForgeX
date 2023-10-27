using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PortalForgeX.Client.Components.Common;

/// <summary>
/// HTML based <dialog> for modals. Uses partial Bootstrap for UI styling.
/// </summary>
public partial class Dialog : ComponentBase, IAsyncDisposable
{
    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = null!;

    /// <summary>
    /// Object Reference to the JS script.
    /// </summary>
    protected IJSObjectReference JsReference = null!;

    /// <summary>
    /// Element reference for the Modal HTML element.
    /// </summary>
    protected ElementReference Modal;

    /// <summary>
    /// Indicator if the Dialog is open.
    /// </summary>
    public bool IsOpen { get; protected set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            JsReference = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
            "./Components/Common/Dialog.razor.js");
        }

        if (ShowDialogOnRender)
        {
            await OpenDialog();
        }
    }

    /// <summary>
    /// Open the Dialog.
    /// </summary>
    /// <returns></returns>
    public async Task OpenDialog()
    {
        IsOpen = true;
        StateHasChanged();

        await JsReference.InvokeVoidAsync("openDialog", Modal);
    }

    /// <summary>
    /// Close the Dialog.
    /// </summary>
    /// <returns></returns>
    public async Task CloseDialog()
    {
        IsOpen = false;

        await JsReference.InvokeVoidAsync("closeDialog", Modal);
    }

    public async ValueTask DisposeAsync()
    {
        if (JsReference is not null)
        {
            await JsReference.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}
