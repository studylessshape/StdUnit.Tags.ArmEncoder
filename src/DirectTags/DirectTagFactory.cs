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
        var addr = descriptor.RawAddress;
        // 你可以在这里实现你自己的地址转换逻辑，将用户传入原始地址转换为测点的标准化地址
        // descriptor.NormalizedAddress = transform(addr);


        var tagname = descriptor.TagName;
        var tagkind = descriptor.TagKind;
        // 在这里实现你自己的直接测点构造逻辑，判断依据可以是上面tagKind等标准属性，也可以是任何自定义属性
        var tag = tagkind switch
        {
            "your-tag-kind" => new ArmEncoderDirectTag(descriptor, channel, _container),
            _ => throw new TagsProjectConfigurationException(
                $"ArmEncoder 未预料到的测点种类={tagkind}",
                $"Tag({tagname})")
        };
        return tag;
    }

}