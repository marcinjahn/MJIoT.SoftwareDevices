using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJIoTChatter.Models
{
    class ReceivedMessage : ChatMessageBase
    {
        public ReceivedMessage(string message) : base(message)
        {
        }
    }
}
