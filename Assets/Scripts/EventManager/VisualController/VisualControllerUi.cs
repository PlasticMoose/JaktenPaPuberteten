using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualControllerUi : MonoBehaviour
{
	public Canvas MainMenu;
	public Canvas Searching;
	public Canvas TimedOut;


	void OnEnable ()
	{
		EventManager.instance.StartListening ("AbortMM", OnTimeOut);
		EventManager.instance.StartListening ("SearchForGame", OnStartSearch);
	}

	void OnDestroy ()
	{
		if (EventManager.instance != null)
		{
			EventManager.instance.StopListening ("AbortMM", OnTimeOut);
			EventManager.instance.StopListening ("SearchForGame", OnStartSearch);
		}

	}

	void Start ()
	{
		ReturnMainMenu ();
	}

	void OnTimeOut ()
	{
		MainMenu.enabled = false;
		Searching.enabled = false;
		TimedOut.enabled = true;
	}

	void OnStartSearch ()
	{
		MainMenu.enabled = false;
		Searching.enabled = true;
		TimedOut.enabled = false;
	}

	public void ReturnMainMenu ()
	{
		MainMenu.enabled = true;
		Searching.enabled = false;
		TimedOut.enabled = false;
	}
}
