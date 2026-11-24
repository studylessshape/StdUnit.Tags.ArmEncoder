namespace StdUnit.Tags.ArmEncoder.Protocol;

/// <summary>
/// 响应帧解析器：校验包头 / 功能码 / CRC16，并解析出强类型数据。
/// 所有方法在帧非法时抛出 <see cref="InvalidDataException"/>。
/// </summary>
public static class ArmResponseParser
{
    /// <summary>00：获取编码器数据。</summary>
    public static ArmRealtimeData ParseRealtimeData(ReadOnlySpan<byte> frame)
    {
        Validate(frame, ArmCommand.GetEncoderData);
        var angle = ArmFrame.ReadUInt16LittleEndian(frame, 3);
        var speed = ArmFrame.ReadInt16LittleEndian(frame, 5);
        var direction = (ArmDirection)frame[7];
        var displacement = ArmFrame.ReadInt64BigEndian(frame, 8);
        return new ArmRealtimeData(angle, speed, direction, displacement);
    }

    /// <summary>01：获取绝对值角度和旋转方向。</summary>
    public static ArmAbsoluteAngle ParseAbsoluteAngle(ReadOnlySpan<byte> frame)
    {
        Validate(frame, ArmCommand.GetAbsoluteAngle);
        var angle = ArmFrame.ReadUInt16LittleEndian(frame, 3);
        var direction = (ArmDirection)frame[5];
        return new ArmAbsoluteAngle(angle, direction);
    }

    /// <summary>02：获取编码器速度。</summary>
    public static ArmEncoderSpeed ParseEncoderSpeed(ReadOnlySpan<byte> frame)
    {
        Validate(frame, ArmCommand.GetEncoderSpeed);
        var speed = ArmFrame.ReadInt16LittleEndian(frame, 3);
        return new ArmEncoderSpeed(speed);
    }

    /// <summary>03：获取位移和方向。</summary>
    public static ArmDisplacement ParseDisplacement(ReadOnlySpan<byte> frame)
    {
        Validate(frame, ArmCommand.GetDisplacement);
        var displacement = ArmFrame.ReadInt64BigEndian(frame, 3);
        var direction = (ArmDirection)frame[11];
        return new ArmDisplacement(displacement, direction);
    }

    /// <summary>04：获取当前软件版本号。</summary>
    public static ArmSoftwareVersion ParseSoftwareVersion(ReadOnlySpan<byte> frame)
    {
        Validate(frame, ArmCommand.GetSoftwareVersion);
        var version = ArmFrame.ReadUInt16LittleEndian(frame, 3);
        return new ArmSoftwareVersion(version);
    }

    /// <summary>05：初始化编码器零角度。</summary>
    public static ArmZeroAngleResult ParseZeroAngleResult(ReadOnlySpan<byte> frame)
    {
        Validate(frame, ArmCommand.InitZeroAngle);
        var angle = ArmFrame.ReadUInt16LittleEndian(frame, 3);
        var flag = frame[5];
        return new ArmZeroAngleResult(angle, flag);
    }

    /// <summary>06：初始化位移零点。</summary>
    public static ArmZeroDisplacementResult ParseZeroDisplacementResult(ReadOnlySpan<byte> frame)
    {
        Validate(frame, ArmCommand.InitZeroDisplacement);
        var displacement = ArmFrame.ReadInt64BigEndian(frame, 3);
        return new ArmZeroDisplacementResult(displacement);
    }

    /// <summary>F0（初始化可编程地址）/ F1（设置 485 地址）。</summary>
    public static ArmCommandResult ParseCommandResult(ReadOnlySpan<byte> frame, ArmCommand command)
    {
        Validate(frame, command);
        return new ArmCommandResult(frame[3]);
    }

    /// <summary>F2：读取 RS485 地址。</summary>
    public static ArmReadAddressResult ParseReadAddress(ReadOnlySpan<byte> frame)
    {
        Validate(frame, ArmCommand.ReadAddress);
        return new ArmReadAddressResult(frame[3]);
    }

    private static void Validate(ReadOnlySpan<byte> frame, ArmCommand command)
    {
        ArmFrame.Validate(frame, ArmFrame.GetResponseLength(command));

        var expected = (byte)command;
        if (frame[2] != expected)
        {
            throw new InvalidDataException(
                $"响应帧功能码不匹配：期望 0x{expected:X2}，实际 0x{frame[2]:X2}。");
        }
    }

    public static ArmCommand GetFrameCommand(ReadOnlySpan<byte> frame)
    {
        if (frame.Length < 3)
        {
            throw new ArgumentOutOfRangeException("数组的长度小于3，无法获取到对应的命令");
        }

        return (ArmCommand)frame[2];
    }
}
