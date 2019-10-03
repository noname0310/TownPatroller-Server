﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TownPatroller.Command
{
    public class CommandManager : CommandCore
    {
        [Command("test")]
        public void TestCommand(string command, string[] args)
        {
            if (args.Length <= 0)
            {
                IGConsole.Instance.println("인자가 부족합니다");
                return;
            }

            switch (args[0])
            {
                case "apple":
                    IGConsole.Instance.println("I Love Apple");
                    break;
                case "경직":
                    IGConsole.Instance.println("경직의방");
                    break;
                case "boom":
                    IGConsole.Instance.println("뿜!");
                    break;
                default:
                    break;
            }
        }

        [Command("start")]
        public void TestCommServerStartCommand(string command, string[] args)
        {
            GameObject.Find("NetworkManager").GetComponent<SocketObj>().socketServer.Start();
        }

        [Command("stop")]
        public void TestCommServerStopCommand(string command, string[] args)
        {
            GameObject.Find("NetworkManager").GetComponent<SocketObj>().socketServer.Stop();
        }
    }
}
