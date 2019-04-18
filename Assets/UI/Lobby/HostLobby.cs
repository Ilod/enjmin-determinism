using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HostLobby : BaseLobby
{
    private IServer Server;
    public Game Game;
    public LocalPlayerControllers controllers;

    void Start()
    {
        Server = CreateServerConnection();
	}
	
	void Update()
    {
        IConnection connection = null;
        if (Server.TryListen(out connection))
        {
            OnPlayerJoined(connection);
        }
	}

    private IServer CreateServerConnection()
    {
        return ConnectionFactory.CreateServer(Game.port, Game.connectionType);
    }
    
    private void OnPlayerJoined(IConnection connection)
    {
        var panel = GetComponentsInChildren<RemotePanel>().First(p => p.Connection == null);
        panel.SetConnection(connection);
        panel.SetColor(Color.red);
        Color color = GetComponentInChildren<HostPanel>().GetComponentInChildren<ColorWheelControl>().Selection;
        connection.Send(new PacketBuilder().Write((int)MessageType.SetColor).Write(color.r).Write(color.g).Write(color.b).Build());
    }

    public void SetColor(Color color)
    {
        var panelsRoot = gameObject.transform.parent.gameObject;

        foreach (var remotePanel in panelsRoot.GetComponentsInChildren<RemotePanel>())
        {
            if (remotePanel.Connection != null)
            {
                remotePanel.Connection.Send(new PacketBuilder().Write((int)MessageType.SetColor).Write(color.r).Write(color.g).Write(color.b).Build());
            }
        }
    }


    public override void LaunchGame()
    {
        List<PlayerInfo> players = new List<PlayerInfo>();
        var panelsRoot = gameObject.transform.parent.gameObject;

        var hostPanel = panelsRoot.GetComponentInChildren<HostPanel>();

        var connections = new List<IConnection>();
        foreach (var remotePanel in panelsRoot.GetComponentsInChildren<RemotePanel>())
        {
            if (remotePanel.Connection != null)
            {
                players.Add(new PlayerInfo(inputAdapter: new RemotePlayerController(remotePanel.Connection)
                    , color: remotePanel.Color
                    , index: remotePanel.GetComponent<PlayerPanelInfo>().playerIndex));

                // Game will start
                remotePanel.Connection.Send(new PacketBuilder()
                    .Write((int)MessageType.StartGame)
                    .Build());
                
                connections.Add(remotePanel.Connection);
            }
        }

        players.Add(new PlayerInfo(inputAdapter: new MasterPlayerController(controllers.localPlayerControllers[0], connections),
            color: hostPanel.GetComponentInChildren<ColorWheelControl>().Selection,
            index: hostPanel.GetComponent<PlayerPanelInfo>().playerIndex));

        Game.StartGame(players);
    }
}
