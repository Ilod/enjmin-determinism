using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    public Menus menus;
    public Vector2[] spawnPositions;
    public Text[] scores;
    public GameObject playerPrefab;
    public GameObject backgroundPrefab;
    public Vector2 size;
    public GameState state;

    public UnityEvent onGameStart;
    public ConnectionType connectionType = ConnectionType.Udp;
    public short port = 8888;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        state = GameState.ComputeGameState();
	}

    private void LateUpdate()
    {
        GameState frameState = null;
        int frameStateId = 0;
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var state = player.GetComponent<PlayerAI>().controller.GetControls().State;
            if (state == null)
                return;
            if (frameState == null)
            {
                frameState = state;
                frameStateId = frameState.ComputeHash();
            }
            else
            {
                if (state.ComputeHash() != frameStateId)
                {
                    throw new System.Exception($@"Different states:
{frameState}
{state}");
                }
            }
        }
    }

    public void StartGame(IEnumerable<PlayerInfo> players)
    {
        menus.SetActivePanel(menus.ScorePanel);
        Instantiate(backgroundPrefab).transform.localScale = new Vector3(size.x, size.y);

        foreach (PlayerInfo player in players)
        {
            int index = player.Index;
            Vector3 pos = new Vector3(spawnPositions[index].x, spawnPositions[index].y, 0);
            var playerObj = Instantiate(playerPrefab, pos, Quaternion.identity);
            var playerAi = playerObj.GetComponent<PlayerAI>();
            playerAi.controller = player.InputAdapter;
            playerAi.color = player.Color;
            playerAi.index = index;
            playerAi.bounds = new Rect(-size / 2, size);
            playerAi.onScoreChanged.AddListener(new UnityEngine.Events.UnityAction<int>((score) => UpdateScore(index, score)));
            scores[index].color = player.Color;
            scores[index].text = 0.ToString();
        }

        onGameStart.Invoke();
    }

    public void UpdateScore(int playerIndex, int score)
    {
        scores[playerIndex].text = score.ToString();
    }
}
