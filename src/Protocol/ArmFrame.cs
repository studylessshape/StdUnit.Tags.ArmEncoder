using System.Buffers.Binary;

namespace StdUnit.Tags.ArmEncoder.Protocol;

/// <summary>
/// 助力臂帧的组包与通用解析辅助。
/// </summary>
public static class ArmFrame
{
    /// <summary>
    /// 构建请求帧：Header + Address + 功能码 + [数据] + CRC16(L) + CRC16(H)。
    /// </summary>
    /// <param name="address">设备地址（普通命令 0-7，可编程地址命令用 0xFF）。</param>
    /// <param name="command">功能码。</param>
    /// <param name="payload">可选数据段。</param>
    public static byte[] BuildRequest(byte address, ArmCommand command, ReadOnlySpan<byte> payload = default)
    {
        var dataLength = 3 + payload.Length;
        var data = new byte[dataLength];
        data[0] = ArmProtocolConstants.Header;
        data[1] = address;
        data[2] = (byte)command;
        payload.CopyTo(data.AsSpan(3));
        return Crc16.Append(data);
    }

    /// <summary>
    /// 校验响应帧的包头与 CRC16，不校验功能码（由具体解析方法校验）。
    /// </summary>
    /// <exception cref="InvalidDataException">帧长度不足、包头错误或 CRC 校验失败。</exception>
    public static void Validate(ReadOnlySpan<byte> frame, int expectedLength)
    {
        if (frame.Length != expectedLength)
        {
            throw new InvalidDataException(
                $"响应帧长度错误：期望 {expectedLength} 字节，实际 {frame.Length} 字节。");
        }

        if (frame[0] != ArmProtocolConstants.Header)
        {
            throw new InvalidDataException($"响应帧包头错误：期望 0x{ArmProtocolConstants.Header:X2}，实际 0x{frame[0]:X2}。");
        }

        if (!Crc16.Verify(frame))
        {
            throw new InvalidDataException("响应帧 CRC16 校验失败。");
        }
    }

    /// <summary>读取小端序 uint16。</summary>
    public static ushort ReadUInt16LittleEndian(ReadOnlySpan<byte> frame, int offset) =>
        BinaryPrimitives.ReadUInt16LittleEndian(frame.Slice(offset, 2));

    /// <summary>读取小端序 int16。</summary>
    public static short ReadInt16LittleEndian(ReadOnlySpan<byte> frame, int offset) =>
        BinaryPrimitives.ReadInt16LittleEndian(frame.Slice(offset, 2));

    /// <summary>读取大端序 int64（位移数据按协议约定为大端）。</summary>
    public static long ReadInt64BigEndian(ReadOnlySpan<byte> frame, int offset) =>
        BinaryPrimitives.ReadInt64BigEndian(frame.Slice(offset, 8));

    /// <summary>
    /// 获取指定命令的响应帧总长度（字节）。
    /// </summary>
    public static int GetResponseLength(ArmCommand command) => command switch
    {
        ArmCommand.GetEncoderData => 18,
        ArmCommand.GetAbsoluteAngle => 8,
        ArmCommand.GetEncoderSpeed => 7,
        ArmCommand.GetDisplacement => 14,
        ArmCommand.GetSoftwareVersion => 7,
        ArmCommand.InitZeroAngle => 8,
        ArmCommand.InitZeroDisplacement => 13,
        ArmCommand.InitProgrammableAddress => 6,
        ArmCommand.SetAddress => 6,
        ArmCommand.ReadAddress => 6,
        _ => throw new ArgumentOutOfRangeException(nameof(command), command, "未知的命令码。"),
    };
}
