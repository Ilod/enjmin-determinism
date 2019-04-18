using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLobby : BaseLobby {
    public IConnection Connection;
    public Game Game;
    public LocalPlayerControllers controllers;

	// Use this for initialization
	void Start ()
    {
        var panel = GetComponentInChildren<RemotePanel>();
        panel.SetConnection(Connection);
        panel.SetColor(Color.red);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetColor(Color color)
    {
        Connection.Send(new PacketBuilder().Write((int)MessageType.SetColor).Write(color.r).Write(color.g).Write(color.b).Build());
    }

    public override void LaunchGame()
    {
        List<PlayerInfo> players = new List<PlayerInfo>();

        var clientPanel = GetComponentInChildren<ClientPanel>();

        var connections = new List<IConnection>();
        foreach (var remotePanel in GetComponentsInChildren<RemotePanel>())
        {
            if (remotePanel.Connection != null)
            {
                players.Add(new PlayerInfo(inputAdapter: new RemotePlayerController(remotePanel.Connection)
                    , color: remotePanel.Color
                    , index: remotePanel.GetComponent<PlayerPanelInfo>().playerIndex));
                connections.Add(remotePanel.Connection);
            }
        }

        players.Add(new PlayerInfo(inputAdapter: new MasterPlayerController(controllers.localPlayerControllers[0], connections),
            color: clientPanel.GetComponentInChildren<ColorWheelControl>().Selection,
            index: clientPanel.GetComponent<PlayerPanelInfo>().playerIndex));
        Game.StartGame(players);
    }
}
