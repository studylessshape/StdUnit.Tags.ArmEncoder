namespace StdUnit.Tags.ArmEncoder.Protocol;

/// <summary>00：获取编码器数据（角度 + 旋转速度 + 旋转方向 + 位移）的响应。</summary>
/// <param name="Angle">编码器角度数据（uint16_t，小端）。</param>
/// <param name="Speed">旋转速度（int16_t，小端）。</param>
/// <param name="Direction">旋转方向。</param>
/// <param name="Displacement">位移（int64_t，大端），由拉线位移传感器测得。</param>
public record ArmRealtimeData(ushort Angle, short Speed, ArmDirection Direction, long Displacement);

/// <summary>01：获取绝对值角度和旋转方向的响应。</summary>
/// <param name="Angle">编码器绝对值角度（uint16_t，小端）。</param>
/// <param name="Direction">旋转方向。</param>
public record ArmAbsoluteAngle(ushort Angle, ArmDirection Direction);

/// <summary>02：获取编码器速度的响应。</summary>
/// <param name="Speed">旋转速度（int16_t，小端）。</param>
public record ArmEncoderSpeed(short Speed);

/// <summary>03：获取位移和方向的响应。</summary>
/// <param name="Displacement">位移（int64_t，大端）。</param>
/// <param name="Direction">旋转方向。</param>
public record ArmDisplacement(long Displacement, ArmDirection Direction);

/// <summary>04：获取当前软件版本号的响应。</summary>
/// <param name="Version">软件版本（uint16_t，小端）。</param>
public record ArmSoftwareVersion(ushort Version);

/// <summary>05：初始化编码器零角度（角度置零）的响应。</summary>
/// <param name="CurrentAngle">当前角度（uint16_t，小端）。</param>
/// <param name="Flag">执行结果：0 成功，其他为错误。</param>
public record ArmZeroAngleResult(ushort CurrentAngle, byte Flag)
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess => Flag == ArmProtocolConstants.FlagSuccess;
}

/// <summary>06：初始化位移零点的响应。</summary>
/// <param name="Displacement">初始化时刻的位移（int64_t，大端）。</param>
public record ArmZeroDisplacementResult(long Displacement);

/// <summary>F0（初始化可编程地址）/ F1（设置 485 地址）的响应。</summary>
/// <param name="Flag">执行结果：0 成功，其他为错误。</param>
public record ArmCommandResult(byte Flag)
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess => Flag == ArmProtocolConstants.FlagSuccess;
}

/// <summary>F2：读取 RS485 地址的响应。</summary>
/// <param name="Address">当前 RS485 地址（0-7）。</param>
public record ArmReadAddressResult(byte Address);