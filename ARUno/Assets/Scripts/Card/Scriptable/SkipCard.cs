using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "SkipCard", menuName = "Uno/SkipCard")]
public class SkipCard : ColoredCard {
    public override void OnPlay() {
        if (playerID != GameManager.NextPlayer.playerID && CanJumpIn()) {
            GameManager.PlayerIndex = playerID;
        }
        GameManager.Skip();
    }
}
