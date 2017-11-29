using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {
	public float speed = 5f;

	private int currentWall = 0;

	public bool setNewWall(int wall) {
		if(wall >= 0 && wall < transform.childCount) {
			currentWall = wall;
			updateChildren();
			return true;
		}
		return false;
	}

	private void updateChildren() {
		for(int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(i == currentWall);
		}
	}
	
	private void Update() {
		float step = speed * Time.deltaTime;
		transform.Translate(new Vector3(0f, 0f, -1f) * step);
	}
}
