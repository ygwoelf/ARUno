using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile : MonoBehaviour {
    public static Pile shared;
    public List<Card> pile = new List<Card>();
    private List<Card> stock = new List<Card>();
    private System.Random rand;

    // init properties
    public void Awake() {
        shared = this;
        pile = new List<Card>(Resources.LoadAll<Card>("Cards"));
        rand = new System.Random();
        Shuffle();
    }
    
    // restock the pile
    public void RestockPile() {
        foreach(Card card in stock) {
            pile.Add(card);
        }
        stock.Clear();
    }

    // random shuffle
    public void Shuffle() {
        for(int i = 0; i < pile.Count; i++) { 
            Card temp = pile[i];
            int randomNum = rand.Next(0, pile.Count);
            pile[i] = pile[randomNum];
            pile[randomNum] = temp;
        }
    }

    // pop top card
    public Card Pop() {
        Card temp = pile[0];
        stock.Add(temp);
        pile.RemoveAt(0);
        if (pile.Count == 0)
            RestockPile();
        return temp;
    }

    // pile is loaded
    public bool IsReady() {
        return shared.pile.Count != 0;
    }
}