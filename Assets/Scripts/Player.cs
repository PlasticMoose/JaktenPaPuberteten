using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	NetworkManager networkManager;

	//networked
	public string matchID;
	public int playerID;
	public int posID;

	public bool isLocalPlayer;

	void OnEnable ()
	{	
		networkManager = FindObjectOfType<NetworkManager> ();

		if (isLocalPlayer)
		{
			matchID = networkManager.lobbyId;
			playerID = networkManager.playerId;
		} else
		{
			if (networkManager.playerId == 0)
				playerID = 1;
			else
				playerID = 0;
				
		}
	}
}
