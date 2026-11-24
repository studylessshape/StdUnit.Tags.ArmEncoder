using System.IO.Ports;
using System.Xml.Linq;

namespace StdUnit.Tags.ArmEncoder.Channels;

/// <summary>
/// COM 通道描述符
/// </summary>
public class ArmChannelDescriptor : TagChannelDescriptor
{
    /// <summary>
    /// c'tor
    /// </summary>
    public ArmChannelDescriptor()
    {
        this.Driver = ArmEncoderNames.DriverName;
    }

    /// <summary>
    /// 通道选项
    /// </summary>
    public ArmEncoderChannelOption Option { get; set; } = new ArmEncoderChannelOption();

    /// <summary>
    /// 转换为 XElement
    /// </summary>
    /// <returns></returns>
    public override XElement ToXElement()
    {
        var ele = base.ToXElement();
        ele.SetOrAddChild(nameof(Option.Port), this.Option.Port);
        ele.SetOrAddChild(nameof(Option.BaudRate), this.Option.BaudRate);
        ele.SetOrAddChild(nameof(Option.Parity), this.Option.Parity);
        ele.SetOrAddChild(nameof(Option.DataBits), this.Option.DataBits);
        ele.SetOrAddChild(nameof(Option.StopBits), this.Option.StopBits);
        ele.SetOrAddChild(nameof(Option.AutoWriteCrc16), this.Option.AutoWriteCrc16);
        ele.SetOrAddChild(nameof(Option.ChannelCapacity), this.Option.ChannelCapacity);
        return ele;
    }
}

/// <summary>
/// extensions for conversions between <see cref="TagChannelDescriptor"/> and <see cref="ArmChannelDescriptor"/>
/// </summary>
public static class TagChannelDescriptor_ComExtensions
{
    /// <summary>
    /// 转成 <see cref="ArmChannelDescriptor"/>
    /// </summary>
    /// <param name="descriptor"></param>
    /// <returns></returns>
    /// <exception cref="TagsProjectConfigurationException">当前描述符的驱动不是 <see cref="ArmEncoderNames.DriverName"/></exception>
    /// <exception cref="TagsProjectXmlException">串口选项无法解析</exception>
    public static ArmChannelDescriptor ToArmChannelDescriptor(this TagChannelDescriptor descriptor)
    {
        if (descriptor.Driver != ArmEncoderNames.DriverName)
        {
            throw new TagsProjectConfigurationException(
                $"通道驱动错误：期望 {ArmEncoderNames.DriverName}，而当前为{descriptor.Driver}",
                $"Channel({descriptor.Name})");
        }
        if (descriptor is ArmChannelDescriptor d)
        {
            return d;
        }

        int defaultBaudRate = 9600;
        Parity defaultParity = Parity.None;
        int defaultDataBits = 8;
        StopBits defaultStopBits = StopBits.One;
        int defaultChannelCapacity = 1;
        var defaultAutoWriteCrc16 = true;
        var location = $"Channel({descriptor.Name})";

        var port = !descriptor.Extras.TryGetValue(nameof(ArmChannelDescriptor.Option.Port), out var comPort) ?
                    "COM1" :
                    comPort.Value;
        // 向后兼容：0.10 及之前版本使用拼写错误的 `<BaundRate>` 元素名，0.11 起修正为 `BaudRate`。
        // 优先读新拼写，找不到时回退旧拼写——现场存量 XML 无需修改即可升级。
        var baudRate =
                    !descriptor.Extras.TryGetValue(nameof(ArmChannelDescriptor.Option.BaudRate), out var baudRateStr) &&
                    !descriptor.Extras.TryGetValue("BaundRate", out baudRateStr) ? defaultBaudRate :
                    int.TryParse(baudRateStr.Value, out var baudRateVal) ? baudRateVal :
                    throw new TagsProjectXmlException($"串口波特率非法，无法解析成整数({baudRateStr.Value})", location);
        var parity = !descriptor.Extras.TryGetValue(nameof(ArmChannelDescriptor.Option.Parity), out var parityStr) ? defaultParity :
                    Enum.TryParse<Parity>(parityStr.Value, out var parityVal) ? parityVal :
                    throw new TagsProjectXmlException($"串口极性非法，无法解析成Parity({parityStr.Value})", location);
        var databits =
                    !descriptor.Extras.TryGetValue(nameof(ArmChannelDescriptor.Option.DataBits), out var databitsStr) ? defaultDataBits :
                    int.TryParse(databitsStr.Value, out var databitsVal) ? databitsVal :
                    throw new TagsProjectXmlException($"串口数据位非法，无法解析成整数({databitsStr.Value})", location);
        var stopbits = !descriptor.Extras.TryGetValue(nameof(ArmChannelDescriptor.Option.StopBits), out var stopbitsStr) ? defaultStopBits :
                    Enum.TryParse<StopBits>(stopbitsStr.Value, out var stopbitsVal) ? stopbitsVal :
                    throw new TagsProjectXmlException($"串口停止位非法，无法解析成StopBits({stopbitsStr.Value})", location);
        var autoWriteCrc16 = !descriptor.Extras.TryGetValue(nameof(ArmChannelDescriptor.Option.AutoWriteCrc16), out var autoWriteCrc16Str) ? defaultAutoWriteCrc16 :
                    bool.TryParse(autoWriteCrc16Str.Value, out var autoWriteCrc16Val) ? autoWriteCrc16Val :
                    throw new TagsProjectXmlException($"自动计算Crc16非法，无法解析成布尔型({autoWriteCrc16Str.Value})", location);
        var channelCapacity = !descriptor.Extras.TryGetValue(nameof(ArmChannelDescriptor.Option.ChannelCapacity), out var channelCapacityStr) ? defaultChannelCapacity :
                   int.TryParse(channelCapacityStr.Value, out var channelCapacityVal) ? channelCapacityVal :
                    throw new TagsProjectXmlException($"通道容量非法，无法解析成正整数({channelCapacityStr.Value})", location);

        var res = new ArmChannelDescriptor
        {
            Name = descriptor.Name,
            Driver = descriptor.Driver,
            Extras = descriptor.Extras,
            Option = new ArmEncoderChannelOption
            {
                Port = port,
                BaudRate = baudRate,
                Parity = parity,
                DataBits = databits,
                StopBits = stopbits,
                AutoWriteCrc16 = autoWriteCrc16,
                ChannelCapacity = channelCapacity,
            },
        };
        return res;
    }
}