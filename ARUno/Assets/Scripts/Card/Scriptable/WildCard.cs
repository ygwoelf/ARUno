﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WildCard", menuName = "Uno/WildCard")]
public class WildCard : Card {
    GameObject ColorChangeMenu;

    public override bool CanBePlayed() {
        return true;
    }

	// opens color change menu
    public override void OnPlay() {
        if (CanJumpIn()) {
            GameManager.PlayerIndex = playerID;
        }
        ShowWildMenu();
    }

    // show color selection
    public void ShowWildMenu(int nextPlayer = 1) {
        if (EasyUnoAI.IsAIControlled()) {
            ChangeColor( (CardColor)Random.Range(1, 4), nextPlayer );
            return;
        }
        ColorChangeMenu = Resources.Load<GameObject>("Prefabs/WildMenu");
        ColorChangeMenu = Instantiate<GameObject>(ColorChangeMenu, GameObject.FindGameObjectWithTag("screen").transform);
        ColorChangeMenu.transform.SetAsLastSibling();
        ColorChangeMenu.transform.localPosition = Vector3.zero;

        ColorChangeMenu.transform.Find("red").GetComponent<Button>().onClick.AddListener(delegate { ChangeColor(CardColor.red, nextPlayer); });
        ColorChangeMenu.transform.Find("blue").GetComponent<Button>().onClick.AddListener(delegate { ChangeColor(CardColor.blue, nextPlayer); });
        ColorChangeMenu.transform.Find("green").GetComponent<Button>().onClick.AddListener(delegate { ChangeColor(CardColor.green, nextPlayer); });
        ColorChangeMenu.transform.Find("yellow").GetComponent<Button>().onClick.AddListener(delegate { ChangeColor(CardColor.yellow, nextPlayer); });
    }
 
	// change color
    public void ChangeColor(CardColor color, int nextPlayer) {
        CurrentCard.shared.model.color = color;
        CurrentCard.shared.RedrawCard();
        Destroy(ColorChangeMenu);
        GameManager.ToggleNextPlayer(nextPlayer);
    }
}