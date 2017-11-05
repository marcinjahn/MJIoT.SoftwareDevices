using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

//WindowsAzure.ServiceBus
using Microsoft.ServiceBus.Messaging;

//Microsoft.Azure.Devices.Client
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;



namespace SimpleSimulator
{
    class Program
    {
        static string connectionString = "HostName=MJIoT-IoTHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=ieEi5UNBx6C7js/+e6/G+oM3K/isI4WRARv2bBNd270=";
        static string iotHubD2cEndpoint = "messages/events";
        static EventHubClient eventHubClient;

        static string iotHubUri = "MJIoT-IoTHub.azure-devices.net";
        static DeviceClient deviceClient;
        static string deviceKey = "W0U+2qf1rLlutN5lT5DtvtqbT4MvKHWbbTYwJJvz5qs=";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("7", deviceKey), Microsoft.Azure.Devices.Client.TransportType.Mqtt);

            Task.Run( () => SendDeviceToCloudMessagesAsync());

            Console.WriteLine("Receive messages. Ctrl-C to exit.\n");
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, iotHubD2cEndpoint);

            var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

            CancellationTokenSource cts = new CancellationTokenSource();

            System.Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Exiting...");
            };

            var tasks = new List<Task>();
            foreach (string partition in d2cPartitions)
            {
                tasks.Add(ReceiveMessagesFromDeviceAsync(partition, cts.Token));
            }
            Task.WaitAll(tasks.ToArray());
        }

        private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);
            while (true)
            {
                if (ct.IsCancellationRequested) break;
                EventData eventData = await eventHubReceiver.ReceiveAsync();
                if (eventData == null) continue;

                string data = Encoding.UTF8.GetString(eventData.GetBytes());
                Console.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, data);
            }
        }


        private static async void SendDeviceToCloudMessagesAsync()
        {
            double minTemperature = 20;
            double minHumidity = 60;
            int messageId = 1;
            Random rand = new Random();

            while (true)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                var telemetryDataPoint = new
                {
                    messageId = messageId++,
                    deviceId = "myFirstDevice",
                    temperature = currentTemperature,
                    humidity = currentHumidity
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000);
            }
        }
    }
}
