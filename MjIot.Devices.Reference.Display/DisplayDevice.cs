using MjIot.Devices.Common;
using MjIot.Devices.Common.Core;
using MjIot.Devices.Reference.Display.EventArgs;
using System;

namespace MjIot.Devices.Reference.Display
{
    public class DisplayDevice
    {
        private readonly IotHubDevice _device;
        private readonly Sender _sender;
        private readonly Listener _listener;

        public event EventHandler<ContentChangedEventArgs> ContentChanged;


        public DisplayDevice(string iotHubUri, string deviceKey, string deviceId)
        {
            _device = new IotHubDevice(iotHubUri, deviceKey, deviceId);
            _sender = new Sender(_device);
            _listener = new Listener(_device);

            _listener.MessageReceived += OnMessageReceived;
        }

        public void Start()
        {
            _listener.StartListening();
        }

        public string Content { get; set; }

        private void OnMessageReceived(object sender, MesageReceivedEventArgs e)
        {
            if (e.Payload.PropertyName != "Content")
                return;
            else
            {
                Content = e.Payload.PropertyValue;
                OnContentChanged(new ContentChangedEventArgs(Content, e.ReceiveTimestamp));
            }
        }

        protected virtual void OnContentChanged(ContentChangedEventArgs e)
        {
            ContentChanged?.Invoke(this, e);
        }
    }
}
