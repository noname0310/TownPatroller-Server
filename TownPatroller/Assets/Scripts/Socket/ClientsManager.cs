using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TownPatroller.SocketServer
{
    public class ClientsManager
    {
        private const int BufferSize = 1024 * 4; 

        public delegate void DataInvoked(ulong Id, ref byte[] Buffer);
        public delegate void PreDataInvoked(SocketClient socketClient, ref byte[] Buffer);
        public event DataInvoked OnReceiveData;
        public event PreDataInvoked OnPreReceiveData;

        public readonly Dictionary<ulong, SocketClient> SocketClients;
        public readonly List<SocketClient> PreClients;

        private Task DataReceiveTask;
        private bool StopTask;

        public ClientsManager()
        {
            SocketClients = new Dictionary<ulong, SocketClient>();
            PreClients = new List<SocketClient>();
        }

        public void Start()
        {
            DataReceiveTask = new Task(new Action(DataReceive));
            DataReceiveTask.Start();
        }

        public void Stop()
        {
            StopTask = true;
        }

        public void DisposeAllClients()
        {
            foreach (var item in SocketClients)
            {
                item.Value.Dispose();
            }
            foreach (var item in PreClients)
            {
                item.Dispose();
            }
        }

        public void AddPreClient(TcpClient tcpClient)
        {
            PreClients.Add(new SocketClient(tcpClient, BufferSize));
        }

        public void AddClient(ulong Id, bool IsPatrollBot, TcpClient _TcpClient)
        {
            SocketClient sc = new SocketClient(Id, IsPatrollBot, _TcpClient);
            SocketClients.Add(Id, sc);
        }

        public SocketClient RemoveClient(ulong Id)
        {
            if (SocketClients.TryGetValue(Id, out SocketClient sc))
            {
                SocketClients.Remove(Id);
            }

            return sc;
        }

        public void UpdatePreClient(ulong _Id, bool _IsPatrollBot, SocketClient socketClient)
        {
            if (PreClients.Contains(socketClient))
            {
                socketClient.Update2Client(_Id, _IsPatrollBot);
                PreClients.Remove(socketClient);
                SocketClients.Add(_Id, socketClient);
            }
        }

        private void DataReceive()
        {
            StopTask = false;

            while (!StopTask)
            {
                ReadClientsStream();
                ReadPreClientsStream();
            }
        }

        private void ReadClientsStream()
        {
            foreach (var item in SocketClients)
            {
                int result = item.Value.ReadStream(0);
                if(result == -1)
                {
                    item.Value.Dispose();
                    SocketClients.Remove(item.Key);
                }
                else
                {
                    OnReceiveData?.Invoke(item.Key, ref item.Value.ReadBuffer);
                }
            }
        }

        private void ReadPreClientsStream()
        {
            foreach (var item in PreClients)
            {
                int result = item.ReadStream(0);
                if (result == -1)
                {
                    item.Dispose();
                    PreClients.Remove(item);
                }
                else
                {
                    OnPreReceiveData?.Invoke(item, ref item.ReadBuffer);
                }
            }
        }
    }
}
