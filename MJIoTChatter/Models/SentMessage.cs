using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJIoTChatter.Models
{
    public class SentMessage : ChatMessageBase
    {
        public SentMessage(string message) : base(message)
        {
        }
    }
}
