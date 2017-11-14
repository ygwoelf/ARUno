using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(fileName = "WildDraw4Card", menuName = "Uno/WildDraw4Card")]
public class WildDraw4Card : WildCard {
    public override void OnPlay() {
        ShowWildMenu();
        GameManager.NextPlayer.Draw(4);
        GameManager.ToggleNextPlayer(2);
    }
}