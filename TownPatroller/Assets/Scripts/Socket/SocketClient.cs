using System.Net.Sockets;
using TPPacket.Serializer;

namespace TownPatroller.SocketServer
{
    public interface IClientSender
    {
        void SendPacket(object packet);
    }

    public class SocketClient : IClientSender
    {
        public delegate void ClientDisposed(ulong Id);
        public event ClientDisposed OnClientDisposed;

        public ulong Id { get; private set; }
        public bool IsPatrollBot { get; private set; }
        public bool IsReady { get; private set; }
        public NetworkStream m_networkStream { get; private set; }
        public byte[] ReadBuffer;
        public byte[] SendBuffer;

        public TcpClient tcpClient { get; private set; }

        private PacketSerializer packetSerializer;

        private SocketClient()
        {

        }

        public SocketClient(TcpClient _tcpClient, int SegmentSize, int BufferSize)
        {
            Id = 0;

            ReadBuffer = new byte[SegmentSize];
            SendBuffer = new byte[SegmentSize];

            tcpClient = _tcpClient;
            m_networkStream = tcpClient.GetStream();

            CheckReady();

            packetSerializer = new PacketSerializer(BufferSize, SendBuffer.Length);
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
                else
                    return -2;
            }
            else
            {
                Dispose();
                return -1;
            }
        }

        public void SendPacket(object packet)
        {
            packetSerializer.Serialize(packet);
            while (true)
            {
                int result = packetSerializer.GetSerializedSegment(SendBuffer);

                if (result == 0)
                {
                    break;
                }

                SendData();
            }
            packetSerializer.Clear();
        }

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
            OnClientDisposed?.Invoke(Id);
            tcpClient.Dispose();
            m_networkStream.Close();
            packetSerializer = null;
        }
    }
}
