using Microsoft.AspNetCore.Components.Forms;

namespace PortalForgeX.Client.Components.FormInputs;

public class BootstrapFieldCssClassProvider : FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();
        if (editContext.IsModified(fieldIdentifier))
        {
            return isValid ? "modified is-valid" : "modified is-invalid";
        }

        return isValid ? string.Empty : "is-invalid";
    }
}
