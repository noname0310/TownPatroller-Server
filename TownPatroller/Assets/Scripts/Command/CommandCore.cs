using System;
using UnityEngine;

namespace TownPatroller.Command
{
    public class CommandCore
    {
        private CommandObj commandObj;
        protected SocketObj socketObj;

        public CommandCore()
        {
            commandObj = CommandObj.Instance;
            socketObj = GameObject.Find("NetworkManager").GetComponent<SocketObj>();

            ReflectRegisterCommandsNEvents();
        }

        public void ReflectRegisterCommandsNEvents()
        {
            foreach (var item in GetType().GetMethods())
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(item);
                if (attributes.Length == 0)
                {
                    switch (item.Name)
                    {
                        case "OnClientDataInvoked":
                            SocketObj.DataInvoke DataInvokeDelegate = (SocketObj.DataInvoke)item.CreateDelegate(typeof(SocketObj.DataInvoke), this);
                            socketObj.OnDataInvoke += DataInvokeDelegate;
                            break;

                        case "OnPacketSended":
                            SocketObj.PacketSended PacketSendedDelegate = (SocketObj.PacketSended)item.CreateDelegate(typeof(SocketObj.PacketSended), this);
                            socketObj.OnPacketSended += PacketSendedDelegate;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    foreach (var att in attributes)
                    {
                        if (att.GetType() == typeof(CommandAttribute))
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
}
