// Copyright (c) Snap.HutaoRemasteringProject. All rights reserved.
// Licensed under the MIT license.

using Quartz;
using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Entity.Unlocker;
using Snap.Hutao.Server.Model.Fufu;
using Snap.Hutao.Server.Option;

namespace Snap.Hutao.Server.Job;

public class FufuUidBanRefreshJob : IJob
{
    private readonly AppOptions appOptions;
    private readonly AppDbContext appDbContext;
    private readonly HttpClient httpClient;

    public FufuUidBanRefreshJob(AppOptions options, HttpClient httpClient, AppDbContext appDbContext)
    {
        this.appOptions = options;
        this.appDbContext = appDbContext;
        this.httpClient = httpClient;

        httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task Execute(IJobExecutionContext context)
    {
        FufuOptions fufuOptions = appOptions.Fufu;

        HttpRequestMessage request = new(HttpMethod.Get, fufuOptions.UidBanQueryUri);
        HttpResponseMessage response = await httpClient.SendAsync(request);

        FufuUidBanQuery result = JsonSerializer.Deserialize<FufuUidBanQuery>(await response.Content.ReadAsStringAsync())!;

        foreach (FufuUidBanItem ban in result.BannedList)
        {
            if (!appDbContext.UnlockerBanned.Any(b => b.Uid == ban.Uid.ToString()))
            {
                appDbContext.UnlockerBanned.Add(new()
                {
                    Uid = ban.Uid.ToString(),
                    Reason = ban.Reason,
                });
            }
        }

        foreach (UnlockerBanned ban in appDbContext.UnlockerBanned)
        {
            if (!result.BannedList.Any(b => b.Uid.ToString() == ban.Uid))
            {
                request = new(HttpMethod.Post, fufuOptions.UidBanAddUri);
                request.Headers.Add("x-api-token", $"{fufuOptions.ApiToken}");
                request.Content = new StringContent(JsonSerializer.Serialize(new FufuUidBanItem()
                {
                    Uid = long.Parse(ban.Uid),
                    Reason = ban.Reason,
                }), Encoding.UTF8, "application/json");

                await httpClient.SendAsync(request);
            }
        }

        appDbContext.SaveChanges();
    }
}
