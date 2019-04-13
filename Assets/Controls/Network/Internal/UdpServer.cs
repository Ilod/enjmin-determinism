using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public static partial class ConnectionFactory
{
    private sealed class UdpServer : IServer
    {
        private readonly UdpClient Socket;
        private readonly Dictionary<IPEndPoint, UdpServerConnection> Connections = new Dictionary<IPEndPoint, UdpServerConnection>();

        public UdpServer(int port)
        {
            Socket = new UdpClient(port, AddressFamily.InterNetwork);
        }

        public UdpServer(IPEndPoint endPoint)
        {
            Socket = new UdpClient(endPoint);
        }

        public void Dispose()
        {
            Socket.Dispose();
        }

        public bool IsDataAvailable => Socket.Available != 0;

        public IConnection ReadData()
        {
            var endPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = Socket.Receive(ref endPoint);
            UdpServerConnection connection;
            if (Connections.TryGetValue(endPoint, out connection))
            {
                connection.HandleData(data);
                return null;
            }
            else
            {
                connection = new UdpServerConnection(this, new IPEndPoint(endPoint.Address, endPoint.Port));
                Connections[new IPEndPoint(endPoint.Address, endPoint.Port)] = connection;
                // Skip data, detection packet
                //connection.HandleData(data);
                return connection;
            }
        }

        public void Send(Packet packet, IPEndPoint endPoint)
        {
            Socket.Send(packet.Data, packet.Size, endPoint);
        }

        public IConnection Listen()
        {
            IConnection connection = null;
            do
            {
                connection = ReadData();
            } while (connection == null);
            return connection;
        }

        public bool TryListen(out IConnection connection)
        {
            connection = null;
            if (Socket.Available == 0)
                return false;
            connection = ReadData();
            return connection != null;
        }

        private sealed class UdpServerConnection : IConnection
        {
            private readonly UdpServer Server;
            private readonly IPEndPoint EndPoint;
            private readonly Queue<Packet> Packets = new Queue<Packet>();

            public UdpServerConnection(UdpServer server, IPEndPoint endPoint)
            {
                Server = server;
                EndPoint = new IPEndPoint(endPoint.Address, endPoint.Port);
            }

            public void HandleData(byte[] data)
            {
                Packets.Enqueue(new Packet(data));
            }

            public void Dispose() { }

            public Packet Read()
            {
                while (!Packets.Any())
                    Server.ReadData();
                return Packets.Dequeue();
            }

            public void Send(Packet packet)
            {
                Server.Send(packet, EndPoint);
            }

            public bool TryRead(out Packet packet)
            {
                while (!Packets.Any() && Server.IsDataAvailable)
                    Server.ReadData();
                packet = (Packets.Any() ? Packets.Dequeue() : null);
                return packet != null;
            }
        }
    }
}
