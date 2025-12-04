using System.Threading;

namespace SampleApp.Infrastructure;

public interface ITelemetryApiClient
{
    Task<TelemetrySnapshot> GetLatestTelemetryAsync(CancellationToken cancellationToken = default);
}
