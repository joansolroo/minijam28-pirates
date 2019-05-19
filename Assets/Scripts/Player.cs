using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public bool player1 = false;
    [SerializeField] public Ship ship;
    [SerializeField] public Player enemy;
    [SerializeField] public Color color;
    [SerializeField] public Sprite faction;

    [SerializeField] public int maxHp = 3;
    [SerializeField] public int hp = 3;
    [SerializeField] public HP HPCounter;

    [SerializeField] public List<CardRule> deckbuild;
    [SerializeField] Card cardTemplate;


    [Header("Card regions")]
    public CardRegion hand;
    public CardRegion selected;

    public CardRegion deck;
    public CardRegion discard;
    public CardRegion exhile;

    List<Card> deckContainer = new List<Card>();
    [SerializeField] PreviewAction preview;

    public void RemakeDeck()
    {
        if (deckContainer!=null && deckContainer.Count>0)
        {
            for (int c = 0; c < deckContainer.Count; ++c)
            {
                GameObject.Destroy(deckContainer[c].gameObject);
            }
            deckContainer.Clear();
        }
        foreach (CardRule rule in deckbuild)
        {
            Card card = GameObject.Instantiate<Card>(cardTemplate);
            card.owner = this;
            card.rule = rule;
            card.transform.parent = deck.transform;
            card.transform.localPosition = Vector3.zero;
            card.transform.localEulerAngles = new Vector3(180, 0, 180);
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
            c.SetVisible(this == player1);
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
            preview.DoPreviewAction();
        }
       
    }
    public void RevealSelected()
    {
        foreach (Card card in selected.cards)
        {
            card.SetVisible(true);
        }
        preview.DoPreviewAction();
    }
    public void DiscardSelected()
    {
        foreach (Card card in selected.cards)
        {
            discard.AddCardLast(card);
        }
        selected.Clear();
        preview.DoPreviewAction();

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

    
    public void ShuffleDiscard()
    {
        foreach (Card card in discard.cards)
        {
            deck.AddCardLast(card);
            card.SetVisible(false);
        }
        deck.Shuffle();
        discard.Clear();
    }
}
