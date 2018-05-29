using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using MjIot.Devices.Common.Models;


namespace MjIot.Devices.Common.Core
{
    public class Sender
    {
        IotHubDevice _device;

        public Sender(IotHubDevice device)
        {
            _device = device;
        }

        public Message CreateMessage(string propertyName, object propertyValue)
        {
            var payload = new TelemetryPayload(_device.DeviceId, propertyName, propertyValue.ToString());
            return CreateMessage(payload);
        }
        
        public Message CreateMessage(TelemetryPayload payload)
        {
            var messageString = JsonConvert.SerializeObject(payload);
            return new Message(Encoding.ASCII.GetBytes(messageString));
        }

        public async void SendMessageAsync(Message message)
        {
            await _device.DeviceClient.SendEventAsync(message);
        }
    }
}
