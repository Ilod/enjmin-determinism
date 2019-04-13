using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAdapter : InputAdapterComponent
{
    public int player;

    public override bool GetAxisPositive(string name)
    {
        return Input.GetAxis($"{name}{player}") > .2f;
    }

    public override bool GetAxisNegative(string name)
    {
        return Input.GetAxis($"{name}{player}") < -.2f;
    }

    public override bool GetButton(string name)
    {
        return Input.GetButtonDown($"{name}{player}");
    }
}
