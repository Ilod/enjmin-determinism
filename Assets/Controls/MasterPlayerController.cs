using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MasterPlayerController : IPlayerController
{
    private IPlayerController Controller;
    private List<IConnection> Connections;

    public MasterPlayerController(IPlayerController controller, IEnumerable<IConnection> connections)
    {
        Controller = controller;
        Connections = connections.ToList();
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
