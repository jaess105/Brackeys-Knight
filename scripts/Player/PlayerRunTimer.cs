using Brackeys.Knight.Interfaces;
using Godot;
using System;

namespace Brackeys.Knight.Player;

[GlobalClass]
public partial class PlayerRunTimer : Node, IPlayerTimerTracker
{
    private ulong _startMS;
    private ulong _endMS;

    private bool _started;
    private bool _finished;

    public ulong? FinalElapsedTime => _finished ? _endMS - _startMS : null;
    public ulong ElapsedTime
    {
        get
        {
            if (_finished) { return FinalElapsedTime.Value; }
            else if (_started) { return Time.GetTicksMsec() - _startMS; }
            else { return 0; }
        }
    }

    public void Start()
    {
        _started = true;
        _startMS = Time.GetTicksMsec();
    }

    public void Stop()
    {
        if (!_finished) { _endMS = Time.GetTicksMsec(); }
        _finished = true;
    }

    public void Reset() => _started = _finished = false;
}
