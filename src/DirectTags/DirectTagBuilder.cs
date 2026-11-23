using StdUnit.Tags.ArmEncoder.Channels;

namespace StdUnit.Tags.ArmEncoder.DirectTags;

/// <summary>
/// ArmEncoder DirectTag Builder
/// </summary>
public class ArmEncoderDirectTagBuilder : TagBuilderBase
{
    /// <inheritdoc/>
    protected override ITag Fallback(ITagChannel channel)
    {
        ArmEncoderChannel? mbch;
        if (this.Channel is null)
        {
            mbch = null;
        }
        else if (this.Channel is not ArmEncoderChannel)
        {
            throw new TagsProjectConfigurationException(
                $"测点 '{this.Name}' 配置了通道，但不是 {nameof(ArmEncoderChannel)}",
                $"Tag({this.Name})");
        }
        else
        {
            mbch = this.Channel as ArmEncoderChannel;
        }

        var factory = new ArmEncoderDirectTagFactory(this.Parent.IntoTagContainer());
        var tag = factory.Create(this.TagDescriptor, mbch);
        return tag;
    }
}