using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class PlayerController : MonoBehaviour
{
	public SocketIOComponent socket;
	LaneController laneController;
	public Player thisPlayer;
	float timer;
	float cooldownTime = 0.5f;
	public bool RemotePlayer;

	public GameObject[] models;
	public Animator[] animators;
	public Image indicator;

	private AnimationController animationController;

	void OnDestroy ()
	{
		socket.Off ("initpos", laneController.MovePlayer);
		socket.Off ("posed", OnPosed);
		socket.Off ("hit", OnHit);
	}

	// Use this for initialization
	void Start ()
	{
		animationController = GetComponent<AnimationController>();
		thisPlayer = GetComponent<Player> ();
		laneController = FindObjectOfType<LaneController> ();
		socket = FindObjectOfType<SocketIOComponent> ();
		socket.On ("initpos", laneController.MovePlayer);
		socket.On ("posed", OnPosed);
		socket.On ("hit", OnHit);

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
		models[0].SetActive(thisPlayer.playerID == 0);
		models[1].SetActive(thisPlayer.playerID == 1);
		if(animationController != null)
			animationController.anim = animators[thisPlayer.playerID];

		if(indicator != null)
			indicator.color = thisPlayer.playerID == 1 ? Color.red : Color.blue;
		
		timer += Time.deltaTime;
		Touch();
		#if UNITY_IOS
			Touch();
		#elif UNITY_ANDROID
			Touch();
		#elif UNITY_STANDALONE
		if (Input.GetKeyDown (KeyCode.A))
			Jump (-1);
		else if (Input.GetKeyDown (KeyCode.D))
			Jump (1);

		if (Input.GetKeyDown (KeyCode.Q))
			DoubleJump (-2);
		else if (Input.GetKeyDown (KeyCode.E))
			DoubleJump (2);
		#endif
	}

	private bool swiping, eventSent;
	private Vector2 lastPosition;
	private float lastTap = 0f;
	private float lastRelease = 0f;

	void Touch() {
		if (Input.touchCount == 0) {
			if(lastRelease == 0f)
				lastRelease = Time.time - Time.deltaTime;
            return;
		}
		
		if((Time.time - lastTap) <= .3f && lastRelease != 0f && Input.touchCount == 1) {
			if(Input.GetTouch(0).position.x < (Screen.width / 2f)) {
				DoubleJump(-2);
			} else {
				DoubleJump(2);
			}
		}

        if (Input.GetTouch(0).deltaPosition.sqrMagnitude >= 100){
            if (swiping == false){
                swiping = true;
                lastPosition = Input.GetTouch(0).position;
                return;
            } else {
                if (!eventSent) {
                    Vector2 direction = Input.GetTouch(0).position - lastPosition;
 
                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
                        if (direction.x > 0) 
                            Jump (1);
                        else
                            Jump (-1);
					} /*else {
                        if (direction.y > 0)
                            DoubleJump(2);
                        else
                            DoubleJump(-2);
                    }*/
 
                    eventSent = true; 
				}
            }
        } else {
            swiping = false;
            eventSent = false;
        }
		lastRelease = 0f;
		lastTap = Time.time;
	}

	void OnPosed(SocketIOEvent obj) {
		if(RemotePlayer) {
			animationController.ChangeRemotePose(int.Parse(obj.data["pose"].str) + 1);
		}
	}

	void OnHit(SocketIOEvent obj) {
		if(!RemotePlayer) {
			int who = int.Parse(obj.data["who"].str);
			int hits = int.Parse(obj.data["hit"].str);
			if(who == thisPlayer.playerID) {
				EventManager.instance.TriggerEvent("MeHit");
				if(hits >= 3) {
					EventManager.instance.TriggerEvent("MatchLost");
				}
			} else {
				EventManager.instance.TriggerEvent("OtherHit");
				if(hits >= 3) {
					EventManager.instance.TriggerEvent("MatchWon");
				}
			}
		}
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
