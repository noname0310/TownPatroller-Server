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
        Cardevice cardevice;
        GPSPosition gPSPosition;
        float rotation;
        GPSSpotManager spotManager;
        Texture CamFrame;

        public HardwareClient (ulong _Id, SocketClient socketClient) : base(_Id, socketClient, true)
        {
        }

        public override void ReceiveData(BasePacket basePacket)
        {
            
        }
    }
}
