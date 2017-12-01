using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateDistance : MonoBehaviour {
	public Text text;
	public float speed = 5;

	private float startTime = 0f;

	void Start() {
		text = GetComponent<Text>();
		startTime = Time.time;
	}
	
	void Update() {
		float dist = (Time.time - startTime) * speed;
		text.text = dist.ToString("0.0") + "m";
	}
}
