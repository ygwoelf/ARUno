using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Draw2Card", menuName = "Uno/Draw2Card")]
public class Draw2Card : ColoredCard {
    public override void OnPlay() {
        if (CanJumpIn()) {
            GameManager.PlayerIndex = playerID;
        }
        GameManager.NextPlayer.Draw(2);
        GameManager.Skip();
    }
}
