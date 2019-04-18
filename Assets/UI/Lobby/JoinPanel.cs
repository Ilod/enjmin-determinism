using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinPanel : MonoBehaviour {
    public string ip { get; set; }
    public Menus menus;
    public ClientLobby lobby;

	public void Connect()
    {
        var game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        IConnection connection =
            ConnectionFactory.Connect(ip,
            game.port,
            game.connectionType);
        lobby.Connection = connection;
        menus.SetActivePanel(lobby.gameObject);
    }
}
