using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class Card : ScriptableObject, IComparable<Card> {
    [SerializeField]
    public CardModel model;
    public int index;

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

    //This method is required by the IComparable interface. 
    public int CompareTo(Card other) {
        if(other == null) {
            return 1;
        }
        return index - other.index;
    }
}
