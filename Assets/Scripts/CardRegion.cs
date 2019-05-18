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

    [SerializeField] Player player;
    public enum Region
    {
        Table, Hand, Selected, Deck, Exhile, Discard
    }
    [SerializeField] public Region region;
    [SerializeField] public List<Card> cards = new List<Card>();

    private void Start()
    {
        for (int c = 0; c < transform.childCount; ++c)
        {
            Card card = transform.GetChild(c).GetComponent<Card>();
            if (card)
            {
                cards.Add(card);
                card.region = this;
            }
        }
        Shuffle();
    }
    public void Shuffle()
    {
        cards.Shuffle();
    }

    public Card Draw()
    {
        if (cards.Count > 0)
        {
            Card card = cards[0];
            cards.RemoveAt(0);
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
                return true;
            }
        }
        return false;
    }
    public void Clear()
    {
        cards.Clear();
    }

    public void AddCard(Card card)
    {
        card.transform.parent = this.transform;
        cards.Add(card);

        card.region = this;
        UpdateLayout();
    }

    public void UpdateLayout()
    {
        if (region == Region.Hand)
        {
            for (int c = 0; c < cards.Count; ++c){
                cards[c].transform.localPosition = new Vector3(c* Card.CARD_SIZE.x, 0,0);
            }
        }
        else
        {
            for (int c = 0; c < cards.Count; ++c)
            {
                cards[c].transform.localPosition = Vector3.zero;
            }
        }
    }
}
