using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public static partial class ConnectionFactory
{
    private sealed class UdpConnection : IConnection
    {
        private readonly UdpClient Socket;
        private IPEndPoint EndPoint;

        public UdpConnection(string address, int port)
        {
            EndPoint = new IPEndPoint(IPAddress.Parse(address), port);
            Socket = new UdpClient(0, AddressFamily.InterNetwork);
            Send(new PacketBuilder().Write(0).Build());
        }

        public void Dispose()
        {
            Socket.Dispose();
        }

        public Packet Read()
        {
            IPEndPoint endPoint = null;
            return new Packet(Socket.Receive(ref endPoint));
        }

        public void Send(Packet packet)
        {
            Socket.Send(packet.Data, packet.Size, EndPoint);
        }

        public bool TryRead(out Packet packet)
        {
            if (Socket.Available == 0)
            {
                packet = null;
                return false;
            }
            else
            {
                packet = Read();
                return true;
            }
        }
    }

}
