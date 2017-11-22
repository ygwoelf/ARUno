using UnityEngine;
using System.Collections;

[System.Serializable]
public class CardModel {
    public Sprite face;
    public CardColor color;
    public int value;

    public CardModel() { }
    public CardModel(Sprite face, CardColor color, int value) {
        this.face = face;
        this.color = color;
        this.value = value;
    }

    public Color getColor() {
        switch (color) {
            case CardColor.red:
                return Color.red;
            case CardColor.blue:
                return Color.blue;
            case CardColor.green:
                return Color.green;
            case CardColor.yellow:
                return Color.yellow;
            default:
                return Color.black;
        }
    }
}

public enum CardColor {
	red,
	blue,
	green,
	yellow,
	wild
}