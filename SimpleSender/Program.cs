﻿using System;
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
        static string iotHubUri = "MJIoT-IoTHub.azure-devices.net";
        static DeviceClient deviceClient;
        static string deviceKey = "W0U+2qf1rLlutN5lT5DtvtqbT4MvKHWbbTYwJJvz5qs=";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("7", deviceKey), Microsoft.Azure.Devices.Client.TransportType.Mqtt);

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
                    Value = choice == "1" ? "true" : "false"
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                //message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                //await Task.Delay(1000);
            }
        }
    }
}