using Microsoft.Extensions.Logging;

namespace StdUnit.Tags.ArmEncoder.Channels;

/// <summary>
/// 构建 <see cref="ArmEncoderChannel"/>
/// </summary>
internal class ArmEncoderChannelFactory : ITagChannelFactory
{
    private readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// c'tor
    /// </summary>
    public ArmEncoderChannelFactory(ILoggerFactory loggerFactory)
    {

        this._loggerFactory = loggerFactory;
    }

    private IReadOnlyList<string> _drivers = new List<string>() { ArmEncoderNames.DriverName };

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IReadOnlyList<string> GetAvailableDrivers() => _drivers;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="descriptor"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ITagChannel Create(TagChannelDescriptor descriptor)
    {
        var thisDescriptor = descriptor.ToArmChannelDescriptor();

        var logger = _loggerFactory.CreateLogger<ArmEncoderChannel>();
        return new ArmEncoderChannel(thisDescriptor, logger);
    }
}