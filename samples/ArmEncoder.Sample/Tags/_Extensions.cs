using StdUnit.Tags;
using StdUnit.Tags.ArmEncoder;
using StdUnit.Tags.S7;
using StdUnit.Tags.SimpleFiles;
using StdUnit.Tags.WebUi;

namespace ArmEncoder.Sample.Tags;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTags(this IServiceCollection services)
    {
        services.AddTagsProjectServices(builder =>
        {
            // S7 支持
            builder.AddS7Support();

            // 简单文件支持
            builder.AddSimpleFilesChannel()
                .AddJsonSerializerContext(MyJsonSerializerContext.Default)
                .AddSimpleFilesDirectTagBuilder(
                    configure: b => b.WithJsonTagFactory<MyJson>(),
                    predicate: b => b.TagDescriptor.TagKind == "MyJson"
                ) 
                // ... 可以以类似手法注册其它特化的节点
                // 内置的简单文件支持
                .AddSimpleFilesDirectTagBuilder();

            builder.AddTagsStream(); // webui

            builder.AddArmEncoderSupport();
        });


        // hosted service for background tasks
        services.AddHostedService<DemoBackgroundService>();
        return services;
    }
}