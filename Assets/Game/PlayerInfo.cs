using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public IPlayerController InputAdapter { get; }
    public Color Color { get; }
    public int Index { get; }

    public PlayerInfo(IPlayerController inputAdapter, Color color, int index)
    {
        InputAdapter = inputAdapter;
        Color = color;
        Index = index;
    }
}
