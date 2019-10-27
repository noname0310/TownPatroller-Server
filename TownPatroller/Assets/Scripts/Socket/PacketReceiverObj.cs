using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPPacket.Packet;
using TPPacket.PacketManager;


public delegate void DataInvoke(ulong Id, BasePacket basePacket);

public class PacketReceiverObj : MonoBehaviour
{
    public event DataInvoke OnDataInvoke;

    private Dictionary<ulong, PacketReciver> PacketRecivers;

    public void Init()
    {
        PacketRecivers = new Dictionary<ulong, PacketReciver>();
    }

    public void AddSegment(ulong Id, Segment segment)
    {
        if (PacketRecivers.ContainsKey(Id))
        {
            PacketRecivers[Id].AddSegment(segment);
        }
        else
        {
            PacketRecivers.Add(Id, new PacketReciver(Id));
            PacketRecivers[Id].OnDataInvoke += PacketReceiverObj_OnDataInvoke;
            PacketRecivers[Id].AddSegment(segment);
        }
    }

    private void PacketReceiverObj_OnDataInvoke(ulong Id, BasePacket basePacket)
    {
        IGConsole.Instance.println("" + Id + ":" + basePacket.packetType.ToString() + " - PacketReceived");
        OnDataInvoke?.Invoke(Id, basePacket);
    }

    public void Dispose(ulong Id)
    {
        if (PacketRecivers.ContainsKey(Id))
        {
            PacketRecivers.Remove(Id);
        }
    }
}
