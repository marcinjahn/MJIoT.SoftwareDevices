using Mj.CmdDashArgsReaderLibrary;
using MjIot.Devices.Reference.Display.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjIot.Devices.Reference.Display.ViewModels
{
    class DisplayViewModel : MJMVVM.NotificationBase
    {
        private CmdDashArgsReader _argsReader;
        private DisplayDevice _displayDevice;

        private string _displayName;
        private string _displayContent;
        private DateTime _lastChangeTime;


        private readonly string _iotHubUri = "MJIoT-Hub.azure-devices.net";

        public DisplayViewModel()
        {
            var args = Environment.GetCommandLineArgs().Skip(1);
            _argsReader = SetupCmdArgsReader(args);

            _displayDevice = SetupDisplayDevice();

            DisplayName = _argsReader.Get("displayName");
            DisplayContent = "---";
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { SetProperty(ref _displayName, value); }
        }

        public string DisplayContent
        {
            get { return _displayContent; }
            set { SetProperty(ref _displayContent, value); }
        }

        public DateTime LastChangeTime
        {
            get { return _lastChangeTime; }
            set { SetProperty(ref _lastChangeTime, value); }
        }


        private CmdDashArgsReader SetupCmdArgsReader(IEnumerable<string> args)
        {
            var definitions = new List<ArgumentDefinition>() {
                new ArgumentDefinition("k", "deviceKey"),
                new ArgumentDefinition("i", "deviceId"),
                new ArgumentDefinition("n", "displayName"),
            };
            return new CmdDashArgsReader(definitions, args);
        }

        private DisplayDevice SetupDisplayDevice()
        {
            var deviceKey = _argsReader.Get("deviceKey");
            var deviceId = _argsReader.Get("deviceId");

            var displayDevice = new DisplayDevice(_iotHubUri, deviceKey, deviceId);
            displayDevice.ContentChanged += DisplayDevice_ContentChanged;
            displayDevice.Start();

            return displayDevice;
        }

        private void DisplayDevice_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            DisplayContent = e.Content;
            LastChangeTime = e.ReceiveTimestamp;
        }
    }
}
