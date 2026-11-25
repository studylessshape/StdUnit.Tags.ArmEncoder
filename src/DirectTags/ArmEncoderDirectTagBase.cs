using StdUnit.Tags.ArmEncoder.Channels;
using StdUnit.Tags.ArmEncoder.Protocol;

namespace StdUnit.Tags.ArmEncoder.DirectTags;

/// <summary>
/// 直接测点示例
/// </summary>
internal abstract class ArmEncoderDirectTagBase<T> : Tag<T, ArmEncoderChannel>, IArmEncoderTag
{
    /// <summary>
    /// c'tor
    /// </summary>
    /// <param name="descriptor">当前测点描述符</param>
    /// <param name="thisChannel">当前测点的通道</param>
    /// <param name="container">当前测点的容器</param>
    public ArmEncoderDirectTagBase(TagDescriptor descriptor, ArmEncoderChannel? thisChannel, TagContainer container)
        : base(descriptor, thisChannel, container)
    {
    }

    protected readonly SemaphoreSlim writeCmdSlim = new SemaphoreSlim(1, 1);

    /// <inheritdoc/>
    public override async Task ReadAsync(CancellationToken ct)
    {
        await this._bubbleChannel.EnsureConnectedAsync(force: false, ct);

        if (!_bubbleChannel.TryDequeueInput(out var input))
        {
            return;
        }

        Value = await HandleReadValue(input, ct);
        Timestamp = DateTime.UtcNow;
        this.NotifyTagRead(Value);
    }

    protected abstract Task<T?> HandleReadValue(byte[]? value, CancellationToken ct);

    /// <inheritdoc/>
    public override async Task WriteAsync(CancellationToken ct)
    {
        throw new NotImplementedException("编码器测点不支持直接调用写入方法，请使用 IArmEncoderTag.WriteCommand");
    }

    /// <inheritdoc/>
    public async Task WriteCommand(byte[] command, CancellationToken ct = default)
    {
        await this._bubbleChannel.EnsureConnectedAsync(force: false, ct);

        if (command is null || command.Length == 0)
        {
            return;
        }

        var commandVal = command;
        if (!Crc16.Verify(commandVal) && _bubbleChannel.Descriptor.ToArmChannelDescriptor().Option.AutoWriteCrc16)
        {
            commandVal = Crc16.Append(commandVal);
        }
        await _bubbleChannel.WriteAsync(commandVal, 0, commandVal.Length);
        Timestamp = DateTime.UtcNow;
        this.NotifyTagWritten(commandVal);
    }
}
