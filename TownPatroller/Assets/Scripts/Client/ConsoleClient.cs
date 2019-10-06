using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TownPatroller.SocketServer;
using TPPacket.Packet;
using TPPacket.Class;

namespace TownPatroller.Client
{
    class ConsoleClient : BaseClient
    {
        public ConsoleMode consoleMode { get; private set; }
        private ulong TargetBot;

        public ConsoleClient(ulong _Id, SocketClient socketClient) : base(_Id, socketClient, true)
        {

        }

        public override void ManualReceiveData(BasePacket basePacket)
        {

        }
    }
}
