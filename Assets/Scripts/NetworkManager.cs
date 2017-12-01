using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkManager : MonoBehaviour
{
	[Header("Wall Settings")]
	public GameObject wallPrefab;
	public Vector3 wallSpawnPoint = new Vector3(-2.32f, -2.387f, 19.47f);

	[Header("Other")]
	public Player LocalPlayer;
	public LaneController FUCKINGOODJOBMATIAS;

	private SocketIOComponent socket;

	public int playerId;
	public string lobbyId;
	public string mapSeed;

	void OnEnable ()
	{
		EventManager.instance.StartListening ("SearchForGame", QueForMatch);
		EventManager.instance.StartListening ("AbortMM", AbortMM);
	}

	void OnDestroy ()
	{
		if (EventManager.instance != null)
		{
			EventManager.instance.StopListening ("SearchForGame", QueForMatch);
			EventManager.instance.StopListening ("AbortMM", AbortMM);
		}
	}

	void Start ()
	{
		socket = GetComponent <SocketIOComponent> ();
		socket.On ("moved", OnMoved);
		socket.On ("matchfound", OnMatchFound);
		socket.On ("init", OnInit); 
		socket.On ("timeout", OnTimeOut);
		socket.On ("wall", OnWall);
		

	
	}

	void Update ()
	{
		
	}

	void OnWall(SocketIOEvent obj)
	{
		//int wallNumber = int.Parse(obj.data["wall"].str);
		GameObject wall = Instantiate(wallPrefab, wallSpawnPoint, Quaternion.identity) as GameObject;
		WallClass gotWall = new WallClass();
		gotWall = JsonUtility.FromJson<WallClass>(obj.data.ToString());
		wall.GetComponent<WallController>().setNewWall(int.Parse(gotWall.lanes[0]), int.Parse(gotWall.lanes[1]), int.Parse(gotWall.lanes[2]), int.Parse(gotWall.lanes[3]), gotWall.hash, socket);
	}

	void OnMoved (SocketIOEvent obj)
	{
		int who = int.Parse (obj.data ["who"].str);
		int pos_lane = int.Parse (obj.data ["pos"].str);

		FUCKINGOODJOBMATIAS.MovePlayer (obj);


	}

	void OnMatchFound (SocketIOEvent obj)
	{
		string lobbyID = obj.data ["lobbyid"].str;
		int playerID = int.Parse (obj.data ["playerNumber"].str);
		string mapseed = obj.data ["map"].str;

		playerId = playerID;
		lobbyId = lobbyID;
		mapSeed = mapseed;

		Debug.Log ("MatchFound");

		EventManager.instance.TriggerEvent ("MatchFound");
	}

	void OnInit (SocketIOEvent obj)
	{
		Debug.Log ("connected to server");
	}

	void OnTimeOut (SocketIOEvent obj)
	{
		//change ui to display failed attempt
		EventManager.instance.TriggerEvent ("TimeOut");
	}

	void AbortMM ()
	{
		socket.Emit ("abortmm");
	}

	public void QueForMatch ()
	{
		socket.Emit ("matchmake");

	}

	public class WallClass {
		public string hash;
		public string[] lanes;
		public WallClass() {
			lanes = new string[4];
		}
	}
}
