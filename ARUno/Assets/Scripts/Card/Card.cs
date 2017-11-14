using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Card : ScriptableObject {
    [SerializeField]
    public CardModel model;

    // true if the card can be played this turn
    public abstract bool CanBePlayed();

    // special cards override for special moves
    public virtual void OnPlay() {
        GameManager.CurrentPlayer.EndTurn();
        GameManager.ToggleNextPlayer();
    }

    // instatiate new card view
    public CardView CreateCardView(Transform parent) {
        CardView cardView = Instantiate(Resources.Load<CardView>("Card"));
        cardView.gameObject.GetComponent<Image>().sprite = model.face;
        cardView.transform.SetParent(parent);
        cardView.card = this;
        return cardView;
    }

    // true if is action card (wild or reverse, skip, +2)
    public bool IsActionCard() {
        return (model.color == CardColor.wild) || (model.value < 0);
    }
}
