using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
	private IDictionary<string, UnityEvent> eventDictionary;

	private static EventManager eventManager;
	private static bool ApplicationIsQuitting = false;

	public IDictionary<string, UnityEvent> EventDictionary {
		get {
			return eventDictionary;
		}
	}

	public static EventManager instance {
		get {
			//prevent the creation of an instance after the object has been destroyed
			if (ApplicationIsQuitting)
				return null;

			if (eventManager == null)
			{
				//try to find a eventManager in the hierarchy
				eventManager = GameObject.FindObjectOfType<EventManager> ();

				if (eventManager == null)
				{
					GameObject container = new GameObject ("EventManager");
					eventManager = container.AddComponent<EventManager> ();
					eventManager.initialize ();
				}
			}
			eventManager.initialize ();
			return eventManager;
		}
	}


	private void initialize ()
	{
		//create an event dictionary if none exists
		if (eventDictionary == null)
			eventDictionary = new Dictionary<String, UnityEvent> ();
	

	}

	private void OnDestroy ()
	{
		ApplicationIsQuitting = true;
	}

	//starts listening for events
	public void StartListening (string eventName, UnityAction listener)
	{
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.AddListener (listener);

		} else
		{
			thisEvent = new UnityEvent ();
			thisEvent.AddListener (listener);
			instance.eventDictionary.Add (eventName, thisEvent);
 
		}
	}

	//stops listening for events
	public void StopListening (string eventName, UnityAction listener)
	{
		if (eventManager == null)
			return;
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.RemoveListener (listener);
		}
	}

	//triggers an event
	public void TriggerEvent (string eventName, bool debug = false)
	{
		UnityEvent thisEvent = null;

		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			if (debug)
				Debug.Log (eventName);
			thisEvent.Invoke ();
		}
	}
}
