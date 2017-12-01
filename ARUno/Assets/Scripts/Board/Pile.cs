using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Pile : MonoBehaviour {
    public static Pile shared;
    public static Card[] CardRes { get; private set; }
    private List<int> pile;
    private List<int> stock = new List<int>();
    private System.Random rand;

    // init properties
    public void Awake() {
        shared = this;
        CardRes = Resources.LoadAll<Card>("Cards");
        Array.Sort(CardRes);
        pile = Enumerable.Range(0, CardRes.Length).ToList<int>();
        rand = new System.Random();
        gameObject.GetComponent<Button>().onClick.AddListener(OnPileClick);
        Shuffle();
    }
    
    // restock the pile
    public void RestockPile() {
        pile = stock.ToList();
        stock.Clear();
        Shuffle();
    }

    // random shuffle
    public void Shuffle() {
        for(int i = 0; i < pile.Count; i++) { 
            int temp = pile[i];
            int randomNum = rand.Next(0, pile.Count);
            pile[i] = pile[randomNum];
            pile[randomNum] = temp;
        }
    }

    // pop top card
    public int Pop() {
        int temp = pile[0];
        stock.Add(pile[0]);
        pile.RemoveAt(0);
        if (pile.Count == 0)
            RestockPile();
        return temp;
    }

    // pop n cards
    public List<int> Pop(int n) {
        List<int> cards = new List<int>();
        for (int i = 0; i < n; i++) {
            cards.Add(Pop());
        }
        return cards;
    }

    // get card from index
    public Card PopCard() {
        return CardRes[Pop()];
    }

    // peek card from top
    public Card PeekCard() {
        Debug.Log(pile[0]);
        return CardRes[pile[0]];
    }

    // pile is loaded
    public bool IsReady() {
        return shared.pile.Count != 0;
    }

    // pile click action, draw a card
    public void OnPileClick() {
        GameManager.CurrentPlayer.OnPileClick();
    }
}