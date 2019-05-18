using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] public Ship ship;
    [SerializeField] public Player enemy;
    [SerializeField] public int hp = 3;

    [Header("Card regions")]
    public CardRegion hand;
    public CardRegion selected;

    public CardRegion deck;
    public CardRegion discard;
    public CardRegion exhile;


    public void Draw()
    {
        Card c = deck.Draw();
        if (c == null)
        {
            ShuffleDiscard();
            c = deck.Draw();
        }
        if (c != null)
        {
            c.gameObject.SetActive(true);
            hand.AddCard(c);
        }
    }

    public void Select(Card card)
    {
        if (card.region.Remove(card))
        {
            if (selected.cards.Count > 0)
            {
                Card previous = selected.cards[0];
                selected.Remove(previous);
                hand.AddCard(previous);
            }
            selected.AddCard(card);
        }
    }

    public void Hurt(CardRule source)
    {
        this.hp -= source.damageAmount;
    }

    public void DiscardSelected()
    {
        foreach (Card card in selected.cards)
        {
            discard.AddCard(card);
            card.gameObject.SetActive(false);
        }
        selected.Clear();
    }
    public void ShuffleDiscard()
    {
        foreach (Card card in discard.cards)
        {
            deck.AddCard(card);
            card.gameObject.SetActive(false);
        }
        deck.Shuffle();
        discard.Clear();
    }
}
