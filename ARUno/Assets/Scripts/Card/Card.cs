using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class Card : ScriptableObject, IComparable<Card> {
    [SerializeField]
    public CardModel model;
    public int index;
    public int playerID = -1;

    // true if the card can be played this turn
    public abstract bool CanBePlayed();

    // special cards override for special moves
    public virtual void OnPlay() {
        if (CanJumpIn()) {
            GameManager.PlayerIndex = playerID;
        }
        GameManager.ToggleNextPlayer();
    }

    // jump in condition
    public bool CanJumpIn() {
        return model.color == CurrentCard.shared.model.color && model.value == CurrentCard.shared.model.value;
    }

    // instatiate new card view
    public CardView CreateCardView(Transform parent) {
        CardView cardView = Instantiate(Resources.Load<CardView>("Prefabs/Card"), parent);
        cardView.gameObject.GetComponent<Image>().sprite = model.face;
        cardView.card = this;
        return cardView;
    }

    // true if is action card (wild or reverse, skip, +2)
    public bool IsActionCard() {
        return (model.color == CardColor.wild) || (model.value > 10);
    }

    // This method is required by the IComparable interface. 
    public int CompareTo(Card other) {
        if(other == null) {
            return 1;
        }
        return index - other.index;
    }
}
