using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputAdapter
{
    bool GetAxisPositive(string name);
    bool GetAxisNegative(string name);
    bool GetButton(string name);
}

public abstract class InputAdapterComponent : MonoBehaviour, IInputAdapter
{
    public abstract bool GetAxisNegative(string name);
    public abstract bool GetAxisPositive(string name);
    public abstract bool GetButton(string name);
}