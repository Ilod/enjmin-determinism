using System.Collections;
using System.Collections.Generic;
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
		// TODO
	}

    private IServer CreateServerConnection()
    {
        // TODO
        return null;
    }
    
    private void OnPlayerJoined(IConnection connection)
    {
        // TODO
    }


    public override void LaunchGame()
    {
        List<PlayerInfo> players = new List<PlayerInfo>();
        var panelsRoot = gameObject.transform.parent.gameObject;

        var hostPanel = panelsRoot.GetComponentInChildren<HostPanel>();
        players.Add(new PlayerInfo(inputAdapter: new MasterPlayerController(controllers.localPlayerControllers[0]),
            color: hostPanel.GetComponentInChildren<ColorWheelControl>().Selection,
            index: hostPanel.GetComponent<PlayerPanelInfo>().playerIndex));

        foreach (var remotePanel in panelsRoot.GetComponentsInChildren<RemotePanel>())
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
