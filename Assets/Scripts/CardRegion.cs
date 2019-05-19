using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}

public class CardRegion : MonoBehaviour
{

    [SerializeField] public Player player;
    public enum Region
    {
        Table, Hand, Selected, Deck, Exhile, Discard
    }
    [SerializeField] public Region region;
    [SerializeField] public List<Card> cards = new List<Card>();
    [SerializeField] public TextMesh labelText;

    string label;
    private void Awake()
    {
        label = labelText.text;
    }
    public void Shuffle()
    {
        cards.Shuffle();
        UpdateLayout();
    }

    public Card Draw()
    {
        if (cards.Count > 0)
        {
            int idx = cards.Count - 1;
            Card card = cards[idx];
            cards.RemoveAt(idx);
            labelText.text = label + " \u00A0(" + cards.Count + ")";
            return card;
        }
        else
        {
            return null;
        }
    }

    public bool Remove(Card card)
    {
        for(int i = 0; i < cards.Count;++i)
        {
            if( cards[i] == card)
            {
                cards.RemoveAt(i);
                labelText.text = label + " \u00A0(" + cards.Count + ")";
                return true;
            }
        }
        return false;
    }
    public void Clear()
    {
        cards.Clear();
        labelText.text = label + " \u00A0(" + cards.Count + ")";
    }

    public void AddCardFirst(Card card)
    {
        card.transform.parent = this.transform;
        cards.Insert(0, card);
        card.region = this;
        labelText.text = label + " \u00A0(" + cards.Count + ")";
        UpdateLayout();
    }
    public void AddCardLast(Card card)
    {
        card.transform.parent = this.transform;
        card.region = this;
        cards.Add(card);
        labelText.text = label + " \u00A0(" + cards.Count + ")";
        UpdateLayout();
    }

    public void UpdateLayout()
    {
        if (region == Region.Hand)
        {
            for (int c = 0; c < cards.Count; ++c){
                cards[c].MoveTo(this, new Vector3(c* Card.CARD_SIZE.x, 0,0));
            }
        }
        else if (region == Region.Selected)
        {
            for (int c = 0; c < cards.Count; ++c)
            {
                cards[c].MoveTo(this, new Vector3(c * Card.CARD_SIZE.x, Card.CARD_SIZE.z, 0));
            }
        }
        else if (region == Region.Deck)
        {
            for (int c = 0; c < cards.Count; ++c)
            {
                cards[c].MoveTo(this, new Vector3(0, (c+1) * Card.CARD_SIZE.z, 0));
            }
        }
        else if (region == Region.Discard)
        {
            for (int c = 0; c < cards.Count; ++c)
            {
                cards[c].MoveTo(this, new Vector3(0, (c+1) * Card.CARD_SIZE.z, 0));
            }
        }
        else if (region == Region.Exhile)
        {
            for (int c = 0; c < cards.Count; ++c)
            {
                cards[c].MoveTo(this, new Vector3(0, (c + 1) * Card.CARD_SIZE.z, 0));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector3(Card.CARD_SIZE.x, Card.CARD_SIZE.z, Card.CARD_SIZE.y));
    }
}
