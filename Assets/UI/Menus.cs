using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour {
    public GameObject MainMenuPanel;
    public GameObject LocalLobbyPanel;
    public GameObject JoinPanel;
    public GameObject ClientLobbyPanel;
    public GameObject ScorePanel;
    public GameObject HostLobbyPanel;

    // Use this for initialization
    void Start()
    {
        SetActivePanel(MainMenuPanel);
    }

    public void LocalGame()
    {
        SetActivePanel(LocalLobbyPanel);
    }

    public void StartJoinGameFlow()
    {
        SetActivePanel(JoinPanel);
    }

    public void HostGame()
    {
        SetActivePanel(HostLobbyPanel);
    }

    public void BackToMainMenu()
    {
        SetActivePanel(MainMenuPanel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void HideAll()
    {
        MainMenuPanel.SetActive(false);
        JoinPanel.SetActive(false);
        LocalLobbyPanel.SetActive(false);
        ScorePanel.SetActive(false);
        ClientLobbyPanel.SetActive(false);
        HostLobbyPanel.SetActive(false);
    }

    public void SetActivePanel(GameObject panel)
    {
        HideAll();
        panel.SetActive(true);
    }
}
