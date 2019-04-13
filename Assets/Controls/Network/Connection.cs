using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public interface IConnection : IDisposable
{
    Packet Read();
    bool TryRead(out Packet packet);
    void Send(Packet packet);
}

public interface IServer : IDisposable
{
    IConnection Listen();
    bool TryListen(out IConnection connection);
}

public enum ConnectionType
{
    Tcp,
    Udp,
}
