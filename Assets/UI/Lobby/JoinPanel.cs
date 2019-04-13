using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinPanel : MonoBehaviour {
    public string ip { get; set; }
    public Menus menus;
    public ClientLobby lobby;

	public void Connect()
    {
        IConnection connection = null;
        // TODO
        lobby.Connection = connection;
        menus.SetActivePanel(lobby.gameObject);
    }
}
