using Microsoft.Extensions.Logging;
using StdUnit.Tags.ArmEncoder.Protocol;

namespace StdUnit.Tags.ArmEncoder.Channels
{
    internal class ArmEncoderChannel : ArmEncoderChannelBase<byte[]>
    {
        public ArmEncoderChannel(ArmChannelDescriptor descriptor, ILogger<ArmEncoderChannelBase<byte[]>> logger) : base(descriptor, logger)
        {
        }

        protected override async Task<byte[]?> ParseDataAsync(ISerialPortHandle sport, CancellationToken ct)
        {
            var bytes = sport.Read();
            if (bytes is null || bytes.Length == 0)
            {
                return null;
            }

            if (!Crc16.Verify(bytes))
            {
                var crc = Crc16.Compute(bytes.AsSpan()[..^2]);
                throw new InvalidDataException($"接收的字节数组 [{string.Join(", ", bytes)}] Crc16 校验失败。尾部两位应为: [{(byte)(crc & 0xFF)}, {(byte)(crc >> 8)}]");
            }

            return bytes;
        }
    }
}
