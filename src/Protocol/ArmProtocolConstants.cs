namespace StdUnit.Tags.ArmEncoder.Protocol;

/// <summary>
/// 助力臂（编码器）RS485 通讯协议常量。
/// 帧结构：Header + Address + 功能码(CMD) + 数据(DATA) + CRC16(L) + CRC16(H)。
/// CRC16 采用 Modbus CRC16（poly=0x8005，init=0xFFFF，输入/输出反转），
/// 对 Header 至 DATA 的全部字节进行校验，校验结果按低字节在前（CRC16(L)）顺序附加。
/// </summary>
public static class ArmProtocolConstants
{
    /// <summary>包头（命令格式头）。</summary>
    public const byte Header = 0x3E;

    /// <summary>设备地址最小值（0-7）。</summary>
    public const byte MinAddress = 0;

    /// <summary>设备地址最大值（0-7）。</summary>
    public const byte MaxAddress = 7;

    /// <summary>广播地址，用于 F0/F1/F2 等可编程地址相关命令。</summary>
    public const byte BroadcastAddress = 0xFF;

    /// <summary>操作成功标志（Flag）。</summary>
    public const byte FlagSuccess = 0;

    /// <summary>帧的最小长度（Header + Address + CMD + CRC16 两字节）。</summary>
    public const int MinFrameLength = 5;
}
