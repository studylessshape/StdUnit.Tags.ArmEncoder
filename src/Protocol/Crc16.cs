namespace StdUnit.Tags.ArmEncoder.Protocol;

/// <summary>
/// Modbus CRC16 计算器。
/// poly = 0x8005，初始值 = 0xFFFF，输入与输出均按位反转。
/// 校验值按低字节在前（CRC16(L)）、高字节在后（CRC16(H)）的顺序写入帧尾。
/// </summary>
public static class Crc16
{
    private const ushort Polynomial = 0xA001; // 0x8005 位反转后的形式

    /// <summary>
    /// 计算给定字节序列的 Modbus CRC16。
    /// </summary>
    public static ushort Compute(ReadOnlySpan<byte> data)
    {
        ushort crc = 0xFFFF;
        foreach (var b in data)
        {
            crc ^= b;
            for (var i = 0; i < 8; i++)
            {
                if ((crc & 0x0001) != 0)
                {
                    crc = (ushort)((crc >> 1) ^ Polynomial);
                }
                else
                {
                    crc >>= 1;
                }
            }
        }
        return crc;
    }

    /// <summary>
    /// 在数据末尾追加 CRC16，低字节在前、高字节在后，返回新数组。
    /// </summary>
    public static byte[] Append(ReadOnlySpan<byte> data)
    {
        var crc = Compute(data);
        var result = new byte[data.Length + 2];
        data.CopyTo(result);
        result[data.Length] = (byte)(crc & 0xFF);     // CRC16(L)
        result[data.Length + 1] = (byte)(crc >> 8);   // CRC16(H)
        return result;
    }

    /// <summary>
    /// 校验完整帧（末尾两字节为 CRC16(L)/CRC16(H)）是否正确。
    /// </summary>
    public static bool Verify(ReadOnlySpan<byte> frame)
    {
        if (frame.Length < 2)
        {
            return false;
        }

        var crc = Compute(frame[..^2]);
        return frame[^2] == (byte)(crc & 0xFF) && frame[^1] == (byte)(crc >> 8);
    }
}
