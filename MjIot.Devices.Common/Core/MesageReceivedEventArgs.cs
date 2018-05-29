using System;
using System.Collections.Generic;
using System.Text;
using MjIot.Devices.Common.Models;

namespace MjIot.Devices.Common
{
    public class MesageReceivedEventArgs : EventArgs
    {
        public MesageReceivedEventArgs(TelemetryPayload payload, DateTime receiveTimestamp)
        {
            Payload = payload;
            ReceiveTimestamp = receiveTimestamp;
        }

        public TelemetryPayload Payload { get; set; }
        public DateTime ReceiveTimestamp { get; set; }
    }
}
