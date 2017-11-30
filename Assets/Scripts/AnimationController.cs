using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	Animator anim;
	float timer;

	// Use this for initialization
	void Start () {
		anim = GameObject.FindWithTag ("Player").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		print (timer);
		timer += Time.deltaTime;
		
		if (Input.GetKey (KeyCode.Alpha1) && timer > 2.5f) 
		{
			anim.SetInteger ("PoseInt", 1);
			print (1);
			timer = 0;
		}

		if (Input.GetKey (KeyCode.Alpha2) && timer > 2.5f) 
		{
			anim.SetInteger ("PoseInt", 2);
			print (2);
			timer = 0;
		}


		if (Input.GetKey (KeyCode.Alpha3) && timer > 2.5f) 
		{
			anim.SetInteger ("PoseInt", 3);
			print (3);
			timer = 0;
		}


		if (Input.GetKey (KeyCode.Alpha4) && timer > 2.5f) 
		{
			anim.SetInteger ("PoseInt", 4);
			print (4);
			timer = 0;
		}

		if (timer > 2) 
		{
			anim.SetInteger ("PoseInt", 0);
		}

	}
}
