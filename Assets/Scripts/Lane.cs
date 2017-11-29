using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lane
{

	public float lanePos;
	public bool laneEmpty;

	public Lane(float _lanePos, bool _laneEmpty = true)
	{
		lanePos = _lanePos;
		laneEmpty = _laneEmpty;
	}
}
