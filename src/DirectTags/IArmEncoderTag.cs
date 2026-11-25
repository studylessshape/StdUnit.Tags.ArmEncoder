namespace StdUnit.Tags.ArmEncoder.DirectTags
{
    /// <summary>
    /// 编码器命令写入接口
    /// </summary>
    public interface IArmEncoderTag
    {
        /// <summary>
        /// <para>写入命令，类似 <see cref="ITag.WriteAsync(CancellationToken)"/>。</para>
        /// <para>在测点循环的外部，推荐在 <see cref="ITagsProject.WriteIntent(string, TagGrpWriteIntent, out Task)"/> 中调用，以防止死锁。</para>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task WriteCommand(byte[] command, CancellationToken ct = default);
    }
}
