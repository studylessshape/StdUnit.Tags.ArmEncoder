using StdUnit.Tags;
using System.Diagnostics;

namespace ArmEncoder.Sample.Tags.Logicets;

public class HeartBeatLogicet : LogicetBase
{
    private readonly ITagGrp _entry;
    private readonly string _entryName;

    private readonly ITag _heartReq;
    private readonly ITag _heartAck;
    private readonly ILogger<HeartBeatLogicet> _logger;

    private readonly ITag _interval;
    private Stopwatch _stopwatch = new Stopwatch();
    private bool _fst = true;
    private TimeSpan _duration = TimeSpan.Zero;


    public HeartBeatLogicet(ITagsProject proj, ILogger<HeartBeatLogicet> logger) : base(proj)
    {
        this._entry = this.Tags.SelectGrp(TagsPathDefines.EntryName);
        this._entryName = this._entry.TagName();

        _heartReq = this._entry.SelectTag(TagsPathDefines.心跳请求);
        _heartAck = this._entry.SelectTag(TagsPathDefines.心跳响应);
        _interval = this._entry.SelectTag(TagsPathDefines.扫描周期);
        this._logger = logger;
    }

    public override int Order => 2;

    public override bool MatchEntry(ITagGrp entry) => entry.TagName() == this._entryName;

    public override Task ProcessAsync(ITagGrp entry, ITagChannel? thisChannel, CancellationToken ct)
    {
        _heartAck.Value = _heartReq.Value;


        // 统计周期
        if (_fst)
        {
            _stopwatch.Start();
            _fst = false;
        }
        else
        {
            _duration = _stopwatch.Elapsed;
            _interval.Value = (float)_duration.TotalMilliseconds;
            _stopwatch.Restart();
        }

        return Task.CompletedTask;
    }
}