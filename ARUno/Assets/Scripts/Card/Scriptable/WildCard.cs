using UnityEngine;
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
        ShowWildMenu();
    }

    // show color selection
    public void ShowWildMenu() {
        ColorChangeMenu = Resources.Load<GameObject>("WildMenu");
        ColorChangeMenu = Instantiate<GameObject>(ColorChangeMenu);
        ColorChangeMenu.transform.SetParent(GameObject.FindGameObjectWithTag("screen").transform);
        ColorChangeMenu.transform.SetAsLastSibling();
        ColorChangeMenu.transform.localPosition = Vector3.zero;

        ColorChangeMenu.transform.Find("red").GetComponent<Button>().onClick.AddListener(delegate { ChangeColor(CardColor.red); });
        ColorChangeMenu.transform.Find("blue").GetComponent<Button>().onClick.AddListener(delegate { ChangeColor(CardColor.blue); });
        ColorChangeMenu.transform.Find("green").GetComponent<Button>().onClick.AddListener(delegate { ChangeColor(CardColor.green); });
        ColorChangeMenu.transform.Find("yellow").GetComponent<Button>().onClick.AddListener(delegate { ChangeColor(CardColor.yellow); });
    }
 
	// change color
    public void ChangeColor(CardColor color) {
        CurrentCard.shared.model.color = color;
        CurrentCard.shared.RedrawCard();
        Destroy(ColorChangeMenu);
        GameManager.CurrentPlayer.EndTurn();
        GameManager.ToggleNextPlayer();
    }
}