using System;

namespace PulseNet
{
    public class Protocol
    {
        public enum Sizes
        {
            Byte = 1,
            Short = 2,
            Normal = 4,
            Long = 8,
        }

        public enum DisconnectReasons
        {
            Unknown = 0,
            LostConnection = 1,
            CorrectlyClosed = 2,
        }

        public const int DEFAULT_PORT = 5223;
        public const int DEFAULT_BUFF_SIZE = 1024;

        internal static Random rand = new Random();

        public static void PushLog(string msg)
        {
            OnPublishLog?.Invoke( msg );
        }

        public static void EnableDebugMode()
        {
            debugMode = true;
        }

        public static bool debugMode { get; private set; } = false;

        public delegate void PublishLogEvent( string msg );
        public static event PublishLogEvent OnPublishLog;
    }
}
