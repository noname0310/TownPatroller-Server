using System;
using System.Collections.Generic;
using UnityEngine;
using TownPatroller.SocketServer;
using TownPatroller.Client;
using TPPacket.Packet;
using TPPacket.Serializer;

public class SocketObj : MonoBehaviour
{
    public delegate void DataInvoke(ulong Id, BasePacket basePacket);
    public event DataInvoke OnDataInvoke;
    public delegate void PacketSended(ulong Id, BasePacket basePacket);
    public event PacketSended OnPacketSended;

    public IReadOnlyDictionary<ulong, BaseClient> ConnectedClients { get; private set; }
    public IReadOnlyDictionary<ulong, SocketClient> UnmanagedConnectedClients { get; private set; }
    public IReadOnlyList<SocketClient> UnmanagedPreClients { get; private set; }
    public bool ServerIsRunning { get { return socketServer.ServerIsRunning; } }

    private PacketReceiverObj receiverObj;
    private ClientsManagerObj clientsManagerObj;
    private Queue<Action> TaskQueue;
    private object lockObject = new object();
    private SocketServer socketServer;

    private void Start()
    {
        receiverObj = gameObject.GetComponent<PacketReceiverObj>();
        receiverObj.Init();
        receiverObj.OnDataInvoke += (Id, basepacket) => { OnDataInvoke?.Invoke(Id, basepacket); };
        clientsManagerObj = gameObject.GetComponent<ClientsManagerObj>();
        clientsManagerObj.Init(receiverObj);
        clientsManagerObj.clientsManager.OnPacketSended += (Id, basepacket) => { OnPacketSended?.Invoke(Id, basepacket); };
        ConnectedClients = clientsManagerObj.clientsManager.Clients;

        TaskQueue = new Queue<Action>();
        socketServer = new SocketServer(TaskQueue, this, lockObject);
        UnmanagedConnectedClients = socketServer.clientsManager.SocketClients;
        UnmanagedPreClients = socketServer.clientsManager.PreClients;
        socketServer.Start();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("server stopped");
        socketServer.Stop();
    }

    private void Update()
    {
        while (TaskQueue.Count > 0)
        {
            Action act;

            lock (lockObject)
            {
                act = TaskQueue.Dequeue();
            }
            act.Invoke();
        }
    }

    public void ServerStart()
    {
        socketServer.Start();
    }

    public void ServerStop()
    {
        socketServer.Stop();
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
                socketClient.SendPacket(new ConnectionPacket(false, 0, false, true));
                socketClient.Dispose();
                IGConsole.Instance.println("Same Client Already Connected");
            }
            else
            {
                socketServer.clientsManager.UpdatePreClient(connectionpacket.ClientId, connectionpacket.IsBot, socketClient);
                if (connectionpacket.IsBot)
                {
                    clientsManagerObj.clientsManager.AddClient(new HardwareClient(connectionpacket.ClientId, socketClient));
                }
                else
                {
                    clientsManagerObj.clientsManager.AddClient(new ConsoleClient(connectionpacket.ClientId, socketClient, clientsManagerObj.clientsManager.Clients));
                }
                socketClient.SendPacket(new ConnectionPacket(true, 0, false));
                IGConsole.Instance.println("Client Connected    ID(" + connectionpacket.ClientId + ") ISBOT(" + connectionpacket.IsBot + ")");
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
        receiverObj.Dispose(Id);
        IGConsole.Instance.println("Client Disconnected ID(" + Id + ")");
    }
}
