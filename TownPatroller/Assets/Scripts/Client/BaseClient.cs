using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownPatroller.SocketServer;
using TPPacket.Packet;

namespace TownPatroller.Client
{
    public class BaseClient
    {
        public readonly ulong Id;
        private readonly IClientSender clientSender;
        public readonly bool IsBot;

        public BaseClient(ulong _Id, SocketClient socketClient, bool _IsBot)
        {
            Id = _Id;
            clientSender = socketClient;
            IsBot = _IsBot;
        }

        public virtual void ReceiveData(BasePacket basePacket)
        {

        }

        public void SendPacket(BasePacket basePacket)
        {
            clientSender.SendPacket(basePacket);
        }
    }
}
