using System;

namespace TownPatroller.Command
{
    [AttributeUsage(AttributeTargets.Method)]
    class CommandAttribute : Attribute
    {
        public string Command;

        public CommandAttribute(string command)
        {
            Command = command;
        }
    }
}
