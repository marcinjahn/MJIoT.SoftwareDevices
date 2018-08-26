//ARGS: DeviceKey, DeviceId, Type (sender/listener), PropertyName (only for listener), Custom message to display at the top (optional)


using System;

using MjIot.Devices.Common.Core;
using MjIot.Devices.Common.Models;
using Mj.CmdDashArgsReaderLibrary;
using System.Collections.Generic;

namespace MjIot.Devices.Reference.CmdDevice
{
    class Program
    {
        static readonly string _iotHubUri = "MJIoT-Hub.azure-devices.net";
        static IotHubDevice _device;

        static CmdDashArgsReader _argsReader;

        static void Main(string[] args)
        {
            Console.WriteLine("MJIoT Reference Simulator started");

            SetCmdArgsReader(args);

            var argsCount = _argsReader.GetCount();
            var type = _argsReader.Get("type");
            if ((type == "sender" && argsCount < 4) || (type == "listener" && argsCount < 3) || _argsReader.Get("deviceKey") == null || _argsReader.Get("deviceId") == null)
            {
                Console.WriteLine("Wrong arguments. PRESS RETURN TO TERMINATE");
                Console.ReadLine();
                return;
            }

            if (_argsReader.Get("title") != null)
                Console.WriteLine(_argsReader.Get("title"));

            //if (args[2] == "sender" && args.Length == 5)
            //    Console.WriteLine(args[4]);
            //else if (args[2] == "listener" && args.Length == 4)
            //    Console.WriteLine(args[3]);

            _device = new IotHubDevice(_iotHubUri, _argsReader.Get("deviceKey"), _argsReader.Get("deviceId"));
            _device.OnlineStatusChecked += DisplayOnlineCheckMessage;

            if (type == "sender")
                StartSender(_argsReader.Get("property"), _argsReader.Get("format"));
            else if (type == "listener")
                StartListener();

            while (true) { };
        }

        private static void SetCmdArgsReader(IEnumerable<string> args)
        {
            var definitions = new List<ArgumentDefinition>() {
                new ArgumentDefinition("k", "deviceKey"),
                new ArgumentDefinition("i", "deviceId"),
                new ArgumentDefinition("p", "property"),
                new ArgumentDefinition("m", "title"),
                new ArgumentDefinition("t", "type"),
                new ArgumentDefinition("f", "format"),
            };
            _argsReader = new CmdDashArgsReader(definitions, args);
        }

        private static void StartListener()
        {
            var listener = new Listener(_device);
            var sender = new Sender(_device);
            listener.StartListening();
            listener.MessageReceived += async (senderDev, args) =>
            {
                Console.WriteLine();
                Console.WriteLine($"{args.Payload.PropertyName}: {args.Payload.PropertyValue}");
                var message = sender.CreateMessage(new TelemetryPayload(_device.DeviceId, args.Payload.PropertyName, args.Payload.PropertyValue));
                await sender.SendMessageAsync(message);
            };

            while(true)
            {
                Console.ReadLine();
                _device.CloseAsync().Wait();
                Console.WriteLine("Device connection closed");
            }
        }

        static void DisplayOnlineCheckMessage(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Online status of this device was checked");
            Console.ResetColor();
        }

        static async void StartSender(string propertyName, string format)
        {
            var sender = new Sender(_device);

            while (true)
            {
                Console.Write("New message: ");
                var choice = Console.ReadLine();

                if (choice == "")
                {
                    _device.CloseAsync().Wait();
                    Console.WriteLine("Device connection closed");
                    return;
                }

                if (choice == " " && format != null)
                    choice = GetRandomValue(format, choice);

                Console.WriteLine($"Sending: {choice}");

                var message = sender.CreateMessage(new TelemetryPayload(_device.DeviceId, propertyName, choice));
                await sender.SendMessageAsync(message);
            }
        }

        private static string GetRandomValue(string format, string choice)
        {
            if (format.ToLower() == "string")
                choice = RandomValuesGenerator.GetRandomString(10);
            else if (format.ToLower() == "boolean")
                choice = RandomValuesGenerator.GetRandomBoolean();
            else if (format.ToLower() == "number")
                choice = RandomValuesGenerator.GetRandomNumber();
            return choice;
        }
    }
}
