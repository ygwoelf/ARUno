using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WildCard", menuName = "Uno/WildCard")]
public class WildCard : Card {
    GameObject ColorChangeMenu;

    // wild card can be played always
    public override bool CanBePlayed() {
        return true;
    }

    // wild card can jump in if value equals
    public override bool CanJumpIn() {
        return model.value == CurrentCard.shared.model.value;
    }

	// opens color change menu
    public override void OnPlay() {
        if (CanJumpIn()) {
            GameManager.PlayerIndex = playerID;
        }
        ShowWildMenu();
    }

    // show color selection
    public void ShowWildMenu(int wildMove = 1) {
        if (EasyUnoAI.IsAIControlled()) {
            ChangeColor( (CardColor)Random.Range(1, 4), wildMove );
            return;
        }
        ColorChangeMenu = Resources.Load<GameObject>("Prefabs/WildMenu");
        ColorChangeMenu = Instantiate<GameObject>(ColorChangeMenu, GameObject.FindGameObjectWithTag("screen").transform);
        ColorChangeMenu.transform.SetAsLastSibling();
        ColorChangeMenu.transform.localPosition = Vector3.zero;

        ColorChangeMenu.transform.Find("red").GetComponent<Button>().onClick.AddListener(delegate { ChangeColor(CardColor.red, wildMove); });
        ColorChangeMenu.transform.Find("blue").GetComponent<Button>().onClick.AddListener(delegate { ChangeColor(CardColor.blue, wildMove); });
        ColorChangeMenu.transform.Find("green").GetComponent<Button>().onClick.AddListener(delegate { ChangeColor(CardColor.green, wildMove); });
        ColorChangeMenu.transform.Find("yellow").GetComponent<Button>().onClick.AddListener(delegate { ChangeColor(CardColor.yellow, wildMove); });
    }
 
	// change color
    public void ChangeColor(CardColor color, int wildMove) {
        CurrentCard.shared.model.color = color;
        CurrentCard.shared.RedrawCard();
        Destroy(ColorChangeMenu);
        if (wildMove == 4) {
            GameManager.NextPlayer.ForceDraw(4, model.value);
        } else {
            GameManager.ToggleNextPlayer();
        }
    }
}