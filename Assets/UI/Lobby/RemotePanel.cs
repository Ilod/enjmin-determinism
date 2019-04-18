using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePanel : MonoBehaviour
{
    public IConnection Connection { get; private set; }
    public Color Color;
    public BaseLobby Lobby;

    public void SetConnection(IConnection connection)
    {
        Connection = connection;
        GetComponentInChildren<SpritePreview>(true).gameObject.SetActive(true);
    }

    public void SetColor(Color color)
    {
        Color = color;
        GetComponentInChildren<SpritePreview>().SetColor(color);
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Connection == null)
            return;
        Packet packet;
        while (Connection.TryRead(out packet))
        {
            switch ((MessageType)packet.ReadInt())
            {
                case MessageType.SetColor:
                    SetColor(new Color(packet.ReadFloat(), packet.ReadFloat(), packet.ReadFloat()));
                    break;
                case MessageType.StartGame:
                    Lobby.LaunchGame();
                    return;
            }
        }
    }
}
