// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Server.Model.Context;
using Snap.Hutao.Server.Model.Response;
using Snap.Hutao.Server.Model.Update;

namespace Snap.Hutao.Server.Controller;

[Route("[controller]")]
[ApiController]
public class PatchController : ControllerBase
{
    private readonly AppDbContext dbContext;

    public PatchController(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet("hutao")]
    public async Task<IActionResult> GetPatchInfo()
    {
        var packageInfos = await dbContext.HutaoPackageInformations
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        if (!packageInfos.Any())
        {
            return Response<Model.Update.HutaoPackageInformation>.Fail(ReturnCode.InvalidRequestBody, "No package information found");
        }

        var groupedByVersion = packageInfos
            .GroupBy(x => x.Version)
            .OrderByDescending(g => Version.Parse(g.Key))
            .FirstOrDefault();

        if (groupedByVersion == null)
        {
            return Response<Model.Update.HutaoPackageInformation>.Fail(ReturnCode.InvalidRequestBody, "No valid package information found");
        }

        var versionRecords = groupedByVersion.ToList();

        var validation = versionRecords.First().Validation;

        var mirrors = versionRecords.Select(record =>
            new HutaoPackageMirror(record.Url, record.MirrorName, record.MirrorType))
            .ToList();

        var result = new Model.Update.HutaoPackageInformation
        {
            Validation = validation,
            Version = Version.Parse(groupedByVersion.Key),
            Mirrors = mirrors,
        };

        return Response<Model.Update.HutaoPackageInformation>.Success("OK", result);
    }
}
