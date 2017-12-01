using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class CardView : MonoBehaviour, IComparable<CardView>, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    float holdTime = 0.5f;
    bool wasPressed = false;
    public Card card;
    public int playerID;

    // change local scale according to can be played or pressed
    void Update() {
        if (CanBePlayed()) {
            if (wasPressed) {
                transform.localScale = new Vector3(1.2F, 1.2F, 1);
            } else {
                transform.localScale = new Vector3(1.1F, 1.1F, 1);
            }
        } else {
            transform.localScale = Vector3.one;
        }
    }

    // face of the card view
    public Sprite CardFace {
        get {
            return GetComponent<Image>().sprite;
        }
        set {
            Image image = GetComponent<Image>();
            image.sprite = value;
        }
    }

    // card can be played
    public bool CanBePlayed() {
        // TODO: add jump-in
        return card.CanBePlayed() && playerID == GameManager.PlayerIndex;
    }

    // card action
    public virtual void OnPlay() {
        card.OnPlay();
    }

    // press down
    public void OnPointerDown(PointerEventData eventData) {
        if(CanBePlayed()) {
            wasPressed = true;
            Invoke("OnLongPress", holdTime);
        }
    }
 
    // release
    public void OnPointerUp(PointerEventData eventData) {
        CancelLongPress();
    }
 
    // exit pointer
    public void OnPointerExit(PointerEventData eventData) {
        CancelLongPress();
    }

    // helper to cancel long press
    private void CancelLongPress() {
        CancelInvoke("OnLongPress");
        wasPressed = false;
    }
 
    // called when pressed for holdTime long
    private void OnLongPress() {
        if(CanBePlayed()) {
            GameManager.CurrentPlayer.cardViews.Remove(this);
            CurrentCard.SetCurrentCard(card.index);
            Destroy(gameObject);
        }
    }

    // This method is required by the IComparable interface. 
    public int CompareTo(CardView other) {
        if(other == null) {
            return 1;
        }
        // sort by color then by value
        int ret = card.model.color - other.card.model.color;
        return ret == 0 ? card.model.value - other.card.model.value : ret;
    }
}
