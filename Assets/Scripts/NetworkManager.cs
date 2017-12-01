using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
		EventManager.instance.StartListening ("MatchLost", EndGame);
		EventManager.instance.StartListening ("MatchWon", EndGame);
	}

	void OnDestroy ()
	{
		if (EventManager.instance != null)
		{
			EventManager.instance.StopListening ("SearchForGame", QueForMatch);
			EventManager.instance.StopListening ("AbortMM", AbortMM);
			EventManager.instance.StopListening ("MatchLost", EndGame);
			EventManager.instance.StopListening ("MatchWon", EndGame);
		}
		socket.Off ("moved", OnMoved);
		socket.Off ("matchfound", OnMatchFound);
		socket.Off ("init", OnInit); 
		socket.Off ("timeout", OnTimeOut);
		socket.Off ("wall", OnWall);
		socket.Off ("stopped", OnStopped);
		socket.Off ("timeout", OnTimeout);
	}

	void EndGame() {
		socket.Close();
		StartCoroutine(Reconnect());
	}

	IEnumerator Reconnect() {
		yield return new WaitForSeconds(4f);
		socket.Connect();
	}

	void Start ()
	{
		#if UNITY_STANDALONE
		Screen.SetResolution(480, 853, false, 60);
		#elif UNITY_IOS
		Screen.orientation = ScreenOrientation.Portrait;
		#elif UNITY_ANDROID
		Screen.orientation = ScreenOrientation.Portrait;
		#endif
		socket = GetComponent <SocketIOComponent> ();
		socket.On ("moved", OnMoved);
		socket.On ("matchfound", OnMatchFound);
		socket.On ("init", OnInit); 
		socket.On ("timeout", OnTimeOut);
		socket.On ("wall", OnWall);
		socket.On ("stopped", OnStopped);
		socket.On ("timeout", OnTimeout);

	
	}

	void Update ()
	{
		
	}

	void OnStopped(SocketIOEvent obj) {
		EventManager.instance.TriggerEvent("MatchWon");
	}

	void OnTimeout(SocketIOEvent obj) {
		EventManager.instance.TriggerEvent("Menu");
	}

	void OnWall(SocketIOEvent obj)
	{
		//int wallNumber = int.Parse(obj.data["wall"].str);
		GameObject wall = Instantiate(wallPrefab, wallSpawnPoint, Quaternion.identity) as GameObject;
		Scene gameScene = SceneManager.GetSceneByBuildIndex(2);
		if(gameScene.isLoaded) {
			SceneManager.MoveGameObjectToScene(wall, gameScene);
		} else {
			Destroy(wall);
			return;
		}
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

		//Debug.Log ("MatchFound");

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
