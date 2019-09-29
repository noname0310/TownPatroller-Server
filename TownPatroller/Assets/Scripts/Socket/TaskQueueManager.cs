using System;
using System.Collections.Generic;

namespace TownPatroller.SocketServer
{
    class TaskQueueManager
    {
        protected Queue<Action> TaskQueue;
        protected SocketObj socketObj;
        protected byte[] Buffer;

        protected void PrintlnIGConsole(string msg)
        {
            Action act = () => IGConsole.Instance.println(msg);
            TaskQueue.Enqueue(act);
        }

        protected void OnReceiveData(ulong Id, byte[] Buffer)
        {
            Action act = () => socketObj.OnReceiveData(Id, Buffer);
            TaskQueue.Enqueue(act);
        }

        protected void OnPreReceiveData(SocketClient socketClient, byte[] Buffer)
        {
            Action act = () => socketObj.OnPreReceiveData(socketClient, Buffer);
            TaskQueue.Enqueue(act);
        }
    }
}
