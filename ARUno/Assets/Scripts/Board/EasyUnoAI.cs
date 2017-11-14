using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyUnoAI : MonoBehaviour {
	public static int controlledIndex = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (EasyUnoAI.IsCurrentPlayer()) {
			bool didSetCard = false;
			foreach (CardView item in GameManager.CurrentPlayer.cardViews) {
				if (item.CanBePlayed()) {
					CurrentCard.SetCurrentCard(item.gameObject);
					didSetCard = true;
					break;
				}
			}
			if (!didSetCard) {
				GameManager.CurrentPlayer.OnPileClick();
			}
		}
	}

	public static bool IsCurrentPlayer() {
		return EasyUnoAI.controlledIndex == GameManager.PlayerIndex;
	}
}