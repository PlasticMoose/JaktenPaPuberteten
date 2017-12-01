using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class PlayerController : MonoBehaviour
{
	public SocketIOComponent socket;
	LaneController laneController;
	public Player thisPlayer;
	float timer;
	float cooldownTime = 0.5f;
	public bool RemotePlayer;

	// Use this for initialization
	void Start ()
	{
		thisPlayer = GetComponent<Player> ();
		laneController = FindObjectOfType<LaneController> ();
		socket = FindObjectOfType<SocketIOComponent> ();
		socket.On ("initpos", laneController.MovePlayer);

		if (!RemotePlayer)
		{
			//tell the server that the game has loaded
			JSONObject json = new JSONObject ();
			json.AddField ("lobbyid", thisPlayer.matchID);

			socket.Emit ("gameloaded", json);
		}
		/*if (thisPlayer.playerID == 0)
			laneController.MovePlayer (thisPlayer.playerID, 0);
		else if (thisPlayer.playerID == 1)
			laneController.MovePlayer (thisPlayer.playerID, 3);*/
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;
		if (Input.GetKeyDown (KeyCode.A))
			Jump (-1);
		else if (Input.GetKeyDown (KeyCode.D))
			Jump (1);

		if (Input.GetKeyDown (KeyCode.Q))
			DoubleJump (-2);
		else if (Input.GetKeyDown (KeyCode.E))
			DoubleJump (2);
	}

	//makes the player jump two lanes in the given direction
	void DoubleJump (int dir)
	{
		//move two spaces over
		if (timer >= cooldownTime)
		{
			//send a json package to the server for evaluation
			Jump(dir);
			timer = 0;
		}
	}

	//makes the player jump one lane in the given direction
	void Jump (int dir)
	{	
		if (!RemotePlayer)
		{
			int newPosID = 0;
			//move one space over
			if (dir == 1)
			{
				newPosID = thisPlayer.posID + 1;
			} else
			{
				newPosID = thisPlayer.posID - 1;
			}

			if (newPosID >= 0 && newPosID <= 3)
			{
				//laneController.MovePlayer (thisPlayer.playerID, newPosID);
			}

			//send a json package to the server for evaluation
			JSONObject json = new JSONObject ();

			json.AddField ("lobbyid", thisPlayer.matchID);
			json.AddField ("direction", dir);

			socket.Emit ("move", json);
		}
	}
}
