using System;

namespace MjIot.Devices.Reference.Chatter.Models
{
    public class ChatMessageBase
    {
        public ChatMessageBase(string message)
        {
            Message = message;
            Time = DateTime.Now;
        }

        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}