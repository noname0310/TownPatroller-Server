using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TownPatroller.SocketServer;
using TownPatroller.Client.Helper;
using TPPacket.Packet;
using TPPacket.Class;

namespace TownPatroller.Client
{
    class HardwareClient : BaseClient
    {
        /// <summary>
        /// 읽기 가능 멤버들
        /// </summary>
        /// 

        public GPSPosition gPSPosition { get; private set; }
        public float rotation { get; private set; }
        public Texture CamFrame { get; private set; }
        public Cardevice cardevice { get; private set; }

        /// <summary>
        /// 읽고 패킷 통신 할당이 가능한 멤버들
        /// </summary>
        /// 

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
        private Texture2D texture2D;

        public HardwareClient (ulong _Id, SocketClient socketClient) : base(_Id, socketClient, true)
        {
            rotation = -1;
        }

        public void SetCarMember(CarMember carMember, ushort value)
        {
            SendPacket(new CarStatusChangeReqPacket(carMember, value, 0, false));
        }

        public void SetCarMember(CarMember carMember, byte value)
        {
            SendPacket(new CarStatusChangeReqPacket(carMember, 0, value, false));
        }

        public void SetCarMember(CarMember carMember, bool value)
        {
            SendPacket(new CarStatusChangeReqPacket(carMember, 0, 0, value));
        }

        public override void ManualReceiveData(BasePacket basePacket)
        {
            switch (basePacket.packetType)
            {
                //case PacketType.ConnectionStat:
                    //break;

                case PacketType.CamFrame:
                    CamPacket cp = (CamPacket)basePacket;
                    if (texture2D != null)
                    {
                        UnityEngine.Object.Destroy(texture2D);
                    }
                    texture2D = TextureConverter.Base64ToTexture2D(cp.CamFrame);
                    CamFrame = texture2D;
                    SendPacket(new CamPacketRecived());
                    GameObject.Find("TestIMG").GetComponent<UnityEngine.UI.RawImage>().texture = CamFrame;
                    break;

                case PacketType.CarStatus:
                    CarStatusPacket csp = (CarStatusPacket)basePacket;
                    cardevice = csp.cardevice;
                    gPSPosition = csp.position;
                    rotation = csp.rotation;
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
