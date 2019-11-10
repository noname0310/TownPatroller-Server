using System.Net.Sockets;
using TPPacket.Serializer;

namespace TownPatroller.SocketServer
{
    public interface IClientSender
    {
        bool SendPacket(object packet);
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
        public int FullSegmentSize { get; private set; }

        public TcpClient TcpClient { get; private set; }

        private PacketSerializer _packetSerializer;

        public SocketClient(TcpClient tcpClient, int fullSegmentSize, int segmentSize)
        {
            Id = 0;

            FullSegmentSize = fullSegmentSize;
            ReadBuffer = new byte[FullSegmentSize];//1024
            SendBuffer = new byte[FullSegmentSize];//1024

            TcpClient = tcpClient;

            NetworkStream = TcpClient.GetStream();

            CheckReady();

            _packetSerializer = new PacketSerializer(segmentSize);//1012
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
                    ReadNetworkStreamToReadBuffer(0, PacketHeaderSize.HeaderSize);
                    HeaderInfo headerInfo = PacketDeserializer.ParseHeader(ReadBuffer);
                    ReadNetworkStreamToReadBuffer(PacketHeaderSize.HeaderSize, headerInfo.SegmentLength);

                    return PacketHeaderSize.HeaderSize + headerInfo.SegmentLength;
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

        public void ReadNetworkStreamToReadBuffer(int offset, int Length)
        {
            int remainingBytes = Length;
            int Offset = offset;

            while (remainingBytes > 0)
            {
                var readBytes = NetworkStream.Read(ReadBuffer, Offset, remainingBytes);
                if (readBytes == 0)
                {
                    throw new System.Exception("disconnected");
                }
                Offset += readBytes;
                remainingBytes -= readBytes;
            }
        }

        public bool SendPacket(object packet)
        {
            if (_packetSerializer == null)
                return false;

            _packetSerializer.Serialize(packet);
            for (int i = 0; i < _packetSerializer.SegmentCount; i++)
            {
                int fullSegmentLength = _packetSerializer.GetSerializedSegment(SendBuffer);

                SendData(fullSegmentLength);
            }
            _packetSerializer.Clear();

            return true;
        }

        private void SendData(int length)
        {
            NetworkStream.Write(SendBuffer, 0, length);
            NetworkStream.Flush();
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
