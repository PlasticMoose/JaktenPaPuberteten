using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	public Sprite emptyHeart;
	public Player localPlayer;
	public int playerID = 0;

	private int me = 0;
	private int other = 0;

	void OnEnable() {
		EventManager.instance.StartListening("MeHit", MeHit);
		EventManager.instance.StartListening("OtherHit", OtherHit);
	}

	void OnDestroy() {
		if(EventManager.instance != null) {
			EventManager.instance.StopListening("MeHit", MeHit);
			EventManager.instance.StopListening("OtherHit", OtherHit);
		}
	}

	void MeHit() {
		if(localPlayer.playerID != playerID)
			return;
		me++;
		transform.GetChild(3 - me).GetComponent<Image>().sprite = emptyHeart;
	}

	void OtherHit() {
		if(localPlayer.playerID == playerID)
			return;
		other++;
		transform.GetChild(3 - other).GetComponent<Image>().sprite = emptyHeart;
	}
}
