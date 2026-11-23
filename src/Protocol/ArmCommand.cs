namespace StdUnit.Tags.ArmEncoder.Protocol;

/// <summary>
/// 助力臂功能码（命令码）。
/// </summary>
public enum ArmCommand : byte
{
    /// <summary>00：获取编码器数据（角度 + 旋转速度 + 旋转方向 + 位移）。</summary>
    GetEncoderData = 0x00,

    /// <summary>01：获取绝对值角度和旋转方向。</summary>
    GetAbsoluteAngle = 0x01,

    /// <summary>02：获取编码器速度。</summary>
    GetEncoderSpeed = 0x02,

    /// <summary>03：获取位移和方向。</summary>
    GetDisplacement = 0x03,

    /// <summary>04：获取当前软件版本号。</summary>
    GetSoftwareVersion = 0x04,

    /// <summary>05：初始化编码器零角度（角度置零）。</summary>
    InitZeroAngle = 0x05,

    /// <summary>06：初始化位移零点。</summary>
    InitZeroDisplacement = 0x06,

    /// <summary>F0：初始化可编程地址。</summary>
    InitProgrammableAddress = 0xF0,

    /// <summary>F1：设置 485 地址。</summary>
    SetAddress = 0xF1,

    /// <summary>F2：读取 RS485 地址。</summary>
    ReadAddress = 0xF2,
}
