using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public static partial class ConnectionFactory
{
    private sealed class TcpConnection : IConnection
    {
        private readonly System.Net.Sockets.TcpClient Socket;
        private byte[] currentData;
        private enum State
        {
            Idle,
            ReadingSize,
            ReadingPacket,
        };
        private State state;
        private int currentIdx;

        public TcpConnection(string address, int port)
        {
            Socket = new TcpClient();
            Socket.Connect(address, port);
        }

        public TcpConnection(TcpClient client)
        {
            Socket = client;
        }

        private bool ReadData(bool block)
        {
            while (currentIdx != currentData.Length)
            {
                if (!block && !Socket.GetStream().DataAvailable)
                    return false;
                int read = Socket.GetStream().Read(currentData, currentIdx, currentData.Length);
                if (read == 0)
                    throw new IOException("Socket closed");
                currentIdx += read;
            }
            return true;
        }

        private Packet Read(bool block)
        {
            if (state == State.Idle)
            {
                currentData = new byte[4];
                currentIdx = 0;
                state = State.ReadingSize;
            }
            if (state == State.ReadingSize)
            {
                if (!ReadData(block))
                    return null;
                int size = BitConverter.ToInt32(currentData, 0);
                currentIdx = 0;
                currentData = new byte[size];
                state = State.ReadingPacket;
            }
            if (state == State.ReadingPacket)
            {
                if (!ReadData(block))
                    return null;
                state = State.Idle;
            }
            return new Packet(currentData);
        }

        public Packet Read() => Read(true);

        public void Send(Packet packet)
        {
            Socket.GetStream().Write(BitConverter.GetBytes(packet.Size), 0, 4);
            Socket.GetStream().Write(packet.Data, 0, packet.Size);
        }

        public bool TryRead(out Packet packet)
        {
            packet = Read(false);
            return packet != null;
        }

        public void Dispose()
        {
            Socket.Dispose();
        }
    }
}
