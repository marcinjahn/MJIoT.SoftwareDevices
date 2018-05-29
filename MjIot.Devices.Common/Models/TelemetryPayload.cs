using System;
using System.Collections.Generic;
using System.Text;

namespace MjIot.Devices.Common.Models
{
    public class TelemetryPayload
    {
        public TelemetryPayload(string deviceId, string propertyName, string propertyValue)
        {
            DeviceId = deviceId;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string DeviceId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }

        
    }
}
