using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FileInputAdapter : InputAdapterComponent {
    public TextAsset inputs;
    public Game game;

    private Queue<string> text;
    private int currentFrame = 0;
    private int nextFrame = 0;
    private Dictionary<string, int> inputsState = new Dictionary<string, int>();

    public override bool GetAxisNegative(string name) => GetState(name) == -1;

    public override bool GetAxisPositive(string name) => GetState(name) == 1;

    public override bool GetButton(string name) => GetState(name) == 1;

    private int GetState(string name)
    {
        int res = 0;
        if (inputsState.TryGetValue(name, out res))
            return res;
        return 0;
    }

    private void Awake()
    {
        gameObject.SetActive(false);
        if (game != null)
            game.onGameStart.AddListener(() => gameObject.SetActive(true));
    }

    // Use this for initialization
    void Start ()
    {
        text = new Queue<string>(inputs.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
	}

    private void ReadNextFrame()
    {
        if (!text.Any())
        {
            nextFrame = int.MaxValue;
            return;
        }
        var line = text.Dequeue();
        var tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        if (!tokens.Any())
        {
            ReadNextFrame();
            return;
        }
        nextFrame = int.Parse(tokens[0]);
        foreach (var input in tokens.Skip(1).Where((x, i) => (i % 2) == 0).Zip(tokens.Skip(1).Where((x, i) => (i % 2) == 1).Select(x => int.Parse(x)), (n, v) => new KeyValuePair<string, int>(n, v)))
        {
            inputsState[input.Key] = input.Value;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        ++currentFrame;
        while (currentFrame < nextFrame)
            ReadNextFrame();
	}
}
