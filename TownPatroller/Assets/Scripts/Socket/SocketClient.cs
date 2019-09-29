using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TownPatroller.SocketServer
{
    public class SocketClient
    {
        public ulong Id { get; private set; }
        public bool IsPatrollBot { get; private set; }
        public bool IsReady { get; private set; }
        public NetworkStream m_networkStream { get; private set; }
        public byte[] ReadBuffer;
        public byte[] SendBuffer;

        public TcpClient tcpClient { get; private set; }

        private SocketClient()
        {
            Id = 0;
            CheckReady();
        }

        public SocketClient(TcpClient _tcpClient, int BufferSize)
        {
            Id = 0;

            ReadBuffer = new byte[BufferSize];
            SendBuffer = new byte[BufferSize];

            tcpClient = _tcpClient;
            m_networkStream = tcpClient.GetStream();

            CheckReady();
        }

        public SocketClient(ulong _Id, bool _IsPatrollBot, TcpClient _tcpClient)
        {
            Id = _Id;
            IsPatrollBot = _IsPatrollBot;
            tcpClient = _tcpClient;
            m_networkStream = tcpClient.GetStream();

            CheckReady();
        }

        public void Update2Client(ulong _Id, bool _IsPatrollBot)
        {
            Id = _Id;
            IsPatrollBot = _IsPatrollBot;

            CheckReady();
        }

        private void CheckReady()
        {
            if(Id != 0 || tcpClient != null)
            {
                IsReady = true;
            }
            else
            {
                IsReady = false;
            }
        }

        public int ReadStream(int offset)
        {
            if (tcpClient.Connected)
            {
                if (0 < tcpClient.Available)
                    return m_networkStream.Read(ReadBuffer, offset, ReadBuffer.Length);
            }
            return -1;
        }

        //public void SendPacket(object packet)
        //{
        //    //Packet.Serialize(object).CopyTo(sendBuffer, 0);
        //    SendData();
        //}

        private void SendData()
        {
            m_networkStream.Write(SendBuffer, 0, SendBuffer.Length);
            m_networkStream.Flush();

            for (int i = 0; i < SendBuffer.Length; i++)
            {
                SendBuffer[i] = 0;
            }
        }

        public void Dispose()
        {
            tcpClient.Dispose();
            m_networkStream.Close();
        }
    }
}
