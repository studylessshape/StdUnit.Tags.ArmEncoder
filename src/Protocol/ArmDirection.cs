namespace StdUnit.Tags.ArmEncoder.Protocol;

/// <summary>
/// 助力臂旋转方向。
/// </summary>
public enum ArmDirection : byte
{
    /// <summary>负方向。</summary>
    Negative = 0,

    /// <summary>正方向。</summary>
    Positive = 1,

    /// <summary>停止。</summary>
    Stopped = 2,
}
