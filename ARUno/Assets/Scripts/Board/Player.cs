using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public GameObject hand;
    public List<CardView> cardViews = new List<CardView>();
    public int playerID;
    bool didDraw = false;

    // Awake is called when the script instance is being loaded.
    void Awake() {
        hand = transform.Find("Hand").gameObject;
    }

    // setup hand
    void Start() {
        if (playerID == EasyUnoAI.playerControlledIndex) {
            hand.SetActive(true);
        }
    }

    // begin current player turn
    public void BeginTurn() {
        didDraw = false;
        // set current hand as scrollable and text bar if not controlled by AI
        if (!EasyUnoAI.IsAIControlled()) {
            GameManager.playerHolder.GetComponent<ScrollRect>().content = hand.GetComponent<RectTransform>();
            GameObject.FindGameObjectWithTag("PlayerTextBar").GetComponentInChildren<Text>().text = "PLAYER " + playerID + "'S TURN";
        } else {
            GameObject.FindGameObjectWithTag("PlayerTextBar").GetComponentInChildren<Text>().text = "AI " + playerID + "'S TURN";
        }
    }

    // pile click action, draw a card
    public void OnPileClick() {
        Card card = Pile.shared.PopCard();
        if(!didDraw) {
            if(card.CanBePlayed()) {
                didDraw = true;
            } else {
                GameManager.ToggleNextPlayer();
            }
            cardViews.Add(card.CreateCardView(hand.transform, playerID));
        }
    }

    // draw 1 card
    public void Draw() {
        CardView view = Pile.shared.PopCard().CreateCardView(hand.transform, playerID);
        cardViews.Add(view);
    }

    // draw n cards
    public void Draw(int n) {
        for(int i = 0; i < n; i++) {
            Draw();
        }
    }
}