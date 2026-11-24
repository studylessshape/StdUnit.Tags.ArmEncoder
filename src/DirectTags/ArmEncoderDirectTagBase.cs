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

    public virtual byte[]? CommandValue { get; set; }

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
        await this._bubbleChannel.EnsureConnectedAsync(force: false, ct);

        if (CommandValue is null || CommandValue.Length == 0)
        {
            return;
        }

        var commandVal = CommandValue;
        if (!Crc16.Verify(commandVal) && _bubbleChannel.Descriptor.ToArmChannelDescriptor().Option.AutoWriteCrc16)
        {
            commandVal = Crc16.Append(commandVal);
        }
        await _bubbleChannel.WriteAsync(commandVal, 0, commandVal.Length);
        Timestamp = DateTime.UtcNow;
        this.NotifyTagWritten(commandVal);

        CommandValue = null;
    }
}
