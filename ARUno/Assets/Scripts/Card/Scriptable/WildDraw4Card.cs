using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(fileName = "WildDraw4Card", menuName = "Uno/WildDraw4Card")]
public class WildDraw4Card : WildCard {
    public override void OnPlay() {
        ShowWildMenu(2);
        GameManager.NextPlayer.Draw(4);
    }
}