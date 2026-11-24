namespace StdUnit.Tags.ArmEncoder.DirectTags
{
    /// <summary>
    /// 编码器命令写入接口
    /// </summary>
    public interface IArmEncoderTag
    {
        /// <summary>
        /// 需要写入的编码器命令，在写入完成后会置为空
        /// </summary>
        byte[]? CommandValue { get; set; }
    }
}
