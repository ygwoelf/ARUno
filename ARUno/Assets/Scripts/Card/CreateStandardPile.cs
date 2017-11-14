
// Only in editor mode
#if (UNITY_EDITOR) 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

public class CreateStandardPile {

	private static Sprite[] cardFaces = Resources.LoadAll<Sprite>("Images/uno-cards");
	private static int index = 0;

    [MenuItem("Assets/Create/Standard Uno Deck")]
    public static List<Card> CreateUnoDeck() {
		List<Card> cards = new List<Card>();
		for (int i = 0; i < 4; i++) {
			int start = i*16;
			for (int k = 0; k < 2; k++) {
				// 1-9
				for (int j = start; j < start+9; j++) {
					cards.Add( CreateUnoCardHelper(ScriptableObject.CreateInstance<ColoredCard>(), j, GetCardColor(i), j-start) );
				}
				// skip, reverse, draw2
				cards.Add( CreateUnoCardHelper(ScriptableObject.CreateInstance<SkipCard>(), start+11, GetCardColor(i), -1) );
				cards.Add( CreateUnoCardHelper(ScriptableObject.CreateInstance<ReverseCard>(), start+12, GetCardColor(i), -2) );
				cards.Add( CreateUnoCardHelper(ScriptableObject.CreateInstance<Draw2Card>(), start+13, GetCardColor(i), -3) );
			}
			// 0
			cards.Add( CreateUnoCardHelper(ScriptableObject.CreateInstance<ColoredCard>(), start+10, GetCardColor(i), 0) );
			// wild, wild_draw4
			cards.Add( CreateUnoCardHelper(ScriptableObject.CreateInstance<WildCard>(), start+14, GetCardColor(i), -5) );
			cards.Add( CreateUnoCardHelper(ScriptableObject.CreateInstance<WildDraw4Card>(), start+15, GetCardColor(i), -4) );
		}
		return cards;
    }

	// create uno card
	// i is the ith sprite in the folder
	private static Card CreateUnoCardHelper(Card card, int i, CardColor color, int value) {
		CardModel model = new CardModel();
		model.face = cardFaces[i];
		model.color = color;
		model.value = value;
		card.model = model;

		String resName = "Uno_" + index++ + color.ToString() + value;
		Debug.Log("created " + resName);

        AssetDatabase.CreateAsset(card, "Assets/Cards/" + resName + ".asset");
        AssetDatabase.SaveAssets();
		return card;
	}

	private static CardColor GetCardColor(int i = -1) {
		switch (i) {
			case 0:
                return CardColor.red;
            case 1:
                return CardColor.yellow;
            case 2:
                return CardColor.green;
            case 3:
                return CardColor.blue;
            default:
                return CardColor.wild;
		}
	}
}
#endif