using System.Threading;
using PulseNet.Core.Enums;

namespace PulseNet.Core.Threading
{
    public class GameThread
    {
        private void MainLoop()
        {
            while(state >= RunState.STOPPING )
            {
                if(state == RunState.STOPPING)
                {
                    state = RunState.STOPPED;
                    ThreadStoppedEvent?.Invoke();
                }
                else
                {
                    ThreadTickEvent?.Invoke();
                }
                Thread.Sleep(22);
            }
        }


         public GameThread()
         {
            mainThread = new Thread(MainLoop);
            mainThread.IsBackground = false;
         }

         public void Start()
         {
             if(state <= RunState.STOPPING)
             {
                 state = RunState.STARTING;
                 mainThread.Start();
             }
         }
         public void Stop()
         {
             if(state >= RunState.STARTING)
             {
                 state = RunState.STOPPING;
             }
             else if(state == RunState.STOPPING)
             {
                 mainThread.Abort();
                 state = RunState.STOPPED;
             }
         }

         private Thread mainThread = null;
         public RunState state { get; private set; } = RunState.STOPPED;

         public delegate void OnThreadStop();
         public event OnThreadStop ThreadStoppedEvent;
         public delegate void OnThreadTick();
         public event OnThreadTick ThreadTickEvent;
    }
}