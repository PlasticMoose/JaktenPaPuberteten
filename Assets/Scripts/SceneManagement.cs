using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
	void OnEnable ()
	{
		EventManager.instance.StartListening ("MatchFound", LoadGame);
		EventManager.instance.StartListening ("MatchLost", LostGame);
		EventManager.instance.StartListening ("MatchWon", WonGame);
		EventManager.instance.StartListening ("Menu", ReloadMenu);
	}

	void OnDestroy ()
	{
		if (EventManager.instance != null) {
			EventManager.instance.StopListening ("MatchFound", LoadGame);
			EventManager.instance.StopListening ("MatchLost", LostGame);
			EventManager.instance.StopListening ("MatchWon", WonGame);
			EventManager.instance.StopListening ("Menu", ReloadMenu);
		}
	}

	void Start ()
	{
		SceneManager.LoadScene (1, LoadSceneMode.Additive);
	}

	void LostGame() {
		SceneManager.LoadScene (3, LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync (2);
		StartCoroutine(backtoMainMenu(3));
	}

	void WonGame() {
		SceneManager.LoadScene (4, LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync (2);
		StartCoroutine(backtoMainMenu(4));
	}

	void ReloadMenu() {
		SceneManager.LoadScene (1, LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync (1);
	}

	IEnumerator backtoMainMenu(int scene) {
		yield return new WaitForSeconds(5f);
		SceneManager.LoadScene (1, LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync(scene);
		yield break;
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
