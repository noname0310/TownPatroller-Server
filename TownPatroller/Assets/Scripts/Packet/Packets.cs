using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TownPatroller.Packet
{
    [Serializable]
    class CamPacket : BasePacket
    {
        readonly Texture CamFrame;
        CamPacket()
        {
            packetType = PacketType.CamFrame;
        }
    }
}
