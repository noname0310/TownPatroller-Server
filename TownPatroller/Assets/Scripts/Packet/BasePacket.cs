using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TownPatroller.Packet
{
    [Serializable]
    class BasePacket
    {
        protected SegmentInfo segmentInfo;
        protected PacketType packetType;

        public SegmentInfo Segmentinfo
        {
            get
            {
                return segmentInfo;
            }
            set
            {
                segmentInfo = value;
            }
        }

        public BasePacket()
        {

        }

        public static byte[] Serialize(object o)
        {
            MemoryStream ms = new MemoryStream(1024 * 4);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(ms, o);
            return ms.ToArray();
        }

        public static object Desserialize(byte[] bt)
        {
            MemoryStream ms = new MemoryStream(1024 * 4);
            foreach (byte b in bt)
            {
                ms.WriteByte(b);
            }

            ms.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(ms);
            ms.Close();
            return obj;
        }
    }

    [Serializable]
    enum PacketType
    {
        ConnectionStat,
        CamFrame,
        CarStatus,
        ChangeModeReq,
        ModeChanged,
        SetTrackReq,
        SetTrackResponse,
        ConsoleMsg
    } 
}
