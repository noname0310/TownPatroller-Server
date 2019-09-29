using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TownPatroller.SocketServer
{
    class SocketServer : TaskQueueManager
    {
        private const int port = 20310;

        private TcpListener server;
        TcpClient clientSocket;
        public ClientsManager clientsManager;
        private Task ServerTask;
        private bool StopTask;
        private bool ServerIsRunning = false;

        private SocketServer()
        {

        }

        public SocketServer(Queue<Action> taskQueue, SocketObj _socketObj)
        {
            TaskQueue = taskQueue;
            socketObj = _socketObj;
        }

        private void ClientsManager_OnReceiveData(ulong Id, ref byte[] Buffer)
        {
            OnReceiveData(Id, Buffer);
        }

        private void ClientsManager_OnPreReceiveData(SocketClient socketClient, ref byte[] Buffer)
        {
            OnPreReceiveData(socketClient, Buffer);
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
            clientsManager = new ClientsManager();
            clientsManager.OnPreReceiveData += ClientsManager_OnPreReceiveData;
            clientsManager.OnReceiveData += ClientsManager_OnReceiveData;
            ServerTask = new Task(new Action(InitSocket));
            ServerTask.Start();
        }

        public void Stop()
        {
            if (ServerIsRunning)
                StopTask = true;
        }

        private void InitSocket()
        {
            IPAddress myIP = IPAddress.Any;

            server = new TcpListener(myIP, port); // 서버 접속 IP, 포트
            clientsManager.Start();

            server.Start(); // 서버 시작
            PrintlnIGConsole("Socket Server Started at " + myIP + ":" + port);
            ServerIsRunning = true;
            while (!StopTask)
            {
                clientSocket = server.AcceptTcpClient(); // client 소켓 접속 허용
                PrintlnIGConsole("Accept connection from client");

                clientsManager.AddPreClient(clientSocket);
            }
            ServerIsRunning = false;
            server.Stop();
            clientsManager.Stop();
            clientsManager.DisposeAllClients();
            PrintlnIGConsole("Socket Server Stopped");
        }
    }
}
