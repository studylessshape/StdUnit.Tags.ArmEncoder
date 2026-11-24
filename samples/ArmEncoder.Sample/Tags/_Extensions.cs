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
            builder.AddTagsStream(); // webui
            builder.AddSimpleFilesSupport();

            builder.AddArmEncoderSupport();
        });


        // hosted service for background tasks
        services.AddHostedService<DemoBackgroundService>();
        return services;
    }
}