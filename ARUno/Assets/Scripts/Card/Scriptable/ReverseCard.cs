using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ReverseCard", menuName = "Uno/ReverseCard")]
public class ReverseCard : ColoredCard {
    public override void OnPlay() {
        GameManager.Reverse();
    }
}
