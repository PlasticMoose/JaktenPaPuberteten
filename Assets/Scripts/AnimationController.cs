using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class AnimationController : MonoBehaviour {

	public Animator anim;
	private SocketIOComponent socket;
	private Player player;

	//PoseInt 0 = running, PoseInt > 1-4 = Poses

	void Start ()
	{
		socket = GameObject.FindObjectOfType<SocketIOComponent> ();
		player = GetComponent<Player> ();
	}

	public void ChangePose (int PoseInt)
	{
		SendPose (PoseInt);
		anim.SetInteger ("PoseInt", PoseInt);
	}
	public void ChangeRemotePose (int PoseInt)
	{
		anim.SetInteger ("PoseInt", PoseInt);
	}

	void SendPose (int PoseInt)
	{
		JSONObject json = new JSONObject ();
		json.AddField ("lobbyid", player.matchID);
		json.AddField ("pose", PoseInt - 1);

		socket.Emit ("pose", json);

	}

}
