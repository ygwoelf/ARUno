using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyUnoAI : MonoBehaviour {
	public static int playerControlledIndex = 0;
	static int waitInSeconds = 4;
	static GameObject unoAI;
	bool isReady = true;

	// Use this for initialization
	void Start () {
		unoAI = GameObject.Find("UnoAI");
	}
	
	// Update is called once per frame
	void Update () {
		if (isReady && EasyUnoAI.IsAIControlled()) {
			isReady = false;
			StartCoroutine(AIAction());
		}
	}

	// AI take action as the player
	IEnumerator AIAction() {
		// wait a few seconds to decide what action to take
		yield return new WaitForSeconds(waitInSeconds);

		// do actions
		bool didSetCard = false;
		foreach (CardView item in GameManager.CurrentPlayer.cardViews) {
			if (item.CanBePlayed()) {
				GameManager.CurrentPlayer.cardViews.Remove(item);
				CurrentCard.SetCurrentCard(item.card.index);
				Destroy(item.gameObject);
				didSetCard = true;
				break;
			}
		}
		if (!didSetCard) {
			GameManager.CurrentPlayer.Draw();
			GameManager.ToggleNextPlayer();
		}
		// set AI action ready to true
		isReady = true;
	}

	public static bool IsAIControlled() {
		return unoAI && EasyUnoAI.playerControlledIndex != GameManager.PlayerIndex;
	}
}