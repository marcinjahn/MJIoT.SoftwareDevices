using Microsoft.Azure.Devices.Client;
using MjIot.Devices.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MjIot.Devices.Common.Core
{
    public class Listener
    {
        IotHubDevice _device;
        public event EventHandler<MesageReceivedEventArgs> MessageReceived;

        public Listener(IotHubDevice device)
        {
            _device = device;
        }

        public async void StartListening()
        {
            while(true)
            {
                Message receivedMessage = await _device.DeviceClient.ReceiveAsync();

                if (receivedMessage == null)
                    continue;

                var receivedPayload = JsonConvert.DeserializeObject<TelemetryPayload>(Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                OnMessageReceived(new MesageReceivedEventArgs(receivedPayload, DateTime.Now));
                await _device.DeviceClient.CompleteAsync(receivedMessage);
            }

        }

        protected virtual void OnMessageReceived(MesageReceivedEventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}
