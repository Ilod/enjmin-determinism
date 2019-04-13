using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LocalPlayerController : IPlayerController
{
    public InputAdapterComponent Input;
    public PlayerControls Controls = new PlayerControls();

    public PlayerControls GetControls()
    {
        return Controls;
    }

    public void UpdateControls()
    {
        Controls.Up = Input.GetAxisPositive("Vertical");
        Controls.Down = Input.GetAxisNegative("Vertical");
        Controls.Left = Input.GetAxisNegative("Horizontal");
        Controls.Right = Input.GetAxisPositive("Horizontal");
        Controls.Shoot = Input.GetButton("Shoot");
    }
}
