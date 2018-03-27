using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


//Microsoft.Azure.Devices.Client
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;



namespace SimpleSimulator
{
    class Program
    {
        static string iotHubUri = "MJIoT-Hub.azure-devices.net";
        static DeviceClient deviceClient;
        static string deviceKey = "sCckmieMardTTda24JU38pfduJr3m/fxddT4imPvPvM=";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("7", deviceKey), Microsoft.Azure.Devices.Client.TransportType.Mqtt);

            deviceClient.SetMethodHandlerAsync("conn", ConnectionCheck, null);

            Task.Run(() => SendDeviceToCloudMessagesAsync());

            while (true) { };
        }


        private static async void SendDeviceToCloudMessagesAsync()
        {
            
            while (true)
            {
                Console.WriteLine("Your message: ");
                var choice = Console.ReadLine();
                var telemetryDataPoint = new
                {
                    //messageId = messageId++,
                    DeviceId = "7",
                    PropertyName = "SimulatedSwitchState",
                    PropertyValue = choice == "1" ? "ON" : "OFF"
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                //message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                //await Task.Delay(1000);
            }
        }

        static Task<MethodResponse> ConnectionCheck(MethodRequest methodRequest, object userContext)
        {
            Console.WriteLine("isConnected check invoked");
            //Console.WriteLine("\t{0}", methodRequest.DataAsJson);
            //Console.WriteLine("\nReturning response for method {0}", methodRequest.Name);

            //string result = "'Input was written to log.'";
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(""), 200));
        }
    }
}
