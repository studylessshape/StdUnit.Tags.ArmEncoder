namespace StdUnit.Tags.ArmEncoder.Channels;

/// <summary>
/// 串口操作行为的抽象接口。
/// 仅定义 Open/ReadLine/Write 等行为，不暴露 <see cref="System.IO.Ports.SerialPort"/> 类型。
/// 测试场景可通过此接口直接实现 Mock，无需涉及物理串口。
/// </summary>
public interface ISerialPortHandle : IDisposable
{
    /// <summary>
    /// 串口是否已打开
    /// </summary>
    bool IsOpen { get; }

    /// <summary>
    /// 打开串口
    /// </summary>
    void Open();

    /// <summary>
    /// 关闭串口
    /// </summary>
    void Close();

    /// <summary>
    /// 单个字节读取
    /// </summary>
    /// <returns></returns>
    byte[]? Read();

    /// <summary>
    /// 写入字节数组
    /// </summary>
    void Write(byte[] buffer, int offset, int count);
}
