using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [SerializeField] Player player;
    public void SelectCard()
    {
        Card c = player.hand.cards[player.hand.cards.Count - 1];
        player.Select(c);
    }
}
