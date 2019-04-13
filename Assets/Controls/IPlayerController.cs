﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerController
{
    PlayerControls GetControls();
    void UpdateControls();
}
