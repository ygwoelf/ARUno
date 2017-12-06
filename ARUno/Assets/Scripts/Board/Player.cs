using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public GameObject hand;
    public List<CardView> cardViews = new List<CardView>();
    public int playerID;

    // "Uno" house rule
    public bool didCallUno = false;
    public int cardCount {
        get {
            return cardViews.Count;
        }
    }

    // Awake is called when the script instance is being loaded.
    void Awake() {
        hand = transform.Find("Hand").gameObject;
    }

    // show hand only for the player, set scroll rect to player's hand
    void Start() {
        if (playerID == EasyUnoAI.playerControlledIndex) {
            hand.SetActive(true);
            GameManager.playerHolder.GetComponent<ScrollRect>().content = hand.GetComponent<RectTransform>();
        }
    }

    // begin current player turn
    public void BeginTurn() {
        // set text bar indicating who's turn it is
        if (!EasyUnoAI.IsAIControlled()) {
            GameObject.FindGameObjectWithTag("PlayerTextBar").GetComponentInChildren<Text>().text = "PLAYER " + playerID + "'S TURN";
        } else {
            GameObject.FindGameObjectWithTag("PlayerTextBar").GetComponentInChildren<Text>().text = "AI " + playerID + "'S TURN";
        }
    }

    // pile click action, draw a card
    public void OnPileClick() {
        if (playerID != EasyUnoAI.playerControlledIndex) return;
        Card card = Pile.shared.PeekCard();
        if (card.CanBePlayed()) {
            Pile.shared.PopCard();
            CurrentCard.SetCurrentCard(card);
        } else {
            Draw();
            GameManager.ToggleNextPlayer();
        }
    }

    // draw 1 card
    public void Draw() {
        if (didCallUno) didCallUno = false;
        CardView view = Pile.shared.PopCard().CreateCardView(hand.transform);
        view.card.playerID = playerID;
        cardViews.Add(view);
        cardViews.Sort();
        int index = cardViews.IndexOf(view);
        view.transform.SetSiblingIndex(index);

        // show number of cards in ar board
        var arCanvasGO = GameObject.Find("ARUnoCanvas");
        if (arCanvasGO != null) {
            Transform sideT = arCanvasGO.transform.GetChild(playerID);
            if (sideT.childCount > cardCount) {
                // loop all child, active 0 to cardCount-1, hide cardCount to childCount children
            } else if (sideT.childCount < cardCount) {
                // active all child, init (cardCount - childCount) using GetChild(0)
            }
        }
    }

    // draw n cards
    public void Draw(int n) {
        for(int i = 0; i < n; i++) {
            Draw();
        }
    }
}