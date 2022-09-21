
using System;

namespace Microsoft.Phone.Tasks
{
    [Flags]
    public enum MediaPlaybackControls
    {
        None = 0,
        Pause = 1,
        Stop = 2,
        FastForward = 4,
        Rewind = 8,
        Skip = 16, // 0x00000010
        All = Skip | Rewind | FastForward | Stop | Pause, // 0x0000001F
    }
}