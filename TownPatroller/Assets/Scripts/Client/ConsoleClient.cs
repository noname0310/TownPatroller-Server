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
        private Dictionary<ulong, BaseClient> Clients;

        public ConsoleClient(ulong _Id, SocketClient socketClient, Dictionary<ulong, BaseClient> clients) : base(_Id, socketClient, false)
        {
            Clients = clients;
        }

        public override void ManualReceiveData(BasePacket basePacket)
        {
            switch (basePacket.packetType)
            {
                case PacketType.CamConfig:
                    break;

                case PacketType.CamReceived:
                    break;

                case PacketType.CarStatusReceived:
                    break;

                case PacketType.CarStatusChangeReq:
                    break;

                case PacketType.CarGPSSpotStatusChangeReq:
                    break;

                case PacketType.UpdateDataReq:
                    break;

                case PacketType.UpdateConsoleModeReq:
                    ConsoleUpdatePacket cup = (ConsoleUpdatePacket)basePacket;
                    switch (cup.consoleMode)
                    {
                        case ConsoleMode.ViewBotList:

                            break;
                        case ConsoleMode.ViewSingleBot:

                            break;
                        default:
                            break;
                    }
                    break;

                case PacketType.UniversalCommand:
                    break;

                case PacketType.ClientsInfoReq:
                    List<ClientInfo> clientInfos = new List<ClientInfo>();
                    foreach (var item in Clients)
                    {
                        if(item.Value.IsBot == true)
                        {
                            clientInfos.Add(new ClientInfo(item.Value.Id, ((HardwareClient)(item.Value)).gPSPosition));
                        }
                    }
                    SendPacket(new ClientinfoPacket(clientInfos.ToArray()));
                    break;

                default:
                    break;
            }
        }
    }
}
