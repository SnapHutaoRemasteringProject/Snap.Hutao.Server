// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Quartz;
using Sentry;
using Snap.Hutao.Server.Service.Metadata;

namespace Snap.Hutao.Server.Job;

public sealed class MetadataRefreshJob : IJob
{
    private readonly MetadataRefreshService metadataRefreshService;

    public MetadataRefreshJob(MetadataRefreshService metadataRefreshService)
    {
        this.metadataRefreshService = metadataRefreshService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        SentryId id = SentrySdk.CaptureCheckIn("MetadataRefreshJob", CheckInStatus.InProgress);
        try
        {
            await metadataRefreshService.RefreshGachaEventsAsync(context.CancellationToken).ConfigureAwait(false);
            await metadataRefreshService.RefreshKnownItemsAsync(context.CancellationToken).ConfigureAwait(false);

            SentrySdk.CaptureCheckIn("MetadataRefreshJob", CheckInStatus.Ok, id);
        }
        catch
        {
            SentrySdk.CaptureCheckIn("MetadataRefreshJob", CheckInStatus.Error, id);
            throw;
        }
    }
}
