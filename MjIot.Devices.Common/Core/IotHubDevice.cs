using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;


namespace MjIot.Devices.Common.Core
{
    public class IotHubDevice
    {
        public DeviceClient DeviceClient { get; }

        private readonly string _deviceKey;
        private readonly string _iotHubUri;

        public event EventHandler OnlineStatusChecked;

        public string DeviceId { get; set; }

        public IotHubDevice(string iotHubUri, string deviceKey, string deviceId, TransportType transportType = TransportType.Mqtt)
        {
            _iotHubUri = iotHubUri;
            _deviceKey = deviceKey;
            DeviceId = deviceId;

            DeviceClient = DeviceClient.Create(_iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(DeviceId, _deviceKey), transportType);
            SetConnectionCheckHandler();

            //Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }

        public async Task CloseAsync()
        {
            await DeviceClient.CloseAsync();
        }

        protected virtual void OnOnlineStatusChecked(EventArgs e)
        {
            OnlineStatusChecked?.Invoke(this, e);
        }

        private void SetConnectionCheckHandler()
        {
            OnOnlineStatusChecked(EventArgs.Empty);
            DeviceClient.SetMethodHandlerAsync("conn", ConnectionCheck, null);
        }

        private Task<MethodResponse> ConnectionCheck(MethodRequest methodRequest, object userContext)
        {
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(""), 200));
        }

    }
}
