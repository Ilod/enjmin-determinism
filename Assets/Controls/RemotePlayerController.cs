using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlayerController : IPlayerController
{
    public PlayerControls Controls = new PlayerControls();
    public IConnection Connection;

    public RemotePlayerController(IConnection connection)
    {
        Connection = connection;
    }

    public PlayerControls GetControls()
    {
        return Controls;
    }

	public void UpdateControls()
    {
		// TODO
	}
}
