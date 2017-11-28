using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    float holdTime = 0.5f;
    bool wasPressed = false;
    public Card card;

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
        return card.CanBePlayed();
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
            CurrentCard.SetCurrentCard(card.index);
            GameManager.CurrentPlayer.cardViews.Remove(this);
            Destroy(gameObject);
        }
    }
}
