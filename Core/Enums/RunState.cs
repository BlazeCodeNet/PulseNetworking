namespace PulseNet.Core.Enums
{
    public enum RunState
    {
        STOPPED = -5,
        STOPPING = -1,
        INITIALIZING = 1,
        STARTING = 5,
        STARTED = 10,
    }
}