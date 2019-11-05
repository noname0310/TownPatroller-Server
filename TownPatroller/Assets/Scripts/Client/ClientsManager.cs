using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPPacket.Class;
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

                if(baseClient.IsBot == true)
                {
                    List<ClientInfo> clientInfos = new List<ClientInfo>();
                    foreach (var item in Clients)
                    {
                        if (item.Value.IsBot == true)
                        {
                            clientInfos.Add(new ClientInfo(item.Value.Id, ((HardwareClient)(item.Value)).gPSPosition));
                        }
                    }
                    foreach (var item in Clients)
                    {
                        if (item.Value.IsBot == false)
                        {
                            item.Value.SendPacket(new ClientinfoPacket(clientInfos.ToArray()));
                        }
                    }
                }
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
