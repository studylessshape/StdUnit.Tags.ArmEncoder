using Microsoft.Extensions.Logging;

namespace StdUnit.Tags.ArmEncoder.Channels;

public class ArmEncoderChannel : ArmEncoderChannelBase<byte[]>
{
    public ArmEncoderChannel(ArmChannelDescriptor descriptor, ILogger<ArmEncoderChannelBase<byte[]>> logger) : base(descriptor, logger)
    {
    }

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
