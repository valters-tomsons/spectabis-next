using System.Collections.Generic;
using ServiceClient.Enums;

namespace ServiceClient.Interfaces
{
    public interface ITelemetry
    {
        void Flush();
        void InitializeTelemetry();
        void TrackFailedClientOperation(ClientOperation operation, IDictionary<string, string> props);
    }
}