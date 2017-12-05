using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;

public class CurrentCard : MonoBehaviour {
    public static CurrentCard shared;
    public CardModel model = new CardModel();
    public static int Index { get; private set; }

	// Awake is called when the script instance is being loaded.
	void Awake() {
        shared = this;
	}

	// Use this for initialization
	public void Start () {
		// first card can't be action card
        Card firstCard = Pile.shared.PeekCard();
        while (firstCard.IsActionCard()) {
            Pile.shared.Shuffle();
            firstCard = Pile.shared.PeekCard();
        }
        Index = firstCard.index;
        SetCurrentCard(firstCard, true);
	}

    // set current card in board center
    void Update() {
		//use center of board to show current card
        var arCardGO = GameObject.Find("ARUnoCard");
        if (arCardGO == null) {
            return;
        }
        var arCardSpriteRenderer = arCardGO.GetComponent<SpriteRenderer>();
        arCardSpriteRenderer.sprite = model.face;
        // arCardSpriteRenderer.color = model.getColor();
    }
	
    // set top most card, if it's first card, do not play the card
    public static void SetCurrentCard(Card card, bool firstCard = false) {
        shared.model.face = card.model.face;
        shared.model.color = card.model.color;
        shared.model.value = card.model.value;
        shared.RedrawCard();
        Pile.shared.AddToStock(card.index);
        if(!firstCard) {
            GameManager.CheckForWin();
            card.OnPlay();
        }
    }

    // set current card from card index
    public static void SetCurrentCard(int index) {
        Card card = Pile.CardRes[index];
        SetCurrentCard(card);
    }

    // refresh current top most card
    public void RedrawCard() {
        GetComponent<Image>().sprite = model.face;
        GameObject.FindGameObjectWithTag("Background").GetComponent<Image>().color = model.getColor();
    }

    // helper to print description
    public static void DebugCard(CardModel m) {
        Debug.Log("Color: " + m.color + ", Value: " + m.value);
    }
}
