using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Models
{
    public static class CollectionStore
    {
        public static readonly Dictionary<string, Dictionary<string, int>> Data =
            new()
            {
            {
                "SetA",
                new Dictionary<string, int>
                {
                    { "One", 1 },
                    { "Two", 2 }
                }
            },
            {
                "SetB",
                new Dictionary<string, int>
                {
                    { "Three", 3 },
                    { "Four", 4 }
                }
            },
            {
                "SetC",
                new Dictionary<string, int>
                {
                    { "Five", 5 },
                    { "Six", 6 }
                }
            },
            {
                "SetD",
                new Dictionary<string, int>
                {
                    { "Seven", 7 },
                    { "Eight", 8 }
                }
            },
            {
                "SetE",
                new Dictionary<string, int>
                {
                    { "Nine", 9 },
                    { "Ten", 10 }
                }
            }
            };
    }
}
