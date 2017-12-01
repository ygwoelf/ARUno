using UnityEngine;
using System.Collections;

[System.Serializable]
public class CardModel {
    Color LustRed = new Color(0.92F, 0.10F, 0.13F, 1.00F);
    Color ElectricBlue = new Color(0.00F, 0.54F, 0.84F, 1.00F);
    Color PigmentGreen = new Color(0.01F, 0.61F, 0.28F, 1.00F);
    Color GoldYellow = new Color(1.00F, 0.85F, 0.01F, 1.00F);

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
                return LustRed;
            case CardColor.blue:
                return ElectricBlue;
            case CardColor.green:
                return PigmentGreen;
            case CardColor.yellow:
                return GoldYellow;
            default:
                return Color.black;
        }
    }
}

public enum CardColor {
	wild,
	red,
	blue,
	green,
	yellow
}