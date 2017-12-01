using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class LaneController : MonoBehaviour
{
	NetworkManager netManager;
	public SocketIOComponent socket;
	public Player[] players = new Player[2];
	
	public Lane[] lanes = new Lane[4];

	void Awake ()
	{
		Player[] unSortedPlayes = FindObjectsOfType<Player> ();
		
		for (int i = 0; i <= 1; i++)
		{
			if (unSortedPlayes [i].playerID == 0)
				players [0] = unSortedPlayes [i];
			else if (unSortedPlayes [i].playerID == 1)
				players [1] = unSortedPlayes [i];
				
		}

		netManager = FindObjectOfType<NetworkManager> ();
		netManager.FUCKINGOODJOBMATIAS = this;
	}

	public void Update ()
	{
		for (int i = 0; i <= 3; i++)
		{
			if (players [0].posID == i || players [1].posID == i)
				lanes [i].laneEmpty = false;
			else
				lanes [i].laneEmpty = true;
			
		}
	}

	//takes orders from server
	public void MovePlayer (int playerID, int posId)
	{	
		bool valid = false;

		if (lanes [posId].laneEmpty)
			valid = true;

		if (valid)
		{
			players [playerID].posID = posId;
			float newXPos = PosIdToPos (posId);
			Vector3 playerPos = players [playerID].transform.position;
			players [playerID].transform.position = new Vector3 (newXPos, playerPos.y, playerPos.z);
 			
		}
	}

	public void MovePlayer (SocketIOEvent obj)
	{	
		int posID = int.Parse (obj.data ["pos"].str);
		int playerID = int.Parse (obj.data ["who"].str);

		players [playerID].posID = posID;
		float newXPos = PosIdToPos (posID);
		Vector3 playerPos = players [playerID].transform.position;
		players [playerID].transform.position = new Vector3 (newXPos, playerPos.y, playerPos.z);


	}

	//translates a posId to a 1D position
	float PosIdToPos (int posId)
	{
		if (posId == 0)
		{
			return -2f;
		} else if (posId == 1)
		{
			return -0.65f;
		} else if (posId == 2)
		{
			return 0.65f;
		} else if (posId == 3)
		{
			return 2f;
		} else
			return -5;
	}
}
