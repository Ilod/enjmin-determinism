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
        GetComponentInChildren<RemotePanel>().SetConnection(Connection);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void LaunchGame()
    {
        List<PlayerInfo> players = new List<PlayerInfo>();

        var clientPanel = GetComponentInChildren<ClientPanel>();
        players.Add(new PlayerInfo(inputAdapter: new MasterPlayerController(controllers.localPlayerControllers[0]),
            color: clientPanel.GetComponentInChildren<ColorWheelControl>().Selection,
            index: clientPanel.GetComponent<PlayerPanelInfo>().playerIndex));

        foreach (var remotePanel in GetComponentsInChildren<RemotePanel>())
        {
            if (remotePanel.Connection != null)
            {
                players.Add(new PlayerInfo(inputAdapter: new RemotePlayerController(remotePanel.Connection)
                    , color: remotePanel.Color
                    , index: remotePanel.GetComponent<PlayerPanelInfo>().playerIndex));
            }
        }
        Game.StartGame(players);
    }
}
