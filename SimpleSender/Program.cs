using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;



namespace SimpleSimulator
{
    class Program
    {
        static string iotHubUri = "MJIoT-Hub.azure-devices.net";
        static DeviceClient deviceClient;
        //7
        static string deviceKey = "sCckmieMardTTda24JU38pfduJr3m/fxddT4imPvPvM=";

        //16
        //static string deviceKey = "KH1lBXwZG85HVUOnaeMVn7gT5Xum3+JO+F1+BWCXzfU=";


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
                    PropertyName = "Switch State",
                    PropertyValue = choice == "1" ? "true" : "false"
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
