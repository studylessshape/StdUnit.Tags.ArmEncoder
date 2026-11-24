using Microsoft.Extensions.Logging;

namespace StdUnit.Tags.ArmEncoder.Channels;

/// <summary>
/// ArmEncoder Channel 的默认实现
/// </summary>
public class ArmEncoderChannel : ArmEncoderChannelBase<byte[]>
{
    /// <inheritdoc/>
    public ArmEncoderChannel(ArmChannelDescriptor descriptor, ILogger<ArmEncoderChannelBase<byte[]>> logger) : base(descriptor, logger)
    {
    }

    /// <inheritdoc/>
    protected override Task<byte[]?> ParseDataAsync(ISerialPortHandle sport, CancellationToken ct)
    {
        var bytes = sport.Read();
        if (bytes is null || bytes.Length == 0)
        {
            return Task.FromResult<byte[]?>(null);
        }

        return Task.FromResult<byte[]?>(bytes);
    }
}
