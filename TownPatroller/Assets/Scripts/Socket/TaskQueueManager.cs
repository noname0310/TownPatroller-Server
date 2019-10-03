using System;
using System.Collections.Generic;

namespace TownPatroller.SocketServer
{
    class TaskQueueManager
    {
        protected Queue<Action> TaskQueue;
        protected SocketObj socketObj;

        protected void PrintlnIGConsole(string msg)
        {
            Action act = () => IGConsole.Instance.println(msg);
            TaskQueue.Enqueue(act);
        }

        protected void OnReceiveData(ulong Id, byte[] Buffer)
        {
            byte[] BufferC = new byte[Buffer.Length];
            for (int i = 0; i < Buffer.Length; i++)
            {
                BufferC[i] = Buffer[i];
            }

            Action act = () => socketObj.OnReceiveData(Id, BufferC);
            TaskQueue.Enqueue(act);
        }

        protected void OnPreReceiveData(SocketClient socketClient, byte[] Buffer)
        {
            byte[] BufferC = new byte[Buffer.Length];
            for (int i = 0; i < Buffer.Length; i++)
            {
                BufferC[i] = Buffer[i];
            }

            Action act = () => socketObj.OnPreReceiveData(socketClient, BufferC);
            TaskQueue.Enqueue(act);
        }

        protected void OnClientDiposed(ulong Id)
        {
            Action act = () => socketObj.OnClientDisposed(Id);
            TaskQueue.Enqueue(act);
        }
    }
}
