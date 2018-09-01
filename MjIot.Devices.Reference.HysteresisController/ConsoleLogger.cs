using System;

namespace MjIot.Devices.Reference.HysteresisController
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}