using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJIoTChatter.Models
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
