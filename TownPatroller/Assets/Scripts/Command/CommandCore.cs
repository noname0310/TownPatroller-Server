using System;

namespace TownPatroller.Command
{
    public class CommandCore
    {
        CommandObj commandObj;

        public CommandCore()
        {
            commandObj = CommandObj.Instance;

            foreach (var item in GetType().GetMethods())
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(item);
                if (attributes.Length == 0)
                    continue;
                foreach (var att in attributes)
                {
                    if(att.GetType() == typeof(CommandAttribute))
                    {
                        CommandAttribute commandAttribute = (CommandAttribute)att;
                        CommandObj.CommandDelegate commandDelegate = (CommandObj.CommandDelegate)item.CreateDelegate(typeof(CommandObj.CommandDelegate), this);

                        commandObj.CommandDelegates.Add(commandAttribute.Command.ToUpper(), commandDelegate);
                    }
                }
            }
        }
    }
}
