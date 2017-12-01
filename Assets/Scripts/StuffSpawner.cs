using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffSpawner : MonoBehaviour {
	public float lifetime = 30f;
	public float interval = 1f;
	public GameObject[] stuff;

	private float lastUpdate = 0f;

	public void Awake() {
		Random.InitState(GameObject.FindObjectOfType<NetworkManager>().lobbyId.GetHashCode());
		lastUpdate = Time.time;
	}
	
	void Update() {
		float since = Time.time - lastUpdate;
		
		if(since >= interval) {
			int index = Random.Range(0, stuff.Length);
			Vector3 rndPosWithin = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);
			GameObject ting = Instantiate(stuff[index], rndPosWithin, transform.rotation) as GameObject;
			ting.AddComponent<Movement>();
			Destroy(ting, lifetime);
			lastUpdate = Time.time;
		}
	}
}
