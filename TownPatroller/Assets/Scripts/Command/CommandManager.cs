using System.Text;
using TownPatroller.Client;
using TPPacket.Packet;

namespace TownPatroller.Command
{
    public class CommandManager : CommandCore
    {
        bool ShowPacketReceiveMsg;
        bool ShowPacketSendMsg;

        public CommandManager()
        {
            ShowPacketReceiveMsg = false;
            ShowPacketSendMsg = false;
        }

        public void OnClientDataInvoked(ulong Id, BasePacket basePacket)
        {
            if (!ShowPacketReceiveMsg)
                return;

            IGConsole.Instance.println("" + Id + ":" + basePacket.packetType.ToString() + GetPacketMembers(basePacket) + " - PacketReceived");
        }

        public void OnPacketSended(ulong Id, BasePacket basePacket)
        {
            if (!ShowPacketSendMsg)
                return;

            IGConsole.Instance.println("" + Id + ":" + basePacket.packetType.ToString() + GetPacketMembers(basePacket) + " - PacketSended");
        }

        public string GetPacketMembers(BasePacket basePacket)
        {
            StringBuilder PacketMembers = new StringBuilder("(");

            switch (basePacket.packetType)
            {
                case PacketType.ConnectionStat:
                    ConnectionPacket cp = basePacket as ConnectionPacket;
                    PacketMembers.Append("IsConnecting: ");
                    PacketMembers.Append(cp.IsConnecting);
                    PacketMembers.Append(", IsBot: ");
                    PacketMembers.Append(cp.IsBot);
                    PacketMembers.Append(", HasError: ");
                    PacketMembers.Append(cp.HasError);
                    break;
                case PacketType.CamFrame:
                    CamPacket cap = basePacket as CamPacket;
                    PacketMembers.Append("CamFrame: ");
                    PacketMembers.Append(cap.CamFrame);
                    break;
                case PacketType.CamConfig:
                    CamConfigPacket cacc = basePacket as CamConfigPacket;
                    PacketMembers.Append("camaraConfigType: ");
                    PacketMembers.Append(cacc.camaraConfigType.ToString());
                    PacketMembers.Append(", enable: ");
                    PacketMembers.Append(cacc.enable);
                    break;
                case PacketType.CamResolutionReq:
                    CamResolutionReqPacket crqr = basePacket as CamResolutionReqPacket;
                    PacketMembers.Append("Resolution: ");
                    PacketMembers.Append(crqr.Resolution);
                    break;
                case PacketType.CamResolution:
                    CamResolutionPacket crr = basePacket as CamResolutionPacket;
                    PacketMembers.Append("Resolution: ");
                    PacketMembers.Append(crr.Resolution);
                    break;
                case PacketType.CamReceived:
                    CamPacketRecived cpr = basePacket as CamPacketRecived;
                    break;
                case PacketType.CarStatus:
                    CarStatusPacket csp = basePacket as CarStatusPacket;
                    PacketMembers.Append("cardevice: ");
                    PacketMembers.Append(csp.cardevice);
                    PacketMembers.Append(", position: ");
                    PacketMembers.Append(csp.position);
                    PacketMembers.Append(", rotation: ");
                    PacketMembers.Append(csp.rotation);
                    break;
                case PacketType.CarStatusReceived:
                    CarStatusRecivedPacket csrp = basePacket as CarStatusRecivedPacket;
                    break;
                case PacketType.CarGPSSpotStatus:
                    CarGPSSpotStatusPacket cgpsssp = basePacket as CarGPSSpotStatusPacket;
                    PacketMembers.Append("GPSMover: ");
                    PacketMembers.Append(cgpsssp.GPSMover);
                    PacketMembers.Append(", GPSPosition: ");
                    PacketMembers.Append(cgpsssp.GPSPosition);
                    PacketMembers.Append(", GPSSpotManagerChangeType: ");
                    PacketMembers.Append(cgpsssp.GPSSpotManagerChangeType);
                    PacketMembers.Append(", Index: ");
                    PacketMembers.Append(cgpsssp.Index);
                    break;
                case PacketType.CarStatusChangeReq:
                    CarStatusChangeReqPacket cscrp = basePacket as CarStatusChangeReqPacket;
                    PacketMembers.Append("ReqCarDevice: ");
                    PacketMembers.Append(cscrp.ReqCarDevice);
                    break;
                case PacketType.CarGPSSpotStatusChangeReq:
                    CarGPSSpotStatusChangeReqPacket cgpssscrp = basePacket as CarGPSSpotStatusChangeReqPacket;
                    PacketMembers.Append("GPSMover: ");
                    PacketMembers.Append(cgpssscrp.GPSMover);
                    PacketMembers.Append(", GPSPosition: ");
                    PacketMembers.Append(cgpssscrp.GPSPosition);
                    PacketMembers.Append(", GPSSpotManagerChangeType: ");
                    PacketMembers.Append(cgpssscrp.GPSSpotManagerChangeType);
                    PacketMembers.Append(", Index: ");
                    PacketMembers.Append(cgpssscrp.Index);
                    break;
                case PacketType.UpdateDataReq:
                    DataUpdatePacket dup = basePacket as DataUpdatePacket;
                    PacketMembers.Append("modeType: ");
                    PacketMembers.Append(dup.modeType);
                    break;
                case PacketType.UpdateDataChanged:
                    DataUpdatedPacket dudp = basePacket as DataUpdatedPacket;
                    PacketMembers.Append("modeType: ");
                    PacketMembers.Append(dudp.modeType);
                    break;
                case PacketType.UpdateConsoleModeReq:
                    ConsoleUpdatePacket cup = basePacket as ConsoleUpdatePacket;
                    PacketMembers.Append("consoleMode: ");
                    PacketMembers.Append(cup.consoleMode);
                    PacketMembers.Append("TargetBot: ");
                    PacketMembers.Append(cup.TargetBot);
                    break;
                case PacketType.UpdateConsoleModeChanged:
                    ConsoleUpdatedPacket cudp = basePacket as ConsoleUpdatedPacket;
                    PacketMembers.Append("consoleMode: ");
                    PacketMembers.Append(cudp.consoleMode);
                    break;
                case PacketType.UniversalCommand:
                    UniversalCommandPacket ucp = basePacket as UniversalCommandPacket;
                    PacketMembers.Append("key: ");
                    PacketMembers.Append(ucp.key);
                    PacketMembers.Append("keyType: ");
                    PacketMembers.Append(ucp.keyType);
                    break;
                case PacketType.ClientsInfoReq:
                    ClientinfoReqPacket cirp = basePacket as ClientinfoReqPacket;
                    break;
                case PacketType.ClientsInfo:
                    ClientinfoPacket cip = basePacket as ClientinfoPacket;
                    PacketMembers.Append("ClientsInfo: ");
                    PacketMembers.Append(cip.ClientsInfo);
                    break;
                default:
                    break;
            }
            PacketMembers.Append(')');

            return PacketMembers.ToString();
        }

