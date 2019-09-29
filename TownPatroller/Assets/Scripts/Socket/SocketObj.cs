using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using TownPatroller.SocketServer;

public class SocketObj : MonoBehaviour
{
    private Queue<Action> TaskQueue;
    private object lockObject = new object();
    private SocketServer socketServer;

    private void Start()
    {
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

    }

    public void OnPreReceiveData(SocketClient socketClient, byte[] Buffer)
    {

    }
}
