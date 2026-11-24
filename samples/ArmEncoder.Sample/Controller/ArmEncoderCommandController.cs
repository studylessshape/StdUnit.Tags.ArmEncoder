using ArmEncoder.Sample.Tags;
using Microsoft.AspNetCore.Mvc;
using StdUnit.Tags;
using StdUnit.Tags.ArmEncoder.Protocol;

namespace ArmEncoder.Sample.Controller
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ArmEncoderCommandController : ControllerBase
    {
        private readonly ITagsProjectCtrl tagCtrl;

        public ArmEncoderCommandController(ITagsProjectCtrl tagCtrl)
        {
            this.tagCtrl = tagCtrl;
        }

        [HttpGet]
        public async Task<IActionResult> SetCommandAsync(string deviceNo, ArmCommand command, byte? address)
        {
            if (!tagCtrl.TryGetRunning(out var proj, out var _))
            {
                return Problem(detail: "Tag项目未运行");
            }

            var res = proj.WriteIntent(TagsPathDefines.EntryName, (entry, ct) =>
            {
                var commandTag = entry.SelectTag($"{TagsPathDefines.ArmCommand_DevicePath}/{deviceNo}");
                address = command switch
                {
                    > ArmCommand.InitZeroDisplacement => 0xFF,
                    _ => address ?? 0
                };
                var hex = ArmFrame.BuildRequest(address.Value, command, []);
                commandTag.Value = Convert.ToHexString(hex); ;
                return ValueTask.CompletedTask;
            }, out var task);

            if (!res)
            {
                return Problem(detail: "未能获取到写入意图");
            }

            await task;

            return Ok($"写入命令{command}成功");
        }
    }
}