        [Command("test")]
        public void TestCommand(string command, string[] args)
        {
            if (args.Length <= 0)
            {
                IGConsole.Instance.println("more parms need", false);
                return;
            }

            switch (args[0])
            {
                case "apple":
                    IGConsole.Instance.println("rust", false);
                    break;
                case "경직":
                    IGConsole.Instance.println("foo", false);
                    break;
                case "boom":
                    IGConsole.Instance.println("bar", false);
                    break;
                default:
                    IGConsole.Instance.println("invalid parameter", false);
                    break;
            }
        }

        [Command("start")]
        public void ServerStartCommand(string command, string[] args)
        {
            if (socketObj.ServerIsRunning)
            {
                IGConsole.Instance.println("Server is already running", false);
                return;
            }
            socketObj.ServerStart();
        }

        [Command("stop")]
        public void ServerStopCommand(string command, string[] args)
        {
            if (!socketObj.ServerIsRunning)
            {
                IGConsole.Instance.println("Server is already stopped", false);
                return;
            }
            socketObj.ServerStop();
        }

        [Command("pr")]
        public void PacketMsgControl(string command, string[] args)
        {
            if (args.Length <= 0)
            {
                IGConsole.Instance.println("Usage: type \"help\"", false);
                return;
            }

            switch (args[0])
            {
                case "send":
                case "s":
                    if (args.Length <= 1)
                    {
                        IGConsole.Instance.println("invalid parameter", false);
                        return;
                    }

                    switch (args[1])
                    {
                        case "view":
                            ShowPacketSendMsg = true;
                            IGConsole.Instance.println("Enable SendPacket Msg", false);
                            break;
                        case "hide":
                            ShowPacketSendMsg = false;
                            IGConsole.Instance.println("Disable SendPacket Msg", false);
                            break;
                        default:
                            IGConsole.Instance.println("invalid parameter", false);
                            break;
                    }
                    break;

                case "receive":
                case "r":
                    if (args.Length <= 1)
                    {
                        IGConsole.Instance.println("invalid parameter", false);
                        return;
                    }

                    switch (args[1])
                    {
                        case "view":
                            ShowPacketReceiveMsg = true;
                            IGConsole.Instance.println("Enable ReceivePacket Msg", false);
                            break;
                        case "hide":
                            ShowPacketReceiveMsg = false;
                            IGConsole.Instance.println("Disable ReceivePacket Msg", false);
                            break;
                        default:
                            IGConsole.Instance.println("invalid parameter", false);
                            break;
                    }
                    break;

                default:
                    break;
            }
        }

