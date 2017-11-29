using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEmitter : MonoBehaviour
{

	public void EmitStartSearchForGame ()
	{
		EventManager.instance.TriggerEvent ("SearchForGame");
	}

	public void EmitAbortSearch ()
	{
		EventManager.instance.TriggerEvent ("AbortMM");
	}
}
