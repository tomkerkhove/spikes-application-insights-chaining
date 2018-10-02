using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace ApplicationInsights.DependencyTracking
{
    class Program
    {
        static void Main(string[] args)
        {
            string instrumentationKey = "<key>";
            TelemetryConfiguration.Active.DisableTelemetry = false;

            var sessionId = Guid.NewGuid().ToString();

            var operationId = SimulateApiService(sessionId, instrumentationKey);
            SimulateContainer(sessionId, operationId, instrumentationKey);

            Console.WriteLine("Done simulating!");
            Console.ReadLine();
        }

        private static string SimulateApiService(string sessionId, string instrumentationKey)
        {
            var operationId = Guid.NewGuid().ToString();
            var telemetryClient = new TelemetryClient { InstrumentationKey = instrumentationKey };
            telemetryClient.Context.Cloud.RoleName = "Orders API v3";
            telemetryClient.Context.Operation.Id = sessionId;
            telemetryClient.Context.Operation.ParentId = operationId;

            TrackDependency("Http", telemetryClient);
            TrackDependency("Azure Service Bus", telemetryClient);

            return operationId;
        }

        private static void SimulateContainer(string sessionId, string operationId, string instrumentationKey)
        {
            var telemetryClient = new TelemetryClient { InstrumentationKey = instrumentationKey };
            telemetryClient.Context.Cloud.RoleName = "Orders Container v3";
            telemetryClient.Context.Operation.Id = sessionId;
            telemetryClient.Context.Operation.ParentId = operationId;

            TrackDependency("Http", telemetryClient);
            TrackDependency("Azure Service Bus", telemetryClient);
        }

        private static void TrackDependency(string dependencyType, TelemetryClient telemetryClient)
        {
            var dependencyTelemetry = new DependencyTelemetry()
            {
                Name = $"{dependencyType}",
                Type = dependencyType
            };

            telemetryClient.TrackDependency(dependencyTelemetry);
        }
    }
}
