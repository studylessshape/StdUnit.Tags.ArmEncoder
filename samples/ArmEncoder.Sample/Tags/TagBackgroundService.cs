using StdUnit.Tags;
using ArmEncoder.Sample.Tags.Logicets;

namespace ArmEncoder.Sample.Tags;

class DemoBackgroundService: BackgroundService
{
    private readonly ILogger<DemoBackgroundService> _logger;

    private readonly ITagsProjectCtrl _ctrl;

    public DemoBackgroundService(ITagsProjectCtrl ctrl, ILogger<DemoBackgroundService> logger)
    {
        this._ctrl = ctrl;
        this._logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var dir = Path.Combine(AppContext.BaseDirectory, "Tags");
        stoppingToken.Register(async () =>
        {
            _logger.LogInformation("Tags处理停止");
            await _ctrl.StopAsync();
        });
        await _ctrl.StartPollAsync(dir, null, (proj, sp, ct) =>
        {
            proj.RunnerStarted += (grp, ch, ct) =>
            {
                _logger.LogInformation("Tags处理开始,grp={grpName}", grp.TagName());
                return Task.CompletedTask;
            };
            proj.RunnerCrashed += (grp, ch, ex, ct) =>
            {
                _logger.LogError(ex, "Tags处理异常,grp={grpName}", grp.TagName());
                return Task.CompletedTask;
            };

            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            proj.Logicets.Add(new ArmLogicet(proj, "1#", loggerFactory.CreateLogger<ArmLogicet>()));
            proj.Logicets.Add(new ArmLogicet(proj, "2#", loggerFactory.CreateLogger<ArmLogicet>()));

            return Task.CompletedTask;
        });
    }
}
