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
    class HardwareClient : BaseClient
    {
        private List<ConsoleClient> viwerConsoleClients;

        private bool IsCamListening;

        public GPSPosition gPSPosition { get; private set; }
        public float rotation { get; private set; }
        public Cardevice cardevice { get; private set; }

        public GPSSpotManager spotManager
        {
            get
            {
                return _spotManager;
            }
            set
            {
                SendPacket(new CarGPSSpotStatusChangeReqPacket(value));
            }
        }

        public ModeType modeType
        {
            get
            {
                return _modeType;
            }
            set
            {
                SendPacket(new DataUpdatePacket(value));
            }
        }

        private GPSSpotManager _spotManager;
        private ModeType _modeType;

        public HardwareClient (ulong _Id, SocketClient socketClient) : base(_Id, socketClient, true)
        {
            rotation = -1;
            viwerConsoleClients = new List<ConsoleClient>();
            IsCamListening = false;
        }
        public override void Dispose()
        {
            foreach (var item in viwerConsoleClients)
            {
                item.SendPacket(new ConsoleUpdatedPacket(ConsoleMode.ViewBotList));
            }
        }

        public void AddViwer(ConsoleClient consoleClient)
        {
            viwerConsoleClients.Add(consoleClient);

            if (IsCamListening == false)
            {
                IsCamListening = true;

                SendPacket(new CamConfigPacket(CamaraConfigType.SendFrame, true));
            }
        }

        public void RemoveViwer(ConsoleClient consoleClient)
        {
            viwerConsoleClients.Remove(consoleClient);

            if (viwerConsoleClients.Count == 0)
            {
                SendPacket(new CamConfigPacket(CamaraConfigType.SendFrame, false));
                IsCamListening = false;
            }
        }

        public void SetCardevice(ReqCarDevice reqCarDevice)
        {
            SendPacket(new CarStatusChangeReqPacket(reqCarDevice));
        }

        public override void ManualReceiveData(BasePacket basePacket)
        {
            switch (basePacket.packetType)
            {
                case PacketType.CamFrame:
                    foreach (var item in viwerConsoleClients)
                    {
                        if (item.ReceivedCamFrame == true)
                        {
                            item.ReceivedCamFrame = false;
                            item.SendPacket(basePacket);
                        }
                    }
                    SendPacket(new CamPacketRecived());
                    break;

                case PacketType.CarStatus:
                    CarStatusPacket csp = (CarStatusPacket)basePacket;
                    cardevice = csp.cardevice;
                    gPSPosition = csp.position;
                    rotation = csp.rotation;
                    foreach (var item in viwerConsoleClients)
                    {
                        if (item.ReceivedCarStatus == true)
                        {
                            item.ReceivedCarStatus = false;
                            item.SendPacket(basePacket);
                        }
                    }
                    SendPacket(new CarStatusRecivedPacket());
                    break;

                case PacketType.CarGPSSpotStatus:
                    CarGPSSpotStatusPacket cgssp = (CarGPSSpotStatusPacket)basePacket;
                    _spotManager = cgssp.gPSMover; 
                    foreach (var item in viwerConsoleClients)
                    {
                        item.SendPacket(basePacket);
                    }
                    break;

                //case PacketType.CarStatusChanged:
                    //break;

                case PacketType.UpdateDataChanged:
                    DataUpdatedPacket dup = (DataUpdatedPacket)basePacket;
                    _modeType = dup.modeType; 
                    foreach (var item in viwerConsoleClients)
                    {
                        item.SendPacket(basePacket);
                    }
                    break;

               //case PacketType.UniversalCommand:
                    //break;

                default:
                    break;
            }
        }
    }
}
