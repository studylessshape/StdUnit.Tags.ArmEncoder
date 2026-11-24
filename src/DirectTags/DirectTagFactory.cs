using StdUnit.Tags.ArmEncoder.Channels;

namespace StdUnit.Tags.ArmEncoder.DirectTags;

internal class ArmEncoderDirectTagFactory
{
    public ArmEncoderDirectTagFactory(TagContainer container)
    {
        this._container = container;
    }

    private readonly TagContainer _container;

    public ITag Create(TagDescriptor descriptor, ArmEncoderChannel? channel)
    {
        var tagname = descriptor.TagName;
        var tagkind = descriptor.TagKind;

        if (descriptor.AccessMode != TagAccessMode.RO)
        {
            throw new TagsProjectConfigurationException("ArmEncoder 的测点仅支持只读(RO)模式，要写入命令时，需要赋值IArmEncoderTag.CommandValue，并调用ITag.WriteAsync", $"Tag({tagname})");
        }

        // 在这里实现你自己的直接测点构造逻辑，判断依据可以是上面tagKind等标准属性，也可以是任何自定义属性
        return tagkind switch
        {
            var k when k.Equals("bytes", StringComparison.OrdinalIgnoreCase) => new ArmEncoderBytesDirectTag(descriptor, channel, _container),
            var k when k.Equals("str", StringComparison.OrdinalIgnoreCase) => new ArmEncoderStringDirectTag(descriptor, channel, _container),
            _ => throw new TagsProjectConfigurationException(
                $"ArmEncoder 未预料到的测点种类={tagkind}",
                $"Tag({tagname})")
        };
    }

}