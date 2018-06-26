using System;
using System.Collections.Generic;
using System.Linq;

namespace MjIot.Devices.Reference.CmdDevice
{
    public class RandomValuesGenerator
    {
        private static Random random = new Random();
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GetRandomBoolean()
        {
            var options = new List<string> { "true", "false" };

            int index = random.Next(options.Count);
            var result = options[index];
            return result;
        }

        public static string GetRandomNumber()
        {
            return random.Next(1, 101).ToString();
        }
    }
}
