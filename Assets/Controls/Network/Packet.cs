using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Packet
{
    public Packet()
    {
        m_data = new byte[0];
    }

    public Packet(IEnumerable<byte> data)
    {
        m_data = data.ToArray();
    }

    private readonly byte[] m_data;
    private int Index = 0;

    public int ReadInt() => BitConverter.ToInt32(m_data, (Index += 4) - 4);
    public long ReadLong() => BitConverter.ToInt64(m_data, (Index += 8) - 8);
    public short ReadShort() => BitConverter.ToInt16(m_data, (Index += 2) - 2);
    public sbyte ReadSignedByte() => unchecked((sbyte)m_data[Index++]);
    public uint ReadUInt() => BitConverter.ToUInt32(m_data, (Index += 4) - 4);
    public ulong ReadULong() => BitConverter.ToUInt64(m_data, (Index += 8) - 8);
    public ushort ReadUShort() => BitConverter.ToUInt16(m_data, (Index += 2) - 2);
    public char ReadChar() => BitConverter.ToChar(m_data, (Index += 2) - 2);
    public byte ReadByte() => m_data[Index++];
    public bool ReadBool() => BitConverter.ToBoolean(m_data, Index++);
    public float ReadFloat() => BitConverter.ToSingle(m_data, (Index += 4) - 4);
    public double ReadDouble() => BitConverter.ToDouble(m_data, (Index += 8) - 8);
    public string ReadString()
    {
        int size = ReadInt();
        return Encoding.UTF8.GetString(m_data, (Index += size) - size, size);
    }

    public int Size => m_data.Length;
    public byte[] Data => (byte[])m_data.Clone();
    public void Seek(int pos) => Index = pos;
    public int Position => Index;
}

public class PacketBuilder
{
    private List<byte> Data { get; } = new List<byte>();

    public PacketBuilder Write(int _val) => WriteInternal(BitConverter.GetBytes(_val));
    public PacketBuilder Write(long _val) => WriteInternal(BitConverter.GetBytes(_val));
    public PacketBuilder Write(short _val) => WriteInternal(BitConverter.GetBytes(_val));
    public PacketBuilder Write(sbyte _val) => WriteInternal(BitConverter.GetBytes(_val));
    public PacketBuilder Write(uint _val) => WriteInternal(BitConverter.GetBytes(_val));
    public PacketBuilder Write(ulong _val) => WriteInternal(BitConverter.GetBytes(_val));
    public PacketBuilder Write(ushort _val) => WriteInternal(BitConverter.GetBytes(_val));
    public PacketBuilder Write(byte _val) => WriteInternal(BitConverter.GetBytes(_val));
    public PacketBuilder Write(string _val)
    {
        Write(_val.Length);
        return WriteInternal(Encoding.UTF8.GetBytes(_val));
    }
    public PacketBuilder Write(char _val) => WriteInternal(BitConverter.GetBytes(_val));
    public PacketBuilder Write(float _val) => WriteInternal(BitConverter.GetBytes(_val));
    public PacketBuilder Write(double _val) => WriteInternal(BitConverter.GetBytes(_val));
    public PacketBuilder Write(bool _val) => WriteInternal(BitConverter.GetBytes(_val));
    private PacketBuilder WriteInternal(byte[] data)
    {
        Data.AddRange(data);
        return this;
    }

    public Packet Build() => new Packet(Data);
}
