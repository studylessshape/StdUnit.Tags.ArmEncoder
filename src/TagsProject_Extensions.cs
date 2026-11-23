using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StdUnit.ArmEncoder.Schemas;
using StdUnit.Tags.ArmEncoder.Channels;
using StdUnit.Tags.ArmEncoder.DirectTags;

namespace StdUnit.Tags.ArmEncoder;

public static class TagsProject_Extensions
{

    #region 基本函数
    /// <summary>
    /// 注册 ArmEncoder 通道
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static TagsProjectServiceBuilder AddArmEncoderTagChannel(this TagsProjectServiceBuilder builder)
    {
        // register channel factory
        builder.Services.AddKeyedSingleton<ITagChannelFactory>(
            ArmEncoderNames.DriverName,
            (sp, _) => new ArmEncoderChannelFactory(sp.GetRequiredService<ILoggerFactory>())
        );
        builder.ConfigChannelsFactory((sp, composite) =>
        {
            var factory = sp.GetRequiredKeyedService<ITagChannelFactory>(ArmEncoderNames.DriverName);
            composite.AddFactory(factory);
        });

        return builder;
    }

    /// <summary>
    /// 注册 ArmEncoder DirectTag 构造器
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configure"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TagsProjectServiceBuilder AddArmEncoderDirectTagBuilder(
        this TagsProjectServiceBuilder builder,
        Action<ArmEncoderDirectTagBuilder>? configure = null,
        Func<ArmEncoderDirectTagBuilder, bool>? predicate = null
        )
    {
        builder.ConfigTagsLoader((sp, composite) =>
        {
            composite.AddDirectTagBuilder(ArmEncoderNames.DriverName, configure, predicate);
        });
        return builder;
    }
    #endregion

    /// <summary>
    /// 注册 ArmEncoder 支持。
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static TagsProjectServiceBuilder AddArmEncoderSupport(this TagsProjectServiceBuilder builder)
    {
        builder.Services.AddSingleton<ITagsProjectSchemaProvider, ArmEncoderSchemaProvider>();

        builder
            .AddArmEncoderTagChannel()
            .AddArmEncoderDirectTagBuilder();

        return builder;
    }
}