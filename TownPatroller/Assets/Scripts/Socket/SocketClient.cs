using System.Net.Sockets;
using TPPacket.Serializer;

namespace TownPatroller.SocketServer
{
    public interface IClientSender
    {
        void SendPacket(object packet);
        void Dispose();
    }

    public class SocketClient : IClientSender
    {
        public delegate void ClientDisposed(ulong Id);
        public event ClientDisposed OnClientDisposed;

        public delegate void PreClientDisposed(SocketClient socketClient);
        public event PreClientDisposed OnPreClientDisposed;

        public ulong Id { get; private set; }
        public bool IsPatrollBot { get; private set; }
        public bool IsReady { get; private set; }
        public NetworkStream NetworkStream { get; private set; }
        public byte[] ReadBuffer;
        public byte[] SendBuffer;
        public int SegmentSize { get; private set; }

        public TcpClient TcpClient { get; private set; }

        private PacketSerializer _packetSerializer;

        public SocketClient(TcpClient tcpClient, int segmentSize, int BufferSize)
        {
            Id = 0;

            SegmentSize = segmentSize;
            ReadBuffer = new byte[SegmentSize];
            SendBuffer = new byte[SegmentSize];

            TcpClient = tcpClient;

            NetworkStream = TcpClient.GetStream();

            CheckReady();

            _packetSerializer = new PacketSerializer(BufferSize, SendBuffer.Length);
        }

        public void Update2Client(ulong _Id, bool _IsPatrollBot)
        {
            Id = _Id;
            IsPatrollBot = _IsPatrollBot;

            CheckReady();
        }

        private void CheckReady()
        {
            if(Id != 0 || TcpClient != null)
            {
                IsReady = true;
            }
            else
            {
                IsReady = false;
            }
        }

        public int ReadStream()
        {
            if (TcpClient.Connected)
            {
                if (0 < TcpClient.Available)
                {
                    int remaining = SegmentSize;
                    int offset = 0;

                    while(remaining > 0)
                    {
                        var readBytes = NetworkStream.Read(ReadBuffer, offset, remaining);
                        if (readBytes == 0)
                        {
                            throw new System.Exception("disconnected");
                        }
                        offset += readBytes;
                        remaining -= readBytes;
                    }

                    return 0;
                }
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
            _packetSerializer.Serialize(packet);
            while (true)
            {
                int result = _packetSerializer.GetSerializedSegment(SendBuffer);

                if (result == 0)
                {
                    break;
                }

                SendData();
            }
            _packetSerializer.Clear();
        }

        private void SendData()
        {
            NetworkStream.Write(SendBuffer, 0, SendBuffer.Length);
            NetworkStream.Flush();

            for (int i = 0; i < SendBuffer.Length; i++)
            {
                SendBuffer[i] = 0; 
            }
        }

        public void Dispose()
        {
            if (Id == 0)
            {
                OnPreClientDisposed?.Invoke(this);
            }
            else
            {
                OnClientDisposed?.Invoke(Id);
            }
            TcpClient.Dispose();
            NetworkStream.Close();
            _packetSerializer = null;
        }
    }
}
