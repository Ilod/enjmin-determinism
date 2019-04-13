using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LocalStart : MonoBehaviour {
    private Button button;
    public GameObject panels;
    public Game Game;
    public LocalPlayerControllers controllers;

	// Use this for initialization
	void Start ()
    {
        button = GetComponent<Button>();
        button.interactable = panels.GetComponentsInChildren<Toggle>().Any(toggle => toggle.isOn);
	}
	
	// Update is called once per frame
	void Update ()
    {
        button.interactable = panels.GetComponentsInChildren<Toggle>().Any(toggle => toggle.isOn);
    }

    public void LaunchGame()
    {
        List<PlayerInfo> players = new List<PlayerInfo>();
        foreach (var panelInfo in panels.GetComponentsInChildren<PlayerPanelInfo>())
        {
            if (panelInfo.GetComponentInChildren<Toggle>().isOn)
            {
                players.Add(new PlayerInfo(inputAdapter: controllers.localPlayerControllers[panelInfo.playerIndex]
                    , color: panelInfo.GetComponentInChildren<ColorWheelControl>().Selection
                    , index: panelInfo.playerIndex));
            }
        }
        Game.StartGame(players);
    }
}
