using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	public float speed = 5f;

	private float realSpeed = 5f;

	private void Update() {
		realSpeed = speed + Mathf.Clamp(transform.position.z + 110f, 0f, 200f);
		float step = realSpeed * Time.deltaTime;
		transform.Translate(new Vector3(0f, 0f, -1f) * step);
	}
}
