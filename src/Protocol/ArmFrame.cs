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
    /// <exception cref="ArgumentOutOfRangeException">命令码未知。</exception>
    public static int GetResponseLength(ArmCommand command) =>
        TryGetResponseLength((byte)command, out var length)
            ? length
            : throw new ArgumentOutOfRangeException(nameof(command), command, "未知的命令码。");

    /// <summary>
    /// 尝试获取指定功能码的响应帧总长度（字节）。
    /// 用于从字节流中组帧：功能码未知时返回 <c>false</c>，调用方应继续向后寻找包头。
    /// </summary>
    /// <param name="command">功能码原始字节。</param>
    /// <param name="length">响应帧总长度（含 Header、Address、CMD 与 CRC16 两字节）。</param>
    public static bool TryGetResponseLength(byte command, out int length)
    {
        switch ((ArmCommand)command)
        {
            case ArmCommand.GetEncoderData:
                length = 18;
                return true;
            case ArmCommand.GetAbsoluteAngle:
                length = 8;
                return true;
            case ArmCommand.GetEncoderSpeed:
                length = 7;
                return true;
            case ArmCommand.GetDisplacement:
                length = 14;
                return true;
            case ArmCommand.GetSoftwareVersion:
                length = 7;
                return true;
            case ArmCommand.InitZeroAngle:
                length = 8;
                return true;
            case ArmCommand.InitZeroDisplacement:
                length = 13;
                return true;
            case ArmCommand.InitProgrammableAddress:
                length = 6;
                return true;
            case ArmCommand.SetAddress:
                length = 6;
                return true;
            case ArmCommand.ReadAddress:
                length = 6;
                return true;
            default:
                length = 0;
                return false;
        }
    }
}
