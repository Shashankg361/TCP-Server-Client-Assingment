using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Helpers
{
    public static class MessageParser
    {
        public static bool TryParse(string message, out string setName, out string key)
        {
            setName = "";
            key = "";

            var parts = message.Split('-');

            if (parts.Length != 2)
                return false;

            setName = parts[0];
            key = parts[1];

            return true;
        }
    }
}
