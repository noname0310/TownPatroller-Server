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

        private bool IsFullyListening

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
        public byte[] textureByte { get; private set; }

        public HardwareClient (ulong _Id, SocketClient socketClient) : base(_Id, socketClient, true)
        {
            rotation = -1;
            viwerConsoleClients = new List<ConsoleClient>();
        }

        public void AddViwer(ConsoleClient consoleClient)
        {
            viwerConsoleClients.Add(consoleClient);


        }

        public void RemoveViwer(ConsoleClient consoleClient)
        {

        }

        public void SetCardevice(Cardevice cardevice)
        {
            SendPacket(new CarStatusChangeReqPacket(cardevice));
        }

        public override void ManualReceiveData(BasePacket basePacket)
        {
            switch (basePacket.packetType)
            {
                //case PacketType.ConnectionStat:
                    //break;

                case PacketType.CamFrame:
                    CamPacket cp = (CamPacket)basePacket;
                    textureByte = cp.CamFrame;
                    SendPacket(new CamPacketRecived());
                    break;

                case PacketType.CarStatus:
                    CarStatusPacket csp = (CarStatusPacket)basePacket;
                    cardevice = csp.cardevice;
                    gPSPosition = csp.position;
                    rotation = csp.rotation;
                    SendPacket(new CarStatusRecivedPacket());
                    break;

                case PacketType.CarGPSSpotStatus:
                    CarGPSSpotStatusPacket cgssp = (CarGPSSpotStatusPacket)basePacket;
                    _spotManager = cgssp.gPSMover;
                    break;

                //case PacketType.CarStatusChanged:
                    //break;

                case PacketType.UpdateDataChanged:
                    DataUpdatedPacket dup = (DataUpdatedPacket)basePacket;
                    _modeType = dup.modeType;
                    break;

               //case PacketType.UniversalCommand:
                    //break;

                default:
                    break;
            }
        }
    }
}
