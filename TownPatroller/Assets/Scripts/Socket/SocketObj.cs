using System;
using System.Collections.Generic;
using UnityEngine;
using TownPatroller.SocketServer;
using TownPatroller.Client;
using TPPacket.Packet;
using TPPacket.Serializer;

public class SocketObj : MonoBehaviour
{
    private PacketReceiverObj receiverObj;
    private ClientsManagerObj clientsManagerObj;
    private Queue<Action> TaskQueue;
    private object lockObject = new object();
    private SocketServer socketServer;

    private void Start()
    {
        receiverObj = gameObject.GetComponent<PacketReceiverObj>();
        receiverObj.Init();
        clientsManagerObj = gameObject.GetComponent<ClientsManagerObj>();
        clientsManagerObj.Init(receiverObj);

        TaskQueue = new Queue<Action>();
        socketServer = new SocketServer(TaskQueue, this);
        socketServer.Start();
    }

    private void Update()
    {
        while (TaskQueue.Count > 0)
        {
            Action act;

            lock (lockObject)
            {
                act = TaskQueue.Dequeue();
                act.Invoke();
            }
        }
    }

    public void OnReceiveData(ulong Id, byte[] Buffer)
    {
        Segment segment = (Segment)PacketDeserializer.Deserialize(Buffer);
        receiverObj.AddSegment(Id, segment);
    }

    public void OnPreReceiveData(SocketClient socketClient, byte[] Buffer)
    {
        BasePacket basePacket = (BasePacket)PacketDeserializer.Deserialize(Buffer);
        if(basePacket.packetType == PacketType.ConnectionStat)
        {
            ConnectionPacket connectionpacket = (ConnectionPacket)basePacket;
            if (socketServer.clientsManager.SocketClients.ContainsKey(connectionpacket.ClientId))
            {
                socketClient.SendPacket(new ConnectionPacket(false, 0, false));
            }
            else
            {
                socketServer.clientsManager.UpdatePreClient(connectionpacket.ClientId, connectionpacket.IsBot, socketClient);
                clientsManagerObj.clientsManager.AddClient(new BaseClient(connectionpacket.ClientId, socketClient, connectionpacket.IsBot));
            }
        }
        else
        {
            IGConsole.Instance.println("Client Reset has Problem");
        }
    }

    public void OnClientDisposed(ulong Id)
    {
        clientsManagerObj.clientsManager.RemoveClient(Id);
    }
}
