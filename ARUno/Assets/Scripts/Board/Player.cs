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
    public Transform sideT {
        get {
            var arCanvasGO = GameObject.Find("ARUnoCanvas");
            if (arCanvasGO != null) {
                return arCanvasGO.transform.GetChild(playerID);
            } else return null;
        }
    }
    private GameObject progressiveUnoUI;
    private CardView pUnoCV;

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

    /// show card on ar board
    void Update() {
        ShowCardOnARBoard();
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
    }

    // draw n cards
    public void Draw(int n) {
        for(int i = 0; i < n; i++) {
            Draw();
        }
    }

    // force draw from +2,+4 cards
    public void ForceDraw(int n, int cardValue) {
        GameManager.ToggleNextPlayer();
        GameManager.ProgressiveUnoPenalty += n;
        foreach (var cv in cardViews) {
            // either has the same value or value for +4 (500)
            if (cv.card.model.value == 500 || cv.card.model.value == cardValue) {
                pUnoCV = cv;
                if (EasyUnoAI.playerControlledIndex != playerID) {
                    ProgressiveUnoListener();
                } else {
                    progressiveUnoUI = GameManager.CreateUI("Progressive Uno: Stack current penalty and passes it to the following player?", "Confirm");
                    progressiveUnoUI.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(ProgressiveUnoListener);
                    progressiveUnoUI.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(CancelButtonListener);
                    GameManager.IsPaused = true;
                }
                return;
            }
        }
        CancelButtonListener();
    }

    // show number of cards in ar board
    public void ShowCardOnARBoard() {
        if (sideT != null) {
            if (sideT.childCount >= cardCount) {
                for (int i = 0; i < cardCount; i++) {
                    sideT.GetChild(i).gameObject.SetActive(true);
                }
                for (int i = cardCount; i < sideT.childCount; i++) {
                    sideT.GetChild(i).gameObject.SetActive(false);
                }
            } else if (sideT.childCount < cardCount) {
                for (int i = sideT.childCount; i < cardCount; i++) {
                    Instantiate(sideT.GetChild(0).gameObject, sideT);
                }
            }
        }
    }

    // hide card
    public void HideCardFromARBoard() {
        if (sideT != null) {
            sideT.GetChild(cardCount).gameObject.SetActive(false);
        }
    }
    
    // confirm: set draw card from hand
    private void ProgressiveUnoListener() {
        cardViews.Remove(pUnoCV);
        CurrentCard.SetCurrentCard(pUnoCV.card.index);
        Destroy(pUnoCV.gameObject);
        Destroy(progressiveUnoUI);
        GameManager.IsPaused = false;
        pUnoCV = null;
    }

    // cancel to dismiss progressiveUno AlertMenu
    private void CancelButtonListener() {
        Draw(GameManager.ProgressiveUnoPenalty);
        GameManager.ToggleNextPlayer();
        GameManager.ProgressiveUnoPenalty = 0;
        GameManager.IsPaused = false;
        Destroy(progressiveUnoUI);
    }
}