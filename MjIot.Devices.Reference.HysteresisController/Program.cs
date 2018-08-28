using MjIot.Devices.Common.Core;
using System;
using MjIot.Devices.Common;
using MjIot.Devices.Common.Models;
using System.Threading.Tasks;

namespace MjIot.Devices.Reference.HysteresisController
{
    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class HysteresisController
    {
        IotHubDevice _device;
        Sender _sender;
        Listener _listener;

        IStringToFloatConverter _stringToFloatConverter;
        ILogger _logger;

        float _hysteresisGap;
        float _setPoint;
        float _controlledValue;

        public HysteresisController(string iotHubUri, string deviceKey, string deviceId, float setPoint, float hysteresisGap, ILogger logger)
        {
            _setPoint = setPoint;
            _hysteresisGap = hysteresisGap;

            _stringToFloatConverter = new StringToFloatConverter();
            _logger = logger;

            _device = new IotHubDevice(iotHubUri, deviceKey, deviceId);
            _sender = new Sender(_device);

        }

        public void Start()
        {
            _listener = new Listener(_device);
            _listener.MessageReceived += HandleIncomingMessage;
            _listener.StartListening();

            _logger.Log("Started");
        }

        public void Stop()
        {
            _listener.MessageReceived -= HandleIncomingMessage;
            _listener = null;

            _logger.Log("Stopped");
        }

        private async void HandleIncomingMessage(object sender, MesageReceivedEventArgs args)
        {
            var propertyName = args.Payload.PropertyName;

            _logger.Log($"Received new value of {propertyName}");

            var propertyValueString = args.Payload.PropertyValue;
            float propertyValue;
            if (_stringToFloatConverter.IsNumeric(propertyValueString))
                propertyValue = _stringToFloatConverter.Convert(propertyValueString);
            else
            {
                _logger.Log("Property was not numeric.");
                return;
            }


            if (propertyName == "Set Point")
            {
                _setPoint = propertyValue;
                _logger.Log($"Set Point = {propertyValue}");
                await SendTelemetry("Set Point", propertyValueString);
                await GenerateOutput();
            }
            else if (propertyName == "Controlled Value")
            {
                _controlledValue = propertyValue;
                _logger.Log($"Controlled Value = {propertyValue}");
                await SendTelemetry("Controlled Value", propertyValueString);
                await GenerateOutput();
            }
            else if (propertyName == "Hysteresis Gap")
            {
                if (_hysteresisGap < 0)
                    return;
                _hysteresisGap = propertyValue;
                _logger.Log($"Hysteresis Gap = {propertyValue}");

                await SendTelemetry("Hysteresis Gap", propertyValueString);
                await GenerateOutput();
            }
        }

        private async Task GenerateOutput()
        {
            _logger.Log($"Generating Output");
            var e = _setPoint - _controlledValue;
            var eAbs = Math.Abs(e);
            bool? output = null;

            if (eAbs <= _hysteresisGap)
                return;

            if (e > 0)
                output = true;
            else if (e < 0)
                output = false;

            _logger.Log($"Output = {output}");

            if (output != null)
                await SendTelemetry("Output", output.ToString());
        }

        private async Task SendTelemetry(string propertyName, string value)
        {
            var message = _sender.CreateMessage(new TelemetryPayload(_device.DeviceId, propertyName, value));
            await _sender.SendMessageAsync(message);
        }
    }



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