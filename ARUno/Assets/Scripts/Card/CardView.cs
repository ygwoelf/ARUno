using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public Card card;
    Transform previousParent;
    GameObject placeholder;
    int siblingIndex;
    public bool IsDraggable;
    public bool IsBeingDragged;

	// Awake is called when the script instance is being loaded.
    public void Awake() {
        IsDraggable = false;

        if(gameObject.tag != "CurrentCard") {
            IsDraggable = true;
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

    // reset card to hand
    public void ReturnCard() {
        transform.SetParent(previousParent);
        transform.SetSiblingIndex(siblingIndex);
    }

    // card action
    public virtual void OnPlay() {
        card.OnPlay();
    }

    // drag to e.position
    public void OnDrag(PointerEventData eventData) {
        if(IsDraggable) {
            this.transform.position = eventData.position;
            RaycastResult rr = eventData.pointerCurrentRaycast;
            if(rr.isValid && rr.gameObject.tag == "Card") {
                siblingIndex = rr.gameObject.transform.GetSiblingIndex();
                placeholder.transform.SetSiblingIndex(siblingIndex);
            }
        }
    }

    // begin drag
    public void OnBeginDrag(PointerEventData eventData) {
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        if(IsDraggable) {
            IsBeingDragged = true;
            previousParent = transform.parent;
            siblingIndex = transform.GetSiblingIndex();

            placeholder = (GameObject)Instantiate(Resources.Load("Placeholder"));
            placeholder.transform.SetParent(previousParent);
            placeholder.transform.SetSiblingIndex(siblingIndex);
            transform.SetParent(transform.parent.parent);
        }
    }

    // end drag
    public void OnEndDrag(PointerEventData eventData) {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if(IsDraggable) {
            Destroy(placeholder);
            RaycastResult rr = eventData.pointerCurrentRaycast;
            if(rr.isValid && rr.gameObject.tag == "CurrentCard" && card.CanBePlayed()) {
                CurrentCard.SetCurrentCard(card.index);
                GameManager.CurrentPlayer.cardViews.Remove(this);
                Destroy(gameObject);
            } else {
                ReturnCard();
            }
        }
    }
}
