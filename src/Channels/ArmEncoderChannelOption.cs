using System.IO.Ports;

namespace StdUnit.Tags.ArmEncoder.Channels;

/// <summary>
/// 串口通道选项
/// </summary>
public class ArmEncoderChannelOption
{
    /// <summary>
    /// 串口号。默认值为 COM1。<br/>
    /// </summary>
    public string Port { get; set; } = "COM1";

    /// <summary>
    /// 波特率
    /// </summary>
    public int BaudRate { get; set; } = 115200;

    /// <summary>
    /// Parity
    /// </summary>
    public Parity Parity { get; set; }

    /// <summary>
    /// 数据位
    /// </summary>
    public int DataBits { get; set; }

    /// <summary>
    /// 停止位
    /// </summary>
    public StopBits StopBits { get; set; }

    /// <summary>
    /// 为 True 时在写入值时，不需要自带 Crc16 的尾部校验码，而是由 Tag 在写入时自动计算。
    /// </summary>
    public bool AutoWriteCrc16 { get; set; } = true;

    /// <summary>
    /// 通道元素数量
    /// </summary>
    public int ChannelCapacity { get; set; } = 1;
}
