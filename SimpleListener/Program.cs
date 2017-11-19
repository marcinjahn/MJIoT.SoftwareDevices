using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Microsoft.Azure.Devices.Client
using Microsoft.Azure.Devices.Client;

namespace SimpleListener
{
    class Program
    {
        static string iotHubUri = "MJIoT-IoTHub.azure-devices.net";
        static DeviceClient deviceClient;
        static string deviceKey = "gx9H//lbyFI/5pT8CeX/mVamQHo3mGYcVr1iG0N3pMg=";

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

                var value = Encoding.ASCII.GetString(receivedMessage.GetBytes());

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

                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine("Received message: {0}", Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                //Console.ResetColor();

                await deviceClient.CompleteAsync(receivedMessage);
            }
        }
    }
}
