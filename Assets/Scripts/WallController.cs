using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class WallController : MonoBehaviour {
	public float life = 120f;
	public float speed = 5f;

	public SocketIOComponent socket;

	public string hash = "";

	public GameObject[] lanes;
	
	public void setNewWall(int w1, int w2, int w3, int w4, string _hash, SocketIOComponent _socket) {
		Destroy(gameObject, life);
		socket = _socket;
		hash = _hash;
		for(int i = 0; i < lanes[0].transform.childCount; i++) {
			lanes[0].transform.GetChild(i).gameObject.SetActive(i == w1);
		}
		for(int i = 0; i < lanes[1].transform.childCount; i++) {
			lanes[1].transform.GetChild(i).gameObject.SetActive(i == w2);
		}
		for(int i = 0; i < lanes[2].transform.childCount; i++) {
			lanes[2].transform.GetChild(i).gameObject.SetActive(i == w3);
		}
		for(int i = 0; i < lanes[3].transform.childCount; i++) {
			lanes[3].transform.GetChild(i).gameObject.SetActive(i == w4);
		}
	}
	
	private void Update() {
		float step = speed * Time.deltaTime;
		transform.Translate(new Vector3(0f, 0f, -1f) * step);
	}

	void OnTriggerEnter(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player != null) {
			JSONObject json = new JSONObject();
			json.AddField("lobbyid", player.matchID);
			json.AddField("hash", hash);
			socket.Emit("walled", json);
		}
	}

}
