using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "SkipCard", menuName = "Uno/SkipCard")]
public class SkipCard : ColoredCard {
    public override void OnPlay() {
        GameManager.Skip();
    }
}
