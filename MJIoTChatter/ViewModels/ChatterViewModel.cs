﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MjIot.Devices.Common.Core;
using MjIot.Devices.Common.Models;
using Mj.CmdDashArgsReaderLibrary;
using MjIot.Devices.Common;
using MJIoTChatter.Models;
using System.Collections.ObjectModel;

namespace MJIoTChatter.ViewModels
{
    public class ChatterViewModel : MJMVVM.NotificationBase
    {
        readonly string _iotHubUri = "MJIoT-Hub.azure-devices.net";

        readonly IotHubDevice _device;
        readonly Sender _sender;
        readonly Listener _listener;

        CmdDashArgsReader _argsReader;

        public ChatterViewModel()
        {
            LastMessage = String.Empty;

            var args = Environment.GetCommandLineArgs().Skip(1);
            SetCmdArgsReader(args);

            DeviceName = _argsReader.Get("name");

            _device = new IotHubDevice(_iotHubUri, _argsReader.Get("deviceKey"), _argsReader.Get("deviceId"));
            _sender = new Sender(_device);
            _listener = new Listener(_device);
            _listener.StartListening();
            _listener.MessageReceived += OnMessageReceived;

            SendCommand = new MJMVVM.DelegateCommand(OnSend, CanSend);

            Messages = new ObservableCollection<ChatMessageBase>();
            //Messages = new ObservableCollection<ChatMessageBase>
            //{
            //    new SentMessage("Some message"),
            //    new ReceivedMessage("Some message1"),
            //    new SentMessage("Some message2"),
            //    new ReceivedMessage("Some message3"),
            //    new SentMessage("Some message2"),
            //    new ReceivedMessage("Some message3"),
            //    new SentMessage("Some message2"),
            //    new ReceivedMessage("Some message3"),
            //    new SentMessage("Some message2"),
            //    new ReceivedMessage("Some message3"),
            //    new SentMessage("Some message2"),
            //    new ReceivedMessage("Some message3")
            //};

        }


        private string _chatBoxContent;
        public string ChatBoxContent
        {
            get { return _chatBoxContent; }
            set { SetProperty(ref _chatBoxContent, value); }
        }

        private string _deviceName;
        public string DeviceName
        {
            get { return _deviceName; }
            set { SetProperty(ref _deviceName, value); }
        }

        private string _lastMessage;
        public string LastMessage
        {
            get { return _lastMessage; }
            set { SetProperty(ref _lastMessage, value); }
        }

        private ObservableCollection<ChatMessageBase> _messages;
        public ObservableCollection<ChatMessageBase> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages, value); }
        }


        private async void OnMessageReceived(object sender, MesageReceivedEventArgs message)
        {
            ChatBoxContent += $"Received: {message.Payload.PropertyValue}{Environment.NewLine}";

            Messages.Add(new ReceivedMessage(message.Payload.PropertyValue));

            var messageToSend = _sender.CreateMessage("Received Message", message.Payload.PropertyValue);
            await _sender.SendMessageAsync(messageToSend);
        }

        private void SetCmdArgsReader(IEnumerable<string> args)
        {
            var definitions = new List<ArgumentDefinition>() {
                new ArgumentDefinition("k", "deviceKey"),
                new ArgumentDefinition("i", "deviceId"),
                new ArgumentDefinition("n", "name"),
            };
            _argsReader = new CmdDashArgsReader(definitions, args);
        }


        #region COMMANDS

        public MJMVVM.DelegateCommand SendCommand { get; private set; }

        #endregion



        #region COMMAND HANDLERS

        async void OnSend(object arg)
        {
            var message = _sender.CreateMessage("Sent Message", LastMessage);
            await _sender.SendMessageAsync(message);

            ChatBoxContent += $"Sent: {LastMessage} {Environment.NewLine}";

            Messages.Add(new SentMessage(LastMessage));

            LastMessage = String.Empty;
        }

        private bool CanSend(object arg)
        {
            //return true;
            return (LastMessage.Length != 0);
        }

        #endregion
    }
}