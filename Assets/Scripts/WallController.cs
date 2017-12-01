using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {
	public float speed = 5f;

	public string hash = "";

	public GameObject[] lanes;
	
	public void setNewWall(int w1, int w2, int w3, int w4, string _hash) {
		Destroy(gameObject, 60f);
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
		hash = _hash;
	}
	
	private void Update() {
		float step = speed * Time.deltaTime;
		transform.Translate(new Vector3(0f, 0f, -1f) * step);
	}
}
