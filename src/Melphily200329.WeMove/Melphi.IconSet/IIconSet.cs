using System;

namespace Melphi.IconSet
{
    public interface IIconSet
    {
        string AllowLeft { get; }
        string AllowRight { get; }
        string AllowUp { get; }
        string AllowDown { get; }
        string Person { get; }
        string Build { get; }
        string Edit { get; }
        string FullScreen { get; }
        string FullScreenExit { get; }
        string MuteVolumeLow { get; }
        string MuteVolumeMedium { get; }
        string MuteVolumeHigh { get; }
        string VolumeLow { get; }
        string VolumeMedium { get; }
        string VolumeHigh { get; }
        string Close { get; }
        string Menu { get; }
        string Play { get; }
        string Pause { get; }
        
    }
}
