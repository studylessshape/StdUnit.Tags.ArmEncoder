using StdUnit.Tags.ArmEncoder.Channels;

namespace StdUnit.Tags.ArmEncoder.DirectTags;

/// <summary>
/// 直接测点示例
/// </summary>
internal class ArmEncoderStringDirectTag : ArmEncoderDirectTagBase<string>
{
    public ArmEncoderStringDirectTag(TagDescriptor descriptor, ArmEncoderChannel? thisChannel, TagContainer container) : base(descriptor, thisChannel, container)
    {
    }

    protected override async Task<string?> HandleReadValue(byte[]? value, CancellationToken ct)
    {
        if (value is null) return null;

        return Convert.ToHexString(value);
    }
}
