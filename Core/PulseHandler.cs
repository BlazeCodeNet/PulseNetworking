using PulseNet.packet;

using System;
using System.Collections.Generic;
using System.Text;

namespace PulseNet.Core
{
    public abstract class PulseHandler
    {
        public abstract void Handle( PulsePacket packet, PulseClient source );
    }
}
