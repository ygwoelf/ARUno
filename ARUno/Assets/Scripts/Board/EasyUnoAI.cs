using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyUnoAI : MonoBehaviour {
	public static int playerControlledIndex = 0;
	static int waitInSeconds = 4;
	static GameObject unoAI;
	bool isReady = true;
	private int aiMode = 0;

	// Use this for initialization
	void Start () {
		unoAI = GameObject.Find("UnoAI");
	}
	
	// Update is called once per frame
	void Update () {
		if (isReady && EasyUnoAI.IsAIControlled() && !GameManager.IsPaused) {
			isReady = false;
			StartCoroutine(AIAction());
		}
	}

    // testing AI vs. AI
	void OnGUI() {
		string modeString = aiMode == 0 ? "Player vs. AI" : "AI vs. AI";
		if (GUI.Button(new Rect(0.0f, 0.0f, 150.0f, 100.0f), modeString)) {
			aiMode = (aiMode + 1) % 2;
			playerControlledIndex = aiMode == 0 ? 0 : -1;
			waitInSeconds = aiMode == 0 ? 4 : 0;
		}
	}

	// AI take action as the player
	IEnumerator AIAction() {
		// wait a few seconds to decide what action to take
		yield return new WaitForSeconds(waitInSeconds);

		// do actions
		List<CardView> cvs = new List<CardView>();
		foreach (CardView item in GameManager.CurrentPlayer.cardViews) {
			if (item.CanBePlayed()) {
				cvs.Add(item);
			}
		}
		// play the card with highest value
		if (cvs.Count > 0) {
			if (GameManager.CurrentPlayer.cardViews.Count == 2) {
				GameManager.CurrentPlayer.didCallUno = true;
			}
			GameManager.CurrentPlayer.cardViews.Remove(cvs[0]);
			CurrentCard.SetCurrentCard(cvs[0].card.index);
			Destroy(cvs[0].gameObject);
		} else {
			GameManager.CurrentPlayer.PileClickHelper();
		}
		// call "UNO!"
		GameManager.CallUno(GameManager.PlayerIndex);
		
		// set AI action ready to true
		isReady = true;
	}

	public static bool IsAIControlled() {
		return unoAI && EasyUnoAI.playerControlledIndex != GameManager.PlayerIndex;
	}
}