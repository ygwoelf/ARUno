using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    float holdTime = 0.5f;
    bool IsCurrentCard = false;
    
    public Card card;

	// Awake is called when the script instance is being loaded.
    public void Awake() {
        if(gameObject.tag == "CurrentCard") {
            IsCurrentCard = true;
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

    public void OnPointerDown(PointerEventData eventData) {
        if(!IsCurrentCard && CanBePlayed()) {
            Invoke("OnLongPress", holdTime);
        }
    }
 
    public void OnPointerUp(PointerEventData eventData) {
        CancelLongPress();
    }
 
    public void OnPointerExit(PointerEventData eventData) {
        CancelLongPress();
    }

    private void CancelLongPress() {
        CancelInvoke("OnLongPress");
    }
 
    private void OnLongPress() {
        if(CanBePlayed()) {
            CurrentCard.SetCurrentCard(card.index);
            GameManager.CurrentPlayer.cardViews.Remove(this);
            Destroy(gameObject);
        }
    }
}
