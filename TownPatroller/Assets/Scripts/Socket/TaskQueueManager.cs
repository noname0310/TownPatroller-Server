using System;
using System.Collections.Generic;

namespace TownPatroller.SocketServer
{
    public class TaskQueueManager
    {
        protected Queue<Action> TaskQueue;
        protected SocketObj socketObj;

        private object lockObject;
        public TaskQueueManager(object LockObject)
        {
            lockObject = LockObject;
        }

        protected void PrintlnIGConsole(string msg)
        {
            Action act = () => IGConsole.Instance.println(msg);
            LockEnqueue(act);
        }

        protected void OnReceiveData(ulong Id, byte[] Buffer, int DataLength)
        {
            byte[] BufferC = new byte[DataLength];
            for (int i = 0; i < DataLength; i++)
            {
                BufferC[i] = Buffer[i];
            }

            Action act = () => socketObj.OnReceiveData(Id, BufferC);
            LockEnqueue(act);
        }

        protected void OnPreReceiveData(SocketClient socketClient, byte[] Buffer, int DataLength)
        {
            byte[] BufferC = new byte[DataLength];
            for (int i = 0; i < DataLength; i++)
            {
                BufferC[i] = Buffer[i];
            }

            Action act = () => socketObj.OnPreReceiveData(socketClient, BufferC);
            LockEnqueue(act);
        }

        protected void OnClientDiposed(ulong Id)
        {
            Action act = () => socketObj.OnClientDisposed(Id);
            LockEnqueue(act);
        }

        private void LockEnqueue(Action action)
        {
            lock (lockObject)
            {
                TaskQueue.Enqueue(action);
            }
        }
    }
}
