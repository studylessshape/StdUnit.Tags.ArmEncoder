using R3;
using StdUnit.Tags;
using StdUnit.Tags.ArmEncoder.DirectTags;
using StdUnit.Tags.R3;
using System.Text.RegularExpressions;

namespace ArmEncoder.Sample.Tags.Logicets
{
    public class ArmLogicet : LogicetBase
    {
        private readonly ITag commandTag;
        private readonly ITag dataTag;
        private readonly IArmEncoderTag? armTag;

        public ArmLogicet(ITagsProject project, string deviceNo, ILogger<ArmLogicet> logger) : base(project)
        {
            var entry = project.Tags.SelectGrp(TagsPathDefines.EntryName);
            commandTag = entry.SelectTag($"{TagsPathDefines.ArmCommand_DevicePath}/{deviceNo}");
            dataTag = entry.SelectTag($"{TagsPathDefines.ArmReceive_DevicePath}/{deviceNo}");
            if (dataTag is IArmEncoderTag armTag)
            {
                this.armTag = armTag;
            }

            commandTag.Watch()
                .Subscribe((e) =>
                {
                    if (e.NewValue is string str && !string.IsNullOrEmpty(str))
                    {
                        logger.LogInformation("[{source}] 发送命令: {command}", deviceNo, str);
                    }
                });
            dataTag.Watch()
                .Subscribe((e) =>
                {
                    if (e.NewValue is string str)
                    {
                        logger.LogInformation("[{source}] 接收数据: {command}", deviceNo, str);
                    }

                    if (e.NewValue is byte[] bytes)
                    {
                        logger.LogInformation("[{source}] 接收数据: [{command}]", deviceNo, string.Join(", ", bytes));
                    }
                });
        }

        public override int Order => 1;

        public override bool MatchEntry(ITagGrp entry) => entry.TagName() == TagsPathDefines.EntryName;

        public override async Task ProcessAsync(ITagGrp entry, ITagChannel? thisChannel, CancellationToken ct = default)
        {
            if (armTag is not null && commandTag.Value is string commandVal && !string.IsNullOrEmpty(commandVal))
            {
                string hex = Regex.Replace(commandVal, "[^0-9A-Fa-f]", "");
                byte[] bytes = Convert.FromHexString(hex);
                armTag.CommandValue = bytes;
                commandTag.Value = "";

                await dataTag.WriteAsync(ct);
            }
        }
    }
}
