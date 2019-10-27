using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPPacket.Packet;

namespace TownPatroller.Client
{
    public class ClientsManager
    {
        public Dictionary<ulong, BaseClient> Clients { get; set; }
        private PacketReceiverObj PacketReciver;

        public ClientsManager(PacketReceiverObj _PacketReciver)
        {
            Clients = new Dictionary<ulong, BaseClient>();
            PacketReciver = _PacketReciver;

            PacketReciver.OnDataInvoke += PacketReciver_OnDataInvoke;
        }

        private void PacketReciver_OnDataInvoke(ulong Id, BasePacket basePacket)
        {
            if(Clients.ContainsKey(Id))
            {
                Clients[Id].ReceiveData(basePacket);
            }
        }

        public void AddClient(BaseClient baseClient)
        {
            if (!Clients.ContainsKey(baseClient.Id))
            {
                Clients.Add(baseClient.Id, baseClient);
            }
        }

        public void RemoveClient(ulong Id)
        {
            if (Clients.ContainsKey(Id))
            {
                Clients[Id].Dispose();
                Clients.Remove(Id);
            }
        }
    }
}
