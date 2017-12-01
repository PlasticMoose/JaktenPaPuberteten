using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	public float speed = 5f;

	private void Update() {
		float step = speed * Time.deltaTime;
		transform.Translate(new Vector3(0f, 0f, -1f) * step);
	}
}
