using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player {
    public GameObject hand;
    public List<CardView> cardViews = new List<CardView>();
    bool didDraw = false;
    int playerID;

    // setup hand
    public Player(int id) {
        playerID = id;
        hand = GameObject.Instantiate(Resources.Load<GameObject>("Hand"));
        hand.transform.SetParent(GameManager.handHolder.transform);
        hand.transform.localPosition = Vector3.zero;
    }

    // begin current player turn
    public void BeginTurn() {
        Debug.Log(playerID + " begin turn");

        didDraw = false;
        foreach(CardView view in cardViews) {
            view.IsDraggable = true;
        }
        // set current hand as scrollable
        GameManager.handHolder.GetComponent<ScrollRect>().content = hand.GetComponent<RectTransform>();
        // set current player in textbar
        GameObject.FindGameObjectWithTag("PlayerTextBar").GetComponentInChildren<Text>().text = "PLAYER " + playerID + "'S TURN";
        hand.SetActive(true);
    }

    // end current turn
    public void EndTurn() {
        Debug.Log(playerID + " end turn");
        hand.SetActive(false);
        GameManager.ShowContinueUI(playerID);
    }

    // pile click action, draw a card
    public void OnPileClick() {
        if (GameManager.continueUI) return;

        Card card = Pile.shared.pile[0];
        if(!didDraw) {
            if(card.CanBePlayed()) {
                didDraw = true;
                foreach(CardView view in cardViews) {
                    view.IsDraggable = false;
                }
            } else {
                EndTurn();
                GameManager.ToggleNextPlayer();
            }
            Draw();
        }
    }

    // draw 1 card
    public void Draw() {
        CardView view = Pile.shared.Pop().CreateCardView(hand.transform);
        cardViews.Add(view);
    }

    // draw n cards
    public void Draw(int n) {
        for(int i = 0; i < n; i++) {
            Draw();
        }
    }
}