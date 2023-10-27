using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace PortalForgeX.Application.Exceptions;

[Serializable()]
public class ApplicationException : Exception
{
    public ApplicationException(string message) : base(message) { }

    public ApplicationException(string message, Exception ex) : base(message, ex) { }

    public ApplicationException(string message, Exception ex, object debugData) : base(message, ex)
        => Data.Add("_DebugData", debugData is not null ? JsonConvert.SerializeObject(debugData) : "Null");
}
