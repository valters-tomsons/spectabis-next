using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using ServiceClient.Helpers;
using Microsoft.ApplicationInsights.Channel;
using ServiceClient.Enums;
using System.Collections.Generic;

namespace ServiceClient.Services
{
    public class Telemetry : Interfaces.ITelemetry
    {
        private TelemetryClient _client;
        private bool isEnabled;

        public void InitializeTelemetry()
        {
            if(_client == null)
            {
                _client = CreateClient();
            }

            AttachExceptionHandlers();
            isEnabled = true;
        }

        public void TrackFailedClientOperation(ClientOperation operation, IDictionary<string, string> props)
        {
            if(isEnabled)
            {
                Console.WriteLine("[Telemetry] Tracking failed client operation");
                var op = new EventTelemetry(operation.ToString());

                foreach(var prop in props)
                {
                    op.Properties.TryAdd(prop.Key, prop.Value);
                }
            }
        }

        private void TrackException(Exception e)
        {
            if(e != null && isEnabled)
            {
                var ex = new ExceptionTelemetry(e);
                _client.TrackException(ex);
                Flush();
            }
        }

        private void Flush()
        {
            if(isEnabled)
            {
                Console.WriteLine("[Telemetry] Flusing telemetry data from memory");
                _client.Flush();
            }
        }

        private void AttachExceptionHandlers()
        {
            Console.WriteLine("[Telemetry] Attaching exception telemetry handlers");
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if(isEnabled)
            {
                TrackException(e.Exception);
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if(isEnabled)
            {
                var ex = e.ExceptionObject as Exception;
                TrackException(ex);
            }
        }

        private TelemetryClient CreateClient()
        {
            Console.WriteLine("[Telemetry] Creating telemetry client");
            var instrumentationKey = ServiceCredentialsHelper.TelemetryKey;

            var config = new TelemetryConfiguration
            {
                InstrumentationKey = instrumentationKey,
                TelemetryChannel = new InMemoryChannel()
                {
                    DeveloperMode = Debugger.IsAttached
                }
            };

            var client = new TelemetryClient(config);

            client.Context.Device.OperatingSystem = Environment.OSVersion.ToString();

            return client;
        }
    }
}