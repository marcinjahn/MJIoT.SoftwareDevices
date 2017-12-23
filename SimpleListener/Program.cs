using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Microsoft.Azure.Devices.Client
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace SimpleListener
{
    class Program
    {
        static string iotHubUri = "MJIoT-Hub.azure-devices.net";
        static DeviceClient deviceClient;
        static string deviceKey = "UoqCPr/0p21h1+zBqRy7D8EES6pTmDWMWp47W09V/sQ=";

        static void Main(string[] args)
        {
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("8", deviceKey), Microsoft.Azure.Devices.Client.TransportType.Mqtt);
            ReceiveC2dAsync();

            while (true)
            {

            }
        }

        private static async void ReceiveC2dAsync()
        {
            Console.WriteLine("\nReceiving cloud to device messages from service");
            while (true)
            {
                Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                var value = JsonConvert.DeserializeObject<CloudToDeviceMessage>(Encoding.ASCII.GetString(receivedMessage.GetBytes())).Value;

                if (value == "true")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("ON");
                }
                else if (value == "false")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("OFF");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Message unsupported:");
                    Console.WriteLine(value);
                }

                //SEND NEW VALUE TO IOT HUB:
                var telemetryDataPoint = new
                {
                    //messageId = messageId++,
                    DeviceId = "8",
                    PropertyName = "SimulatedLampState",
                    Value = value
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                //message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");
                await deviceClient.SendEventAsync(message);

                await deviceClient.CompleteAsync(receivedMessage);
            }
        }
    }

    class CloudToDeviceMessage
    {
        public string PropertyName { get; set; }
        public string Value { get; set; }
    }
}
