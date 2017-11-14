using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "ColoredCard", menuName = "Uno/ColoredCard")]
public class ColoredCard : Card {
    public override bool CanBePlayed() {
        return (model.color == CurrentCard.shared.model.color) || (model.value == CurrentCard.shared.model.value);
    }
}