using System;

namespace MjIot.Devices.Reference.HysteresisController
{
    class Program
    {
        static readonly string _iotHubUri = "MJIoT-Hub.azure-devices.net";
        static readonly string _deviceKey = "XTuB/oJP9zmBJ3fJL8Cla4zqX9dfdJPCl6rhtN220Ig=";
        static readonly string _devideId = "45";

        static void Main(string[] args)
        {
            Console.WriteLine("---MJIoT Hysteresis Controller 1---");

            var controller = new HysteresisController(_iotHubUri, _deviceKey, _devideId, 25, 1, new ConsoleLogger());
            controller.Start();

            Console.ReadLine();

        }
    }
}