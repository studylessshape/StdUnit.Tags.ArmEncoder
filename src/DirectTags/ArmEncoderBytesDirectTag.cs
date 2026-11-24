using StdUnit.Tags.ArmEncoder.Channels;

namespace StdUnit.Tags.ArmEncoder.DirectTags;

/// <summary>
/// 直接测点示例
/// </summary>
internal class ArmEncoderBytesDirectTag : ArmEncoderDirectTagBase<byte[]>
{
    public ArmEncoderBytesDirectTag(TagDescriptor descriptor, ArmEncoderChannel? thisChannel, TagContainer container) : base(descriptor, thisChannel, container)
    {
    }

    protected override Task<byte[]?> HandleReadValue(byte[]? value, CancellationToken ct)
    {
        return Task.FromResult(value);
    }
}
