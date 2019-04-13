using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterPlayerController : IPlayerController
{
    private IPlayerController Controller;

    public MasterPlayerController(IPlayerController controller)
    {
        Controller = controller;
    }

    public PlayerControls GetControls()
    {
        return Controller.GetControls();
    }

	public void UpdateControls()
    {
        Controller.UpdateControls();
    }
}
