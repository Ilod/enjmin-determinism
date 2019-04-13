using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class ConnectionFactory
{
    public static IServer CreateServer(int port, ConnectionType type)
    {
        switch (type)
        {
            case ConnectionType.Tcp:
                return new TcpServer(port);
            case ConnectionType.Udp:
                return new UdpServer(port);
            default:
                throw new ArgumentException($"Unknown type {type}");
        }
    }
    public static IConnection Connect(string address, int port, ConnectionType type)
    {
        switch (type)
        {
            case ConnectionType.Tcp:
                return new TcpConnection(address, port);
            case ConnectionType.Udp:
                return new UdpConnection(address, port);
            default:
                throw new ArgumentException($"Unknown type {type}");
        }
    }
}
