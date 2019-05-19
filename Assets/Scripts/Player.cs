using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] public Ship ship;
    [SerializeField] public Player enemy;
    [SerializeField] public Color color;
    [SerializeField] public Sprite faction;

    [SerializeField] public int maxHp = 3;
    [SerializeField] public int hp = 3;
    [SerializeField] public HP HPCounter;

    [SerializeField] CardRule[] deckbuild;
    [SerializeField] Card cardTemplate;

    [Header("Card regions")]
    public CardRegion hand;
    public CardRegion selected;

    public CardRegion deck;
    public CardRegion discard;
    public CardRegion exhile;

    Transform deckContainer;
    public void RemakeDeck()
    {
        if (deckContainer)
        {
            for (int c = 0; c < deckContainer.childCount; ++c)
            {
                GameObject.Destroy(deckContainer.GetChild(c).gameObject);
            }
        }
        else
        {
            deckContainer = new GameObject().transform;
            deckContainer.transform.parent = this.transform.parent;
            deckContainer.transform.localPosition = Vector3.zero;
        }

        foreach (CardRule rule in deckbuild)
        {
            Card card = GameObject.Instantiate<Card>(cardTemplate);
            card.owner = this;
            card.rule = rule;
            card.transform.parent = deckContainer;
            card.map = Map.current;
            deck.AddCardFirst(card);

        }
        deck.Shuffle();
    }

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
            c.SetVisible(false);
            hand.AddCardFirst(c);
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
                hand.AddCardLast(previous);
            }
            selected.AddCardLast(card);
        }
    }

    public void Heal(CardRule source)
    {
        if (source.healAmount > 0)
        {
            this.hp = Mathf.Min(this.maxHp, this.hp + source.healAmount);
            HPCounter.UpdateHP();
        }
        if (source.recoverCard)
        {
            Card repaired = exhile.Draw();
            if (repaired)
            {
                deck.AddCardLast(repaired);
            }
        }
    }
    public void Hurt(CardRule source)
    {
        this.hp -= source.damageAmount;

        Card c = deck.Draw();
        if (c == null)
        {
            ShuffleDiscard();
            c = deck.Draw();
        }
        if (c != null)
        {
            exhile.AddCardLast(c);
        }
        HPCounter.UpdateHP();
    }

    public void DiscardSelected()
    {
        foreach (Card card in selected.cards)
        {
            discard.AddCardLast(card);
        }
        selected.Clear();
    }
    public void ShuffleDiscard()
    {
        foreach (Card card in discard.cards)
        {
            deck.AddCardLast(card);
        }
        deck.Shuffle();
        discard.Clear();
    }
}
