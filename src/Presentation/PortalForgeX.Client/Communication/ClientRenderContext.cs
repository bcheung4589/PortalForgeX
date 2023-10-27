using PortalForgeX.Shared.Communication;

namespace PortalForgeX.Client.Communication;

/// <inheritdoc/>
public sealed class ClientRenderContext : IRenderContext
{
    /// <inheritdoc/>
    public bool IsClient => true;

    /// <inheritdoc/>
    public bool IsServer => false;

    /// <inheritdoc/>
    public bool IsPrerendering => false;
}
