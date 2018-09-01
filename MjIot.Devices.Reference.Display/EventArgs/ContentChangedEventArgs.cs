using System;

namespace MjIot.Devices.Reference.Display.EventArgs
{
    public class ContentChangedEventArgs : System.EventArgs
    {
        public ContentChangedEventArgs(string content, DateTime receiveTimestamp)
        {
            Content = content;
            ReceiveTimestamp = receiveTimestamp;
        }

        public string Content { get; set; }
        public DateTime ReceiveTimestamp { get; set; }
    }
}
