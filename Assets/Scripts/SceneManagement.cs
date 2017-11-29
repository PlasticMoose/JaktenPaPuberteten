using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
	void OnEnable ()
	{
		EventManager.instance.StartListening ("MatchFound", LoadGame);
	}

	void OnDestroy ()
	{
		if (EventManager.instance != null)
			EventManager.instance.StopListening ("MatchFound", LoadGame);
	}

	void Start ()
	{
		SceneManager.LoadScene (1, LoadSceneMode.Additive);
	}

	void LoadGame ()
	{
		SceneManager.LoadScene (2, LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync (1);
	}

	void LoadMenu ()
	{
		SceneManager.LoadScene (1, LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync (2);
	}
}
