using System;
using System.Collections.Generic;
using System.Text;

namespace DepressedBot.Commands.Attributes
{
    public sealed class UsageAttribute : Attribute
    {
        public string Usage { get; }

        public UsageAttribute(string usage)
        {
            Usage = usage;
        }
    }
}
