using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ServiceClient.Interfaces;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using ServiceClient.Helpers;

namespace ServiceClient.Services
{
    public class Telemetry : ITelemetry
    {
        private TelemetryClient _client;
        private bool isEnabled;

        public Telemetry()
        {

        }

        public void EnableTelemetry()
        {
            if(_client == null)
            {
                _client = CreateClient();
            }

            Console.WriteLine("[Telemetry] Telemetry Enabled");

            AttachExceptionHandlers();

            isEnabled = true;
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
            var instrumentationKey = ServiceCredentialsHelper.TelemetryInstrumentationKey;

            var config = new TelemetryConfiguration
            {
                InstrumentationKey = instrumentationKey,
                TelemetryChannel = new Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.ServerTelemetryChannel
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