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

        public bool ReceivedCamFrame;
        public bool ReceivedCarStatus;

        public ConsoleClient(ulong _Id, SocketClient socketClient, Dictionary<ulong, BaseClient> clients) : base(_Id, socketClient, false)
        {
            Clients = clients;
        }

        public override void Dispose()
        {
            if (Clients.ContainsKey(TargetBot))
                ((HardwareClient)Clients[TargetBot]).RemoveViwer(this);
        }

        public override void ManualReceiveData(BasePacket basePacket)
        {
            switch (basePacket.packetType)
            {
                case PacketType.CamConfig:
                    CamConfigPacket ccp = (CamConfigPacket)basePacket;
                    switch (ccp.camaraConfigType)
                    {
                        case CamaraConfigType.ChangeCamara:
                            if (Clients.ContainsKey(TargetBot))
                                Clients[TargetBot].SendPacket(basePacket);
                            break;
                        case CamaraConfigType.SendFrame:
                            break;
                        default:
                            break;
                    }
                    break;

                case PacketType.CamReceived:
                    ReceivedCamFrame = true;
                    break;

                case PacketType.CamResolutionReq:
                    if (Clients.ContainsKey(TargetBot) == true)
                    {
                        Clients[TargetBot].SendPacket(basePacket);
                    }
                    break;

                case PacketType.CarStatusReceived:
                    ReceivedCarStatus = true;
                    break;

                case PacketType.CarStatusChangeReq:
                    CarStatusChangeReqPacket cscr = (CarStatusChangeReqPacket)basePacket;
                    if (Clients.ContainsKey(TargetBot) == true)
                    {
                        ((HardwareClient)Clients[TargetBot]).SetCardevice(cscr.ReqCarDevice);
                    }
                    break;

                case PacketType.CarGPSSpotStatusChangeReq:
                    if (Clients.ContainsKey(TargetBot) == true)
                    {
                        Clients[TargetBot].SendPacket(basePacket);
                    }
                    break;

                case PacketType.UpdateDataReq:
                    if (Clients.ContainsKey(TargetBot) == true)
                    {
                        Clients[TargetBot].SendPacket(basePacket);
                    }
                    break;

                case PacketType.UpdateConsoleModeReq:
                    ConsoleUpdatePacket cup = (ConsoleUpdatePacket)basePacket;
                    switch (cup.consoleMode)
                    {
                        case ConsoleMode.ViewBotList:
                            if (Clients.ContainsKey(TargetBot) == true)
                            {
                                ((HardwareClient)Clients[TargetBot]).RemoveViwer(this);
                                SendPacket(new ConsoleUpdatedPacket(ConsoleMode.ViewBotList));
                            }
                            break;
                        case ConsoleMode.ViewSingleBot:
                            if (Clients.ContainsKey(cup.TargetBot))
                            {
                                ReceivedCamFrame = true;
                                ReceivedCarStatus = true;
                                ((HardwareClient)Clients[cup.TargetBot]).AddViwer(this);
                                TargetBot = cup.TargetBot; 
                                SendPacket(new ConsoleUpdatedPacket(ConsoleMode.ViewSingleBot));
                                SendPacket(new DataUpdatedPacket(((HardwareClient)Clients[cup.TargetBot]).modeType));
                                SendPacket(new CarGPSSpotStatusPacket(GPSSpotManagerChangeType.OverWrite, ((HardwareClient)Clients[cup.TargetBot]).spotManager));
                                SendPacket(new CamResolutionPacket(((HardwareClient)Clients[cup.TargetBot]).CamQuality));
                            }
                            else
                            {
                                SendPacket(new ConsoleUpdatedPacket(ConsoleMode.ViewBotList));
                            }
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
