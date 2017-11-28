using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public GameObject hand;
    public List<CardView> cardViews = new List<CardView>();
    bool didDraw = false;
    public int playerID;

    // setup hand
    public void Awake() {
        hand = transform.Find("Hand").gameObject;
    }

    // begin current player turn
    public void BeginTurn() {
        didDraw = false;
        // set current hand as scrollable
        GameManager.playerHolder.GetComponent<ScrollRect>().content = hand.GetComponent<RectTransform>();
        // set current player in textbar
        GameObject.FindGameObjectWithTag("PlayerTextBar").GetComponentInChildren<Text>().text = "PLAYER " + playerID + "'S TURN";
        hand.SetActive(true);
    }

    // end current turn
    public void EndTurn() {
        hand.SetActive(false);
        if (!EasyUnoAI.IsCurrentPlayer())
            GameManager.ShowContinueUI(playerID);
    }

    // pile click action, draw a card
    public void OnPileClick() {
        if (GameManager.continueUI) return;

        Card card = Pile.shared.PopCard();
        Debug.Log(didDraw);
        if(!didDraw) {
            if(card.CanBePlayed()) {
                didDraw = true;
            } else {
                EndTurn();
                GameManager.ToggleNextPlayer();
            }
            cardViews.Add(card.CreateCardView(hand.transform));
        }
    }

    // draw 1 card
    public void Draw() {
        CardView view = Pile.shared.PopCard().CreateCardView(hand.transform);
        cardViews.Add(view);
    }

    // draw n cards
    public void Draw(int n) {
        for(int i = 0; i < n; i++) {
            Draw();
        }
    }
}