        [Command("status")]
        public void ShowStatus(string command, string[] args)
        {
            int ClientsCount = socketObj.ConnectedClients.Count;
            int HardwareClientsCount = 0;
            int ConsoleClientsCount = 0;

            foreach (var item in socketObj.ConnectedClients)
            {
                if (item.Value.IsBot == true)
                {
                    HardwareClientsCount++;
                }
                else
                {
                    ConsoleClientsCount++;
                }
            }
            int i = 0;

            IGConsole.Instance.println("Clients(" + ClientsCount + "), HardwareClients(" + HardwareClientsCount + "), ConsoleClients(" + ConsoleClientsCount + ")", false);
            IGConsole.Instance.println("", false);
            IGConsole.Instance.println("index  ClientID                           IsBot      TargetClient                      ViwersCount    GPSPos", false);
            foreach (var item in socketObj.ConnectedClients)
            {
                if (item.Value.IsBot == true)
                {
                    HardwareClient hc = item.Value as HardwareClient;
                    IGConsole.Instance.println(i.ToString("D3") + "    " + hc.Id.ToString("D19") + "   " + hc.IsBot.ToString().ToUpper().PadRight(5) + "   N/A                                   " + hc.ViwerCount.ToString("D3") + "                  " + hc.gPSPosition.LocationName.ToString() + ", latitude: " + hc.gPSPosition.latitude + ", longitude: " + hc.gPSPosition.longitude, false);
                }
                else
                {
                    ConsoleClient cc = item.Value as ConsoleClient;
                    IGConsole.Instance.println(i.ToString("D3") + "    " + cc.Id.ToString("D19") + "   " + cc.IsBot.ToString().ToUpper().PadRight(5) + "   " + cc.TargetBot.ToString("D19") + "      N/A                  N/A", false);
                }
                i++;
            }
        }

        [Command("kick")]
        public void KickClient(string command, string[] args)
        {
            if (args.Length <= 0)
            {
                IGConsole.Instance.println("Usage: type \"help\"", false);
                return;
            }

            if (ulong.TryParse(args[0], out ulong Id))
            {
                if (socketObj.ConnectedClients.ContainsKey(Id))
                {
                    socketObj.ConnectedClients[Id].SendPacket(new ConnectionPacket(false, 0, false, false));
                    socketObj.ConnectedClients[Id].RootDispose();
                }
                else
                {
                    IGConsole.Instance.println("invalid ID", false);
                }
            }
            else
            {
                switch (args[0])
                {
                    case "all":
                        foreach (var item in socketObj.ConnectedClients)
                        {
                            item.Value.SendPacket(new ConnectionPacket(false, 0, false, false));
                            item.Value.RootDispose();
                        }
                        break;
                    case "index":
                        if (args.Length <= 1)
                        {
                            IGConsole.Instance.println("Usage: type \"help\"", false);
                            return;
                        }

                        if (int.TryParse(args[1], out int index))
                        {
                            int i = 0;
                            BaseClient baseClient = null;

                            foreach (var item in socketObj.ConnectedClients)
                            {
                                if (i == index)
                                {
                                    baseClient = item.Value;
                                }
                                i++;
                            }
                            if (baseClient != null)
                            {
                                baseClient.SendPacket(new ConnectionPacket(false, 0, false, false));
                                baseClient.RootDispose();
                            }
                            else
                            {
                                IGConsole.Instance.println("invalid index", false);
                            }
                        }
                        else
                        {
                            IGConsole.Instance.println("invalid parameter", false);
                        }
                        break;
                    default:
                        IGConsole.Instance.println("invalid parameter", false);
                        break;
                }
            }
        }

        [Command("help")]
        public void HelpMsg(string command, string[] args)
        {
            IGConsole.Instance.println("" +
                "start - start server\n" +
                "stop - stop server\n" +
                "pr (send/receive) (view/hide) - view packet log\n" +
                "status - view clients status\n" +
                "kick (clientID/all/\"index\") (index) - kick client"
                , false);
        }
    }
}
