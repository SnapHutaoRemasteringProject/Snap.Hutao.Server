using Snap.Hutao.Server.Model.Response;
using Snap.Hutao.Server.Model.Update;

namespace Snap.Hutao.Server.Controller;

[Route("[controller]")]
[ApiController]
public class PatchController : ControllerBase
{
    [HttpGet("hutao")]
    public IActionResult GetPatchInfo()
    {
        return Response<HutaoPackageInformation>.Success("OK", new HutaoPackageInformation()
        {
            Validation = "ca4d77bc06650dfc20d89d9dc88835d3faa801210cbfaae4bee27432d489a724",
            Version = Version.Parse("1.18.1.0"),
            Mirrors =
            [
                new HutaoPackageMirror("https://github.yun-wang.top/https://github.com/SnapHutaoRemasteringProject/Snap.Hutao.Remastered/releases/download/1.18.1.0/Snap.Hutao.Remastered_1.18.1.0_x64.msix",
                "github mirror", "Direct")
            ],
        });
    }
}
