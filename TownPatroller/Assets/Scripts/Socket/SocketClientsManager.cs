using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TownPatroller.SocketServer
{
    public class SocketClientsManager
    {
        private const int BufferSize = 1012;
        private const int SegmentSize = 1024;

        public delegate void ClientDisposed(ulong Id);
        public event ClientDisposed OnClientDisposed;

        public delegate void DataInvoked(ulong Id, byte[] Buffer, int DataLength);
        public delegate void PreDataInvoked(SocketClient socketClient, byte[] Buffer, int DataLength);
        public event DataInvoked OnReceiveData;
        public event PreDataInvoked OnPreReceiveData;

        public readonly Dictionary<ulong, SocketClient> SocketClients;
        public readonly List<SocketClient> PreClients;

        private Task DataReceiveTask;
        private bool StopTask;

        public SocketClientsManager()
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
            Dictionary<ulong, SocketClient> SC = new Dictionary<ulong, SocketClient>();
            List<SocketClient> PC = new List<SocketClient>(); 

            foreach (var item in SocketClients)
            {
                SC.Add(item.Key, item.Value);
            }
            foreach (var item in PreClients)
            {
                PC.Add(item);
            }

            foreach (var item in SC)
            {
                item.Value.Dispose();
            }
            foreach (var item in PC)
            {
                item.Dispose();
            }
        }

        public void AddPreClient(TcpClient tcpClient)
        {
            PreClients.Add(new SocketClient(tcpClient, SegmentSize, BufferSize));
            PreClients[PreClients.Count - 1].OnClientDisposed += SocketClientsManager_OnClientDisposed;
            PreClients[PreClients.Count - 1].OnPreClientDisposed += SocketClientsManager_OnPreClientDisposed;
        }

        private void SocketClientsManager_OnPreClientDisposed(SocketClient socketClient)
        {
            RemovePreClient(socketClient);
        }

        private void SocketClientsManager_OnClientDisposed(ulong Id)
        {
            OnClientDisposed?.Invoke(Id);
            RemoveClient(Id);
        }

        public SocketClient RemoveClient(ulong Id)
        {
            if (SocketClients.TryGetValue(Id, out SocketClient sc))
            {
                SocketClients.Remove(Id);
            }

            return sc;
        }

        public SocketClient RemovePreClient(SocketClient sc)
        {
            if (PreClients.Contains(sc))
            {
                PreClients.Remove(sc);
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
                try
                {
                    ReadClientsStream();
                    ReadPreClientsStream();
                }
                catch { }
            }
        }

        private void ReadClientsStream()
        {
            foreach (var item in SocketClients)
            {
                int result = item.Value.ReadStream();
                if(result == -1)
                {

                }
                else if(result == -2)
                {

                }
                else
                {
                    OnReceiveData?.Invoke(item.Key, item.Value.ReadBuffer, result);
                }
            }
        }

        private void ReadPreClientsStream()
        {
            for (int i = 0; i < PreClients.Count; i++)
            {
                int result = PreClients[i].ReadStream();
                if (result == -1)
                {
                }
                else if (result == -2)
                {
                }
                else
                {
                    OnPreReceiveData?.Invoke(PreClients[i], PreClients[i].ReadBuffer, result);
                }
            }
        }
    }
}
