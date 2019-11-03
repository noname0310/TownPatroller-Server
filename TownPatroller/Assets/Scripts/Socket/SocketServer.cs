using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using TPPacket.Packet;

namespace TownPatroller.SocketServer
{
    public class SocketServer : TaskQueueManager
    {
        private const int port = 20310;

        private TcpListener server;
        TcpClient clientSocket;
        public SocketClientsManager clientsManager;
        private Task ServerTask;
        private bool StopTask;
        private bool ServerIsRunning = false;

        public SocketServer(Queue<Action> taskQueue, SocketObj _socketObj, object LockObject) : base(LockObject)
        {
            TaskQueue = taskQueue;
            socketObj = _socketObj;
        }

        private void ClientsManager_OnReceiveData(ulong Id, byte[] Buffer)
        {
            OnReceiveData(Id, Buffer);
        }

        private void ClientsManager_OnPreReceiveData(SocketClient socketClient, byte[] Buffer)
        {
            OnPreReceiveData(socketClient, Buffer);
        }
        private void ClientsManager_OnClientDisposed(ulong Id)
        {
            OnClientDiposed(Id);
        }

        public void Start()
        {
            if (ServerIsRunning)
            {
                StopTask = false;
                if (!ServerIsRunning)
                {
                    goto ResetServer;
                }
                return;
            }
            ResetServer:
            StopTask = false;
            clientSocket = null;
            clientsManager = new SocketClientsManager();
            clientsManager.OnPreReceiveData += ClientsManager_OnPreReceiveData;
            clientsManager.OnReceiveData += ClientsManager_OnReceiveData;
            clientsManager.OnClientDisposed += ClientsManager_OnClientDisposed;
            ServerTask = new Task(new Action(InitSocket));
            ServerTask.Start();
        }

        public void Stop()
        {
            foreach (var item in clientsManager.SocketClients)
            {
                item.Value.SendPacket(new ConnectionPacket(false, 0, false, false));
            }
            foreach (var item in clientsManager.PreClients)
            {
                item.SendPacket(new ConnectionPacket(false, 0, false, false));
            }

            server.Stop();

            if (ServerIsRunning)
                StopTask = true;
        }

        private void InitSocket()
        {
            //IPAddress myIP = IPAddress.Any;
            IPAddress myIP = IPAddress.Any;

            server = new TcpListener(myIP, port);
            clientsManager.Start();

            server.Start();
            PrintlnIGConsole("Socket Server Started at " + myIP + ":" + port);
            ServerIsRunning = true;
            while (!StopTask)
            {
                try
                {
                    clientSocket = server.AcceptTcpClient();
                    PrintlnIGConsole("Accept connection from client");

                    clientsManager.AddPreClient(clientSocket);
                }
                catch (SocketException e)
                {
                    if ((e.SocketErrorCode == SocketError.Interrupted))
                        break;
                }
            }
            ServerIsRunning = false;
            server.Stop();
            clientsManager.Stop();
            clientsManager.DisposeAllClients();
            PrintlnIGConsole("Socket Server Stopped");
        }
    }
}
