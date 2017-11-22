using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// using UnityEngine.Events;

public class CardView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    [SerializeField]
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    private float holdTime = 1f;
    
    public Card card;
    public bool IsCurrentCard = false;
    // public UnityEvent onLongPress = new UnityEvent();

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
        Invoke("OnLongPress", holdTime);
    }
 
    public void OnPointerUp(PointerEventData eventData) {
        CancelInvoke("OnLongPress");
    }
 
    public void OnPointerExit(PointerEventData eventData) {
        CancelInvoke("OnLongPress");
    }
 
    private void OnLongPress() {
        if(card.CanBePlayed()) {
            CurrentCard.SetCurrentCard(card.index);
            GameManager.CurrentPlayer.cardViews.Remove(this);
            Destroy(gameObject);
        }
    }
}
