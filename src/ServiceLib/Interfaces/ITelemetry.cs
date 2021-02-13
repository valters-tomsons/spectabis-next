using System.Collections.Generic;
using ServiceLib.Enums;

namespace ServiceLib.Interfaces
{
    public interface ITelemetry
    {
        void Flush();
        void InitializeTelemetry();
        void TrackFailedClientOperation(ClientOperation operation, IDictionary<string, string> props);
    }
}