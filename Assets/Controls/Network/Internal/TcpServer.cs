using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public static partial class ConnectionFactory
{
    private sealed class TcpServer : IServer
    {
        private readonly TcpListener ServerSocket;

        public TcpServer(int port)
        {
            ServerSocket = new TcpListener(IPAddress.Any, port);
            ServerSocket.Start();
        }

        public void Dispose()
        {
            ServerSocket.Stop();
        }

        public IConnection Listen()
        {
            return new TcpConnection(ServerSocket.AcceptTcpClient());
        }

        public bool TryListen(out IConnection connection)
        {
            if (ServerSocket.Pending())
            {
                connection = Listen();
                return true;
            }
            else
            {
                connection = null;
                return false;
            }
        }
    }

}